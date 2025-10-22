namespace EventBus.Messages.Events.Basket;

public sealed class BasketCheckoutIntegrationEvent : IntegrationBaseEvent
{
    public BasketCheckoutIntegrationEvent() : base()
    { }

    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }

    //Billing Address
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;

    //Payment
    public string CardName { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public string Expiration { get; set; } = default!;
    public string Cvv { get; set; } = default!;
    public int PaymentMethod { get; set; }
}
