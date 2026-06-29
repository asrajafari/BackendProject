namespace BackendProject.Application.DTOs.Products.Responses;

public class DeleteProductResponse
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;
}