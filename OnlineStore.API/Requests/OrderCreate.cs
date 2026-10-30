namespace OnlineStore.API.Requests;

public class OrderCreate
{
    public int CustomerId { get; set; }
    public List<OrderItemCreateDto> Items { get; set; } = new();
}

public class OrderItemCreateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
