using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using QuanLyBanDoAnNhanh.Data;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchTerm, int? categoryId)
    {
        var query = _context.Products
                            .Include(p => p.Category)
                            .AsQueryable();

        // 🔍 Tìm kiếm (không phân biệt hoa thường + tránh null)
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var keyword = searchTerm.ToLower();

            query = query.Where(p =>
                p.Name.ToLower().Contains(keyword) ||
                (p.Description != null && p.Description.ToLower().Contains(keyword))
            );
        }

        // 📂 Lọc theo danh mục
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        // 📌 Truyền dữ liệu sang View
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.SearchTerm = searchTerm;
        ViewBag.BestSellers = await _context.Products.Include(p => p.Category).OrderByDescending(p => p.Id).Take(6).ToListAsync();
        ViewBag.TotalProducts = await _context.Products.CountAsync();

        return View(await query.ToListAsync());
    }

    public async Task<IActionResult> Menu()
    {
        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(products);
    }

    public IActionResult Contact()
    {
        return View();
    }
}