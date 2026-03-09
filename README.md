![.NET Build](https://img.shields.io/badge/build-passing-brightgreen) ![.NET 10](https://img.shields.io/badge/.NET-10-512BD4) ![xUnit Tests](https://img.shields.io/badge/tests-103%20passing-brightgreen)

# ShopSmart

## What is ShopSmart?

ShopSmart is a console-based e-commerce application built with **C# and .NET 10**. It supports two roles — **Customer** and **Administrator** — each with a dedicated menu and feature set. Customers can browse products, manage a shopping cart, check out using multiple payment methods, track orders, and leave reviews. Administrators can manage the product catalogue and advance order statuses. All data is persisted to JSON files so state survives between sessions.

## Why Choose ShopSmart?

- **Clean architecture** — strict layered design (UI → Services → Data → Models) with no layer bypassing another
- **Multiple payment methods** — Wallet, EFT, PayPal, and Voucher strategies via the Strategy design pattern
- **Order state management** — sequential order lifecycle (Pending → Processing → Shipped → Delivered) enforced by the State design pattern
- **Data integrity** — payment is processed before stock is reduced, preventing inconsistent state on failure
- **Extensible design** — new payment methods or order states can be added without touching existing code
- **Full test coverage** — 103 xUnit tests covering all services, payment strategies, and order states
- **JSON persistence** — lightweight file-based storage; no database setup required

---

# Documentation

## Software Requirement Specification

### Overview

ShopSmart is a command-line e-commerce platform. Users register and log in with a username and password. Customers top up a wallet, browse and search a product catalogue, add items to a cart, and check out using a payment method of their choice. Administrators manage the catalogue and fulfil orders by advancing their status through a defined lifecycle. All business rules are validated in the service layer; views handle only input and output.

### Components and functional requirements

**1. Authentication**

- Users must register with a unique username, unique email address, and a password of at least 6 characters
- Passwords are stored as hashed values; plain-text passwords are never persisted
- Login accepts username (case-insensitive) and password; invalid credentials throw a validation error
- Each user is assigned a role: `Customer` or `Administrator`

**2. Product Catalogue (Administrator)**

- Administrators can add a product with name, description, category, price, and stock quantity
- Products can be updated (all fields) or soft-deleted (hidden from customers, retained in admin views)
- Deleted products can be restocked to make them active again
- All products (active and inactive) are visible to administrators with a clear Active/Inactive status indicator

**3. Product Browsing and Search (Customer)**

- Customers see only active products, grouped by category
- Search matches against product name, description, and category (case-insensitive)
- Product detail shows current price, available stock, and average review rating

**4. Shopping Cart (Customer)**

- Customers add products to a cart; adding an existing product increments its quantity
- The unit price is snapshotted at the time of adding — subsequent price changes do not affect the cart
- Quantity can be updated (setting it to 0 removes the item); quantity cannot exceed available stock
- Inactive (deleted) products cannot be added to the cart

**5. Checkout and Payment (Customer)**

- Checkout requires a non-empty cart; the customer selects a payment method before confirming
- **Wallet** — deducts the total from the user's wallet balance; requires sufficient funds
- **EFT** — displays bank details; order is placed with `Pending` payment status awaiting external confirmation
- **PayPal** — prompts for a PayPal email address (must contain `@`); payment is simulated as completed
- **Voucher** — prompts for a voucher code (case-insensitive lookup); if the voucher value covers the total, the wallet is untouched; if partial, the remainder is charged to the wallet; redeemed or invalid codes are rejected
- Payment is processed before stock is reduced — a failed payment leaves stock unchanged
- On success the cart is cleared and an order record is created with payment method and status

**6. Wallet (Customer)**

- Customers can view their current wallet balance
- Wallet can be topped up with any positive amount

**7. Order History and Tracking (Customer)**

- Customers view their own order history, most recent first
- Each order shows the order date, total, payment method, payment status, itemised products, and current order status
- A visual status pipeline shows where an order sits in its lifecycle

**8. Order Status Management (Administrator)**

- Administrators can advance any non-terminal order through its lifecycle: `Pending → Processing → Shipped → Delivered`
- Any non-terminal order can also be cancelled
- Terminal orders (`Delivered`, `Cancelled`) cannot be advanced or cancelled — attempting to do so throws a validation error

**9. Reviews (Customer)**

- Customers can review a product after it appears in a delivered order
- Reviews consist of a star rating (1–5) and a comment
- Average rating is displayed on the product detail screen

**10. Vouchers**

- Voucher codes are seeded at startup (`SAVE10`, `SAVE50`, `WELCOME`, `FREESHIP`)
- Each voucher has a monetary value and can only be redeemed once
- Voucher lookup is case-insensitive; attempting to reuse a redeemed voucher throws a validation error

---

# Design

## [Use Case Diagram](docs/use-case-diagram.png)

> Customer and Administrator actors with their respective use cases.

## [Domain Model](docs/domain-model.png)

> Entities: User, Product, CartItem, Order, OrderItem, Payment, Review, Voucher and their relationships.

## [State Diagram — Order Lifecycle](docs/order-state-diagram.png)

> Pending → Processing → Shipped → Delivered, with Cancelled reachable from any non-terminal state.

## [Architecture Diagram](docs/architecture-diagram.png)

> Layered architecture: UI → Services → Data → Models, with constructor injection wired in Program.cs.

---

# Running application

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Application

```bash
git clone https://github.com/Teboho02/ShopSmart.git
cd ShopSmart/ShopSmart
dotnet run
```

### Tests

```bash
cd ShopSmart/ShopSmart.Tests
dotnet test
```

### Development

```bash
cd ShopSmart/ShopSmart
dotnet build
dotnet watch run
```
