namespace BackendProject.Domain.Entities;

public class Product : BaseEntity<int>
{
    public string Name { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    private Product()
    {
    }

    public Product(string name, decimal price)
    {
        ChangeName(name);
        ChangePrice(price);
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.");

        if (name.Length > 100)
            throw new ArgumentException("Product name cannot exceed 100 characters.");

        Name = name;
        MarkAsUpdated();
    }

    public void ChangePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        Price = price;
        MarkAsUpdated();
    }
}