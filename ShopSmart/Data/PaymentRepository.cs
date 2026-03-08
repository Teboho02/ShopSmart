namespace ShopSmart.Data;

using ShopSmart.Models;

public class PaymentRepository
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "data", "payments.json");

    private readonly AppData _data;
    private int _nextId = 1;

    public PaymentRepository(AppData data)
    {
        _data = data;

        var loaded = JsonFileStore.Load<Payment>(FilePath);
        _data.Payments.AddRange(loaded);

        if (loaded.Count > 0)
            _nextId = loaded.Max(p => p.Id) + 1;
    }

    public void Add(Payment payment)
    {
        _data.Payments.Add(payment);
        JsonFileStore.Save(FilePath, _data.Payments);
    }

    public Payment? FindByOrderId(int orderId) =>
        _data.Payments.FirstOrDefault(p => p.OrderId == orderId);

    public int NextPaymentId() => _nextId++;
}
