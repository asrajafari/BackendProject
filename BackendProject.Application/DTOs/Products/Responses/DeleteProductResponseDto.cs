namespace BackendProject.Application.DTOs.Products.Responses;

public class DeleteProductResponseDto
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;
}