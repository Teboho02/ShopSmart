using ShopSmart.Enums;
using ShopSmart.Services.States;

public class OrderStateTests
{
    // ─── Concrete states ─────────────────────────────────────────────────────

    [Fact]
    public void PendingState_Next_IsProcessing()
    {
        Assert.Equal(OrderStatus.Processing, new PendingState().Next);
    }

    [Fact]
    public void PendingState_CanCancel_IsTrue()
    {
        Assert.True(new PendingState().CanCancel);
    }

    [Fact]
    public void PendingState_NextLabel_IsNotNull()
    {
        Assert.NotNull(new PendingState().NextLabel);
        Assert.Contains("Processing", new PendingState().NextLabel);
    }

    [Fact]
    public void ProcessingState_Next_IsShipped()
    {
        Assert.Equal(OrderStatus.Shipped, new ProcessingState().Next);
    }

    [Fact]
    public void ShippedState_Next_IsDelivered()
    {
        Assert.Equal(OrderStatus.Delivered, new ShippedState().Next);
    }

    [Fact]
    public void DeliveredState_IsTerminal()
    {
        var state = new DeliveredState();

        Assert.Null(state.Next);
        Assert.False(state.CanCancel);
        Assert.Null(state.NextLabel);
    }

    [Fact]
    public void CancelledState_IsTerminal()
    {
        var state = new CancelledState();

        Assert.Null(state.Next);
        Assert.False(state.CanCancel);
        Assert.Null(state.NextLabel);
    }

    // ─── OrderStateFactory ───────────────────────────────────────────────────

    [Theory]
    [InlineData(OrderStatus.Pending,    typeof(PendingState))]
    [InlineData(OrderStatus.Processing, typeof(ProcessingState))]
    [InlineData(OrderStatus.Shipped,    typeof(ShippedState))]
    [InlineData(OrderStatus.Delivered,  typeof(DeliveredState))]
    [InlineData(OrderStatus.Cancelled,  typeof(CancelledState))]
    public void Factory_ReturnsCorrectStateType(OrderStatus status, Type expectedType)
    {
        var state = OrderStateFactory.For(status);

        Assert.IsType(expectedType, state);
    }

    [Theory]
    [InlineData(OrderStatus.Pending)]
    [InlineData(OrderStatus.Processing)]
    [InlineData(OrderStatus.Shipped)]
    public void NonTerminalStates_CanCancel_IsTrue(OrderStatus status)
    {
        Assert.True(OrderStateFactory.For(status).CanCancel);
    }

    [Theory]
    [InlineData(OrderStatus.Delivered)]
    [InlineData(OrderStatus.Cancelled)]
    public void TerminalStates_CanCancel_IsFalse(OrderStatus status)
    {
        Assert.False(OrderStateFactory.For(status).CanCancel);
    }

    [Theory]
    [InlineData(OrderStatus.Delivered)]
    [InlineData(OrderStatus.Cancelled)]
    public void TerminalStates_Next_IsNull(OrderStatus status)
    {
        Assert.Null(OrderStateFactory.For(status).Next);
    }

    // ─── Sequential flow ─────────────────────────────────────────────────────

    [Fact]
    public void SequentialFlow_PendingToDelivered_FollowsExpectedPath()
    {
        var sequence = new[]
        {
            OrderStatus.Pending,
            OrderStatus.Processing,
            OrderStatus.Shipped,
            OrderStatus.Delivered
        };

        for (int i = 0; i < sequence.Length - 1; i++)
        {
            var state = OrderStateFactory.For(sequence[i]);
            Assert.Equal(sequence[i + 1], state.Next);
        }
    }
}
