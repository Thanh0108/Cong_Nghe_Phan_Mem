using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDoAnNhanh.Data;
using QuanLyBanDoAnNhanh.Models;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBanDoAnNhanh.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kiểm tra quyền Admin trước khi thực hiện bất kỳ Action nào
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.RouteValues["action"];
            if (actionName == "Login")
            {
                base.OnActionExecuting(context);
                return;
            }

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                // Nếu không phải Admin, chuyển hướng về trang đăng nhập Admin
                context.Result = new RedirectToActionResult("Login", "Admin", null);
                return;
            }
            base.OnActionExecuting(context);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserRole") == "Admin")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                return View(model);
            }

            if (user.Role != "Admin")
            {
                ModelState.AddModelError("", "Bạn không có quyền truy cập trang quản trị.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            TempData["SuccessMessage"] = $"Chào mừng Admin {user.FullName}!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var productCount = await _context.Products.CountAsync();
            var userCount = await _context.Users.CountAsync();
            var categoryCount = await _context.Categories.CountAsync();

            ViewBag.ProductCount = productCount;
            ViewBag.UserCount = userCount;
            ViewBag.CategoryCount = categoryCount;

            return View();
        }

        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders(int id)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == id)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new {
                    id = o.Id,
                    orderDate = o.OrderDate.ToString("dd/MM/yyyy HH:mm"),
                    totalAmount = o.TotalAmount,
                    status = o.Status,
                    details = o.OrderDetails.Select(od => new {
                        productName = od.Product != null ? od.Product.Name : "Sản phẩm không tồn tại",
                        quantity = od.Quantity,
                        unitPrice = od.UnitPrice
                    }).ToList()
                })
                .ToListAsync();

            return Json(orders);
        }

        // ===== Helper Methods =====
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}
