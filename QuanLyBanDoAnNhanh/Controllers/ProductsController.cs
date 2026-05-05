using Microsoft.AspNetCore.Mvc;
using QuanLyBanDoAnNhanh.Data;
using Microsoft.EntityFrameworkCore;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Trang danh sách món ăn
    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        return View(products);
    }

    // Trang chi tiết món ăn
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // Gửi đánh giá món ăn
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(int productId, string userName, int rating, string comment)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(comment) || rating < 1 || rating > 5)
        {
            return RedirectToAction(nameof(Details), new { id = productId });
        }

        var review = new QuanLyBanDoAnNhanh.Models.Review
        {
            ProductId = productId,
            UserName = userName,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = productId });
    }
}