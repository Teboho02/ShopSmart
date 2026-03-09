using ShopSmart.Data;
using ShopSmart.Services;
using ShopSmart.Services.Payments;

/// <summary>
/// Base class for all tests. Creates a fresh in-memory store and
/// wipes any leftover JSON data files before each test so tests
/// are fully isolated from one another.
/// </summary>
public abstract class TestBase
{
    protected readonly AppData            AppData;
    protected readonly UserRepository     UserRepo;
    protected readonly ProductRepository  ProductRepo;
    protected readonly CartRepository     CartRepo;
    protected readonly OrderRepository    OrderRepo;
    protected readonly PaymentRepository  PaymentRepo;
    protected readonly ReviewRepository   ReviewRepo;
    protected readonly VoucherRepository  VoucherRepo;

    protected readonly UserService           UserService;
    protected readonly ProductService        ProductService;
    protected readonly CartService           CartService;
    protected readonly OrderService          OrderService;
    protected readonly PaymentStrategyFactory PaymentFactory;

    protected TestBase()
    {
        CleanTestData();

        AppData     = new AppData();
        UserRepo    = new UserRepository(AppData);
        ProductRepo = new ProductRepository(AppData);
        CartRepo    = new CartRepository(AppData);
        OrderRepo   = new OrderRepository(AppData);
        PaymentRepo = new PaymentRepository(AppData);
        ReviewRepo  = new ReviewRepository(AppData);
        VoucherRepo = new VoucherRepository(AppData);

        UserService    = new UserService(UserRepo);
        ProductService = new ProductService(ProductRepo);
        CartService    = new CartService(CartRepo, ProductService);
        OrderService   = new OrderService(CartRepo, ProductService, ProductRepo,
                                          OrderRepo, PaymentRepo, UserRepo);
        PaymentFactory = new PaymentStrategyFactory(UserRepo, VoucherRepo);
    }

    private static void CleanTestData()
    {
        string dataDir = Path.Combine(AppContext.BaseDirectory, "data");
        if (Directory.Exists(dataDir))
            foreach (var file in Directory.GetFiles(dataDir, "*.json"))
                File.Delete(file);
    }
}
