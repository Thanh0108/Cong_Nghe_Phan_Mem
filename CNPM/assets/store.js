/* =========================================================
   store.js
   Quản lý dữ liệu sản phẩm và giỏ hàng bằng Local Storage
   Phù hợp với đề tài chỉ làm Frontend, chưa dùng Backend
========================================================= */
const FastFoodStore = (() => {
  const STORAGE_KEY = "fastfood_cart";

  /* -------------------------------------------------------
     Dữ liệu sản phẩm mẫu
     Có thể đồng bộ với danh sách ở trang chủ
  ------------------------------------------------------- */
  const products = [
    {
      id: 1,
      name: "Burger Bò Phô Mai",
      category: "Burger",
      price: 45000,
      image:
        "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Burger bò nướng mềm, phô mai tan chảy, rau tươi giòn.",
      description:
        "Burger Bò Phô Mai là món ăn nổi bật với phần bò nướng thơm ngon, kết hợp cùng phô mai béo ngậy, rau xà lách tươi và sốt đặc trưng. Món ăn phù hợp cho bữa trưa nhanh gọn hoặc bữa tối tiện lợi."
    },
    {
      id: 2,
      name: "Burger Gà Cay",
      category: "Burger",
      price: 42000,
      image:
        "https://images.unsplash.com/photo-1550547660-d9450f859349?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Burger gà giòn cay nhẹ, lớp vỏ vàng rụm hấp dẫn.",
      description:
        "Burger Gà Cay sử dụng thịt gà chiên giòn, vị cay nhẹ vừa phải, đi kèm rau tươi và sốt mayonnaise đậm vị. Đây là lựa chọn phù hợp với người thích đồ ăn nhanh có vị đậm đà."
    },
    {
      id: 3,
      name: "Gà Rán Giòn Crispy",
      category: "Gà rán",
      price: 55000,
      image:
        "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Miếng gà rán giòn tan, thơm ngon chuẩn vị.",
      description:
        "Gà Rán Giòn Crispy có lớp vỏ giòn rụm, thịt bên trong mềm và giữ được độ mọng. Sản phẩm thích hợp dùng riêng hoặc kết hợp cùng khoai tây chiên và nước ngọt."
    },
    {
      id: 4,
      name: "Gà Rán Sốt Cay",
      category: "Gà rán",
      price: 59000,
      image:
        "https://images.unsplash.com/photo-1513639776629-7b61b0ac49cb?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Gà rán phủ sốt cay đậm vị, hấp dẫn hơn mỗi miếng.",
      description:
        "Gà Rán Sốt Cay mang đến trải nghiệm đậm đà với lớp sốt cay ngọt phủ bên ngoài miếng gà giòn. Đây là món phù hợp với người yêu thích hương vị mạnh và bắt mắt."
    },
    {
      id: 5,
      name: "Pepsi Lạnh",
      category: "Đồ uống",
      price: 15000,
      image:
        "https://images.unsplash.com/photo-1581006852262-e4307cf6283a?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Nước ngọt có gas mát lạnh, giải khát nhanh chóng.",
      description:
        "Pepsi Lạnh là đồ uống quen thuộc, giúp cân bằng vị giác khi dùng cùng burger hoặc gà rán. Sản phẩm phù hợp cho các combo đồ ăn nhanh."
    },
    {
      id: 6,
      name: "Trà Chanh",
      category: "Đồ uống",
      price: 18000,
      image:
        "https://images.unsplash.com/photo-1499638673689-79a0b5115d87?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Trà chanh thanh mát, dễ uống, phù hợp nhiều món ăn.",
      description:
        "Trà Chanh có vị chua nhẹ dễ chịu, giúp giảm cảm giác ngấy khi dùng đồ chiên rán. Đây là lựa chọn phổ biến của nhiều khách hàng trẻ."
    },
    {
      id: 7,
      name: "Combo Burger + Khoai + Nước",
      category: "Combo",
      price: 89000,
      image:
        "https://images.unsplash.com/photo-1512152272829-e3139592d56f?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Combo tiết kiệm gồm burger, khoai tây và nước uống.",
      description:
        "Combo Burger + Khoai + Nước là lựa chọn tiện lợi cho một bữa ăn đầy đủ. Sản phẩm phù hợp với học sinh, sinh viên và nhân viên văn phòng."
    },
    {
      id: 8,
      name: "Combo Gà Rán Gia Đình",
      category: "Combo",
      price: 149000,
      image:
        "https://images.unsplash.com/photo-1606755962773-d324e0a13086?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Combo nhiều phần, phù hợp nhóm bạn hoặc gia đình nhỏ.",
      description:
        "Combo Gà Rán Gia Đình gồm nhiều miếng gà rán và món phụ, phù hợp cho 2 đến 4 người. Đây là lựa chọn tối ưu về chi phí và số lượng."
    },
    {
      id: 9,
      name: "Khoai Tây Chiên Lớn",
      category: "Món phụ",
      price: 32000,
      image:
        "https://images.unsplash.com/photo-1518013431117-eb1465fa5752?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Khoai chiên giòn rụm, vàng óng, size lớn.",
      description:
        "Khoai Tây Chiên Lớn với lớp vỏ giòn và ruột khoai bùi, được tẩm muối vừa ăn. Thích hợp ăn kèm burger, gà rán hoặc dùng như món snack."
    },
    {
      id: 10,
      name: "Khoai Tây Lắc Phô Mai",
      category: "Món phụ",
      price: 35000,
      image:
        "https://cdn-www.vinid.net/e163f30e-khoai-tay-lac-pho-mai-7.jpg",
      shortDescription: "Khoai chiên lắc bột phô mai thơm béo.",
      description:
        "Khoai Tây Lắc Phô Mai là khoai chiên nóng hổi được lắc cùng bột phô mai thơm béo. Món phụ cực hợp để nâng cấp trải nghiệm ăn combo."
    },
    {
      id: 11,
      name: "Burger Cá Giòn",
      category: "Burger",
      price: 48000,
      image:
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRvXAHdISsoGm_dTL2O0lDoHe0qIApg9HZ0wQ&s",
      shortDescription: "Burger cá phi-lê chiên giòn, sốt tartar nhẹ.",
      description:
        "Burger Cá Giòn sử dụng phi-lê cá chiên giòn vàng, ăn kèm sốt tartar và rau tươi. Lựa chọn phù hợp cho ai thích vị thanh nhẹ hơn burger bò."
    },
    {
      id: 12,
      name: "Nuggets Gà 6 Miếng",
      category: "Món phụ",
      price: 39000,
      image:
        "https://images.unsplash.com/photo-1604908554105-088645debe26?auto=format&fit=crop&w=900&q=80",
      shortDescription: "Nuggets gà giòn mềm, chấm sốt tùy chọn.",
      description:
        "Nuggets Gà 6 Miếng có lớp vỏ giòn nhẹ, thịt gà mềm và thơm. Phù hợp cho trẻ em hoặc dùng kèm khi bạn muốn thêm món phụ nhanh gọn."
    },
    {
      id: 13,
      name: "Gà Popcorn",
      category: "Gà rán",
      price: 45000,
      image:
        "https://th.bing.com/th/id/OIP.kUvR4FXy38G4U_XX9sIBjQHaEK?w=266&h=180&c=7&r=0&o=7&dpr=1.3&pid=1.7&rm=3",
      shortDescription: "Gà viên nhỏ giòn rụm, ăn vặt cực cuốn.",
      description:
        "Gà Popcorn là những miếng gà nhỏ được tẩm bột và chiên giòn. Dễ ăn, tiện chia sẻ, phù hợp mang đi hoặc dùng cùng nước uống mát lạnh."
    },
    {
      id: 14,
      name: "Coca Cola Lạnh",
      category: "Đồ uống",
      price: 15000,
      image:
        "https://th.bing.com/th/id/OIF.0yOKWHaTURRmwVQZnw5ePA?w=263&h=185&c=7&r=0&o=7&dpr=1.3&pid=1.7&rm=3",
      shortDescription: "Coca mát lạnh, có gas, giải khát sảng khoái.",
      description:
        "Coca Cola Lạnh giúp bữa ăn thêm tròn vị, đặc biệt khi dùng cùng món chiên. Lựa chọn thay thế cho Pepsi tuỳ khẩu vị."
    },
    {
      id: 15,
      name: "Kem Sundae Socola",
      category: "Tráng miệng",
      price: 25000,
      image:
        "https://wallpaper.dog/large/5500101.jpg",
      shortDescription: "Kem mịn lạnh, phủ sốt socola ngọt ngào.",
      description:
        "Kem Sundae Socola là món tráng miệng mát lạnh với kem vani mịn và sốt socola đậm vị. Phù hợp kết thúc bữa ăn hoặc giải nhiệt."
    },
    {
      id: 16,
      name: "Combo Cặp Đôi Burger",
      category: "Combo",
      price: 129000,
      image:
        "https://tse3.mm.bing.net/th/id/OIP.MnsorRbUXnHl9KU8Q6kz8AHaHa?rs=1&pid=ImgDetMain&o=7&rm=3",
      shortDescription: "2 burger + 2 khoai + 2 nước, dành cho 2 người.",
      description:
        "Combo Cặp Đôi Burger gồm 2 burger, 2 phần khoai tây và 2 nước uống. Tiết kiệm hơn khi đi ăn cùng bạn bè hoặc người thân."
    }
  ];

  /* -------------------------------------------------------
     Tùy chọn thêm mặc định cho sản phẩm
  ------------------------------------------------------- */
  const extraOptions = [
    { id: "extra-cheese", label: "Thêm phô mai", price: 10000 },
    { id: "extra-sauce", label: "Thêm sốt đặc biệt", price: 5000 }
  ];

  /* -------------------------------------------------------
     Hàm định dạng tiền VNĐ
  ------------------------------------------------------- */
  function formatPrice(value) {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND"
    }).format(value);
  }

  /* -------------------------------------------------------
     Lấy toàn bộ danh sách sản phẩm
  ------------------------------------------------------- */
  function getProducts() {
    return products;
  }

  /* -------------------------------------------------------
     Tìm sản phẩm theo ID
  ------------------------------------------------------- */
  function getProductById(id) {
    return products.find((item) => item.id === Number(id));
  }

  /* -------------------------------------------------------
     Lấy danh sách tùy chọn thêm
  ------------------------------------------------------- */
  function getDefaultOptions() {
    return extraOptions.map((item) => ({ ...item }));
  }

  /* -------------------------------------------------------
     Lấy dữ liệu giỏ hàng từ Local Storage
  ------------------------------------------------------- */
  function getCart() {
    try {
      return JSON.parse(localStorage.getItem(STORAGE_KEY)) || [];
    } catch (error) {
      return [];
    }
  }

  /* -------------------------------------------------------
     Lưu giỏ hàng vào Local Storage
  ------------------------------------------------------- */
  function saveCart(cart) {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(cart));
    updateCartBadge();
  }

  /* -------------------------------------------------------
     Xóa toàn bộ giỏ hàng
  ------------------------------------------------------- */
  function clearCart() {
    localStorage.removeItem(STORAGE_KEY);
    updateCartBadge();
  }

  /* -------------------------------------------------------
     Tính tổng phụ phí từ tùy chọn thêm
  ------------------------------------------------------- */
  function getOptionsTotal(selectedOptions = []) {
    return selectedOptions.reduce((sum, item) => sum + Number(item.price || 0), 0);
  }

  /* -------------------------------------------------------
     Tạo mã phân biệt từng món trong giỏ
     Ví dụ: cùng Burger nhưng khác option thì là 2 dòng khác nhau
  ------------------------------------------------------- */
  function buildCartItemId(productId, selectedOptions = []) {
    const optionKey =
      selectedOptions.map((item) => item.id).sort().join("|") || "default";
    return `${productId}-${optionKey}`;
  }

  /* -------------------------------------------------------
     Tính đơn giá của 1 sản phẩm trong giỏ
     = giá cơ bản + giá option thêm
  ------------------------------------------------------- */
  function getUnitPrice(item) {
    return Number(item.basePrice) + getOptionsTotal(item.selectedOptions);
  }

  /* -------------------------------------------------------
     Tính thành tiền của 1 dòng sản phẩm
  ------------------------------------------------------- */
  function getItemSubtotal(item) {
    return getUnitPrice(item) * Number(item.quantity);
  }

  /* -------------------------------------------------------
     Tính tổng số lượng và tổng tiền toàn giỏ
  ------------------------------------------------------- */
  function getCartSummary() {
    const cart = getCart();

    return cart.reduce(
      (result, item) => {
        result.totalQuantity += Number(item.quantity);
        result.totalPrice += getItemSubtotal(item);
        return result;
      },
      {
        totalQuantity: 0,
        totalPrice: 0
      }
    );
  }

  /* -------------------------------------------------------
     Thêm sản phẩm vào giỏ
     Nếu sản phẩm + option giống nhau thì cộng dồn số lượng
  ------------------------------------------------------- */
  function addToCart({ product, quantity = 1, selectedOptions = [] }) {
    const cart = getCart();
    const cartItemId = buildCartItemId(product.id, selectedOptions);

    const existedItem = cart.find((item) => item.cartItemId === cartItemId);

    if (existedItem) {
      existedItem.quantity += Number(quantity);
    } else {
      cart.push({
        cartItemId,
        productId: product.id,
        name: product.name,
        image: product.image,
        basePrice: product.price,
        quantity: Number(quantity),
        selectedOptions
      });
    }

    saveCart(cart);
  }

  /* -------------------------------------------------------
     Thêm nhanh sản phẩm vào giỏ (không có option)
     Hữu ích nếu muốn gọi từ trang chủ
  ------------------------------------------------------- */
  function addSimpleProduct(productId, quantity = 1) {
    const product = getProductById(productId);
    if (!product) return;

    addToCart({
      product,
      quantity,
      selectedOptions: []
    });
  }

  /* -------------------------------------------------------
     Cập nhật số lượng 1 sản phẩm trong giỏ
     Nếu <= 0 thì xóa dòng đó
  ------------------------------------------------------- */
  function updateItemQuantity(cartItemId, nextQuantity) {
    const cart = getCart()
      .map((item) => {
        if (item.cartItemId === cartItemId) {
          item.quantity = Number(nextQuantity);
        }
        return item;
      })
      .filter((item) => item.quantity > 0);

    saveCart(cart);
  }

  /* -------------------------------------------------------
     Xóa 1 sản phẩm khỏi giỏ
  ------------------------------------------------------- */
  function removeItem(cartItemId) {
    const cart = getCart().filter((item) => item.cartItemId !== cartItemId);
    saveCart(cart);
  }

  /* -------------------------------------------------------
     Chuyển danh sách option thành chuỗi hiển thị
  ------------------------------------------------------- */
  function getSelectedOptionsText(selectedOptions = []) {
    if (!selectedOptions.length) return "Không có tùy chọn thêm";
    return selectedOptions
      .map((item) => `${item.label} (+${formatPrice(item.price)})`)
      .join(", ");
  }

  /* -------------------------------------------------------
     Cập nhật badge số lượng giỏ hàng trên header
  ------------------------------------------------------- */
  function updateCartBadge(selector = "[data-cart-count]") {
    const { totalQuantity } = getCartSummary();
    document.querySelectorAll(selector).forEach((element) => {
      element.textContent = totalQuantity;
    });
  }

  return {
    getProducts,
    getProductById,
    getDefaultOptions,
    getCart,
    saveCart,
    clearCart,
    formatPrice,
    getOptionsTotal,
    getUnitPrice,
    getItemSubtotal,
    getCartSummary,
    addToCart,
    addSimpleProduct,
    updateItemQuantity,
    removeItem,
    getSelectedOptionsText,
    updateCartBadge
  };
})();

/* ---------------------------------------------------------
   Khi tải trang, tự cập nhật badge giỏ hàng
--------------------------------------------------------- */
document.addEventListener("DOMContentLoaded", () => {
  FastFoodStore.updateCartBadge();
});