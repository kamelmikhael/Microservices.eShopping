namespace Shopping.Aggregator.Models;

public class BasketModel
{
    public string UserName { get; set; }

    public List<BasketItemModel> Items { get; set; } = new();

    public decimal TotalPrice { get; set; }
}

public class BasketItemModel
{
    public int Quantity { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }

    // Product Related Additional fields
    public string Catagory { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string ImageFile { get; set; }
}
