using QuanLyBanDoAnNhanh.Data;
using QuanLyBanDoAnNhanh.Models;
using System.Linq;

namespace QuanLyBanDoAnNhanh.SeedData
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // 0. Thêm Admin mặc định nếu chưa có
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    FullName = "Administrator",
                    Email = "admin@fastbite.com",
                    Phone = "0123456789",
                    Address = "Hệ thống",
                    PasswordHash = HashPassword("Admin@123"), // Mật khẩu mặc định
                    Role = "Admin",
                    CreatedAt = System.DateTime.Now
                };
                context.Users.Add(admin);
                context.SaveChanges();
            }

            // Nếu đã có sản phẩm thì không nạp dữ liệu ban đầu
            if (!context.Products.Any())
            {
                // 1. Thêm các Danh mục (Categories)
                var catPizza = new Category { Name = "PIZZA" };
                var catPasta = new Category { Name = "MÌ Ý" };
                var catChicken = new Category { Name = "GÀ CHIÊN" };
                var catDrink = new Category { Name = "NƯỚC UỐNG" };

                context.Categories.AddRange(catPizza, catPasta, catChicken, catDrink);
                context.SaveChanges();

                // 2. Thêm các Sản phẩm (Products)
                context.Products.AddRange(
                    // Nhóm Pizza
                    new Product { Name = "Big Pizza Thập Cẩm", Price = 89000, Description = "Đầy đủ topping thịt nguội, xá xíu", CategoryId = catPizza.Id, ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Pizza Hải Sản Đút Lò", Price = 129000, Description = "Tôm, mực tươi cùng phô mai Mozzarella", CategoryId = catPizza.Id, ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?q=80&w=1000&auto=format&fit=crop" },

                    // Nhóm Mì Ý
                    new Product { Name = "Mì Ý Bò Bằm", Price = 55000, Description = "Sốt cà chua truyền thống và bò bằm", CategoryId = catPasta.Id, ImageUrl = "https://images.unsplash.com/photo-1546549032-9571cd6b27df?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Mì Ý Hải Sản", Price = 75000, Description = "Mì Ý xào cùng hải sản sốt kem", CategoryId = catPasta.Id, ImageUrl = "https://images.unsplash.com/photo-1551183053-bf91a1d81141?q=80&w=1000&auto=format&fit=crop" },

                    // Nhóm Gà Chiên
                    new Product { Name = "Gà Tẩm Bột Phô Mai", Price = 45000, Description = "Gà chiên giòn rắc bột phô mai", CategoryId = catChicken.Id, ImageUrl = "https://images.unsplash.com/photo-1562967914-608f82629710?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Gà Không Xương BBQ", Price = 49000, Description = "Gà phi lê sốt BBQ đậm đà", CategoryId = catChicken.Id, ImageUrl = "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?q=80&w=1000&auto=format&fit=crop" },

                    // Nhóm Burger
                    new Product { Name = "Double Cheese Burger", Price = 55000, Description = "Bò nướng 2 lớp phô mai", CategoryId = catPizza.Id, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?q=80&w=1000&auto=format&fit=crop" },

                    // Nhóm Nước uống
                    new Product { Name = "Coca-Cola Lon", Price = 15000, Description = "Nước giải khát có gas", CategoryId = catDrink.Id, ImageUrl = "https://images.unsplash.com/photo-1622483767028-3f66f32aef97?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Trà Đào Cam Sả", Price = 25000, Description = "Trà trái cây thanh mát", CategoryId = catDrink.Id, ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?q=80&w=1000&auto=format&fit=crop" }
                );

                context.SaveChanges();
            }

            // 3. Thêm 9 món mới nếu chưa có (kiểm tra theo tên sản phẩm)
            if (!context.Products.Any(p => p.Name == "Pizza Pepperoni Đặc Biệt"))
            {
                // Lấy ID danh mục hiện có
                var pizzaId = context.Categories.First(c => c.Name == "PIZZA").Id;
                var pastaId = context.Categories.First(c => c.Name == "MÌ Ý").Id;
                var chickenId = context.Categories.First(c => c.Name == "GÀ CHIÊN").Id;
                var drinkId = context.Categories.First(c => c.Name == "NƯỚC UỐNG").Id;

                context.Products.AddRange(
                    // Thêm Pizza
                    new Product { Name = "Pizza Pepperoni Đặc Biệt", Price = 109000, Description = "Xúc xích Pepperoni, ớt chuông, phô mai Mozzarella thơm lừng", CategoryId = pizzaId, ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Pizza Bò & Nấm Truffle", Price = 139000, Description = "Thịt bò Úc kết hợp nấm truffle, sốt kem đặc biệt", CategoryId = pizzaId, ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?q=80&w=1000&auto=format&fit=crop" },

                    // Thêm Mì Ý
                    new Product { Name = "Mì Ý Carbonara", Price = 65000, Description = "Mì Ý sốt kem trứng, thịt xông khói, phô mai Parmesan", CategoryId = pastaId, ImageUrl = "https://images.unsplash.com/photo-1612874742237-6526221588e3?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Mì Ý Sốt Pesto Gà", Price = 69000, Description = "Mì Ý sốt pesto húng quế, ức gà nướng, cà chua bi", CategoryId = pastaId, ImageUrl = "https://images.unsplash.com/photo-1473093295043-cdd812d0e601?q=80&w=1000&auto=format&fit=crop" },

                    // Thêm Gà Chiên
                    new Product { Name = "Gà Chiên Giòn Cay", Price = 52000, Description = "Gà chiên giòn tẩm ớt cay, ăn kèm sốt tương ớt", CategoryId = chickenId, ImageUrl = "https://images.unsplash.com/photo-1614398751058-eb2e0bf63e53?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Cánh Gà Chiên Mắm Tỏi", Price = 55000, Description = "Cánh gà chiên giòn rim mắm tỏi đậm vị", CategoryId = chickenId, ImageUrl = "https://images.unsplash.com/photo-1567620832903-9fc6debc209f?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Gà Viên Chiên Phô Mai", Price = 39000, Description = "Gà viên nhân phô mai tan chảy, chiên vàng giòn", CategoryId = chickenId, ImageUrl = "https://images.unsplash.com/photo-1562967916-eb82221dfb92?q=80&w=1000&auto=format&fit=crop" },

                    // Thêm Nước uống
                    new Product { Name = "Sinh Tố Xoài", Price = 30000, Description = "Sinh tố xoài tươi nguyên chất, mát lạnh", CategoryId = drinkId, ImageUrl = "https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?q=80&w=1000&auto=format&fit=crop" },
                    new Product { Name = "Nước Ép Cam Tươi", Price = 28000, Description = "Cam tươi ép nguyên chất, giàu vitamin C", CategoryId = drinkId, ImageUrl = "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?q=80&w=1000&auto=format&fit=crop" }
                );

                context.SaveChanges();
            }

            // 4. Thêm Đánh giá mẫu nếu chưa có
            if (!context.Reviews.Any())
            {
                var products = context.Products.Take(5).ToList();
                foreach (var product in products)
                {
                    context.Reviews.AddRange(
                        new Review { ProductId = product.Id, UserName = "Nguyễn Văn A", Rating = 5, Comment = "Món này rất ngon, giao hàng nhanh!", CreatedAt = System.DateTime.Now.AddDays(-2) },
                        new Review { ProductId = product.Id, UserName = "Trần Thị B", Rating = 4, Comment = "Vị vừa ăn, giá cả hợp lý.", CreatedAt = System.DateTime.Now.AddDays(-1) }
                    );
                }
                context.SaveChanges();
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(bytes);
        }
    }
}