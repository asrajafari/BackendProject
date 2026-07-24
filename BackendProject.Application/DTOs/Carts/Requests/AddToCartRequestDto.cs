namespace BackendProject.Application.DTOs.Carts.Requests;

public class AddToCartRequestDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}