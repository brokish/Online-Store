namespace OnlineStore.API.Requests;

public class CategoryCreate
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
