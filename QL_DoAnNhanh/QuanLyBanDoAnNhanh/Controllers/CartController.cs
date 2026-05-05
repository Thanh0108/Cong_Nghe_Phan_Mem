using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDoAnNhanh.Data;
using QuanLyBanDoAnNhanh.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyBanDoAnNhanh.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt món!";
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var product = _context.Products.Find(productId);
            if (product != null)
            {
                var item = cart.FirstOrDefault(p => p.ProductId == productId);
                if (item == null)
                {
                    cart.Add(new CartItem
                    {
                        ProductId = productId,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = quantity
                    });
                }
                else
                {
                    item.Quantity += quantity;
                }
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        // GET: Trang thanh toán
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để thanh toán!";
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống, không thể thanh toán!";
                return RedirectToAction("Index");
            }

            // Lấy thông tin user để điền sẵn form
            var user = _context.Users.Find(userId.Value);
            if (user != null)
            {
                ViewBag.UserName = user.FullName;
                ViewBag.UserPhone = user.Phone;
                ViewBag.UserAddress = user.Address;
                ViewBag.UserEmail = user.Email;
            }

            return View(cart);
        }

        // POST: Xử lý đặt hàng
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string fullName, string address, string phone, string note, string paymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(i => i.Total),
                FullName = fullName,
                Address = address,
                Phone = phone,
                Note = note,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();

            // Xóa giỏ hàng
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

        // GET: Trang xác nhận đơn hàng
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId.Value);

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
    }
}
