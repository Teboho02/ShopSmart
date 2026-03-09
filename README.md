# ShopSmart

A console-based e-commerce application built with C# and .NET 10. ShopSmart supports two roles — **Customer** and **Administrator** — each with their own menu and feature set. All data is persisted to JSON files so state survives between sessions.

---

## Features

### Customer
| Feature | Description |
|---|---|
| Register / Login | Create an account or sign in with username and password |
| Browse Products | View all available products grouped by category |
| Search Products | Search by product name, description, or category |
| Shopping Cart | Add, update, and remove items |
| Checkout | Pay from wallet balance; stock is reserved at checkout |
| Wallet | View balance and top up funds |
| Order History | View past orders with itemised details and payment info |
| Track Order | Visual status pipeline for any order |
| Review Products | Leave a star rating and comment on delivered items |

### Administrator
| Feature | Description |
|---|---|
| Add Product | Create a new product with name, description, category, price, and stock |
| Update Product | Edit any field on an existing product |
| Delete Product | Soft-delete a product (hidden from customers, visible in admin views) |
| Restock Product | Add stock units to an active product |
| View All Products | See every product including soft-deleted ones, with Active/Inactive status |
| Update Order Status | Advance or change any non-terminal order through Pending → Processing → Shipped → Delivered / Cancelled |

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run
```bash
git clone https://github.com/Teboho02/ShopSmart.git
cd ShopSmart/ShopSmart
dotnet run
```


## Data Persistence

All data is stored as JSON in a `data/` folder next to the executable:

| File | Contents |
|---|---|
| `users.json` | Accounts, roles, wallet balances |
| `products.json` | Catalogue including soft-deleted products |
| `orders.json` | All orders and their current status |
| `payments.json` | Payment records linked to orders |
| `carts.json` | Active cart items per user |
| `reviews.json` | Product reviews submitted by customers |

Delete any file to reset that data. Delete all files to return to the seeded state on next run.

---

## Project Structure

```
ShopSmart/
├── Enums/
│   ├── OrderStatus.cs       # Pending, Processing, Shipped, Delivered, Cancelled
│   ├── PaymentStatus.cs
│   └── UserRole.cs          # Customer, Administrator
│
├── Models/
│   ├── User.cs
│   ├── Product.cs           # IsActive flag supports soft-delete
│   ├── CartItem.cs
│   ├── OrderItem.cs         # Snapshot of product name/price at purchase time
│   ├── Order.cs
│   ├── Payment.cs
│   └── Review.cs
│
├── Data/
│   ├── AppData.cs           # Shared in-memory store (Unit of Work)
│   ├── JsonFileStore.cs     # Generic JSON load/save utility
│   ├── UserRepository.cs
│   ├── ProductRepository.cs
│   ├── CartRepository.cs
│   ├── OrderRepository.cs
│   ├── PaymentRepository.cs
│   └── ReviewRepository.cs
│
├── Services/
│   ├── ValidationException.cs
│   ├── IUserService.cs / UserService.cs
│   ├── IProductService.cs / ProductService.cs
│   ├── ICartService.cs / CartService.cs
│   ├── IOrderService.cs / OrderService.cs
│   └── IReviewService.cs / ReviewService.cs
│
├── UI/
│   ├── ConsoleHelper.cs     # Coloured output, prompts, masked password input
│   ├── MenuRenderer.cs      # Consistent numbered-menu rendering
│   ├── MainMenuView.cs
│   ├── CustomerMenuView.cs
│   ├── AdminMenuView.cs
│   └── ...                  # One view file per feature
│
└── Program.cs               # Composition root — wires all dependencies and starts the app
```

---

## Architecture

The codebase follows a strict **layered architecture**:

```
UI  →  Services  →  Data  →  Models
```

- Each layer only depends on the layer below it — never above.
- All dependencies are passed through **constructor injection**; `new` is only called in `Program.cs`.
- Services depend on **interfaces** (`IProductService`, `IOrderService`, etc.), not concrete classes.
- Business rules and validation live exclusively in the service layer. Views handle only input and output.

---

