namespace ShopSmart.Services.States;

using ShopSmart.Enums;

public interface IOrderState
{
    OrderStatus  Status    { get; }
    OrderStatus? Next      { get; }   // null for terminal states
    bool         CanCancel { get; }   // false for terminal states
    string?      NextLabel { get; }   // e.g. "Advance to Processing"
}
