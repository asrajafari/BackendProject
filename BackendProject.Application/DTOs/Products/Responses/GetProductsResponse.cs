namespace BackendProject.Application.DTOs.Products.Responses;

public class GetProductsResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}