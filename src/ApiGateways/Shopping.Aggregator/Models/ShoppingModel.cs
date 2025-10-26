namespace Shopping.Aggregator.Models;

public class ShoppingModel
{
    public string? UserName { get; set; }

    public BasketModel? BasketWithProdects { get; set; }

    public IEnumerable<OrderModel>? Orders { get; set; }
}