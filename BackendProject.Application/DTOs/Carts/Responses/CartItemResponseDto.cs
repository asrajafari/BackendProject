namespace BackendProject.Application.DTOs.Carts.Responses;

public class CartItemResponseDto
{
    public Guid Id { get; set; }
   // public Guid ProductId { get; set; }
   public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime AddedAt { get; set; }
}