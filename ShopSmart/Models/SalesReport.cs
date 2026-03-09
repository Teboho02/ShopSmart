namespace ShopSmart.Models;

using ShopSmart.Enums;

public record SalesReport(
    int                                                                  TotalOrders,
    decimal                                                              TotalRevenue,
    decimal                                                              AverageOrderValue,
    IReadOnlyList<(OrderStatus Status, int Count)>                       OrdersByStatus,
    IReadOnlyList<(string ProductName, int UnitsSold, decimal Revenue)>  TopProducts
);
