using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Orders;

namespace Ordering.Application.Orders;

public class GetOrderList
{
    public sealed record Response
    {
        public int Id { get; set; }
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

    public sealed record Query(string UserName) : IRequest<IEnumerable<Response>>;

    internal sealed class Handler(
        IReadRepository<Order> orderRepository) 
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly IReadRepository<Order> _orderRepository = orderRepository;

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAsync(o => o.UserName == request.UserName);

            return orders
                .Select(order => MapToResponse(order));
        }

        private static Response MapToResponse(Order order)
        {
            return new Response
            {
                Id = order.Id,
                UserName = order.UserName,
                TotalPrice = order.TotalPrice,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                Address = order.Address,
                Country = order.Country,
                State = order.State,
                ZipCode = order.ZipCode,
                CardName = order.CardName,
                CardNumber = order.CardNumber,
                Expiration = order.Expiration,
                Cvv = order.Cvv,
                PaymentMethod = order.PaymentMethod,
            };
        }
    }
}
