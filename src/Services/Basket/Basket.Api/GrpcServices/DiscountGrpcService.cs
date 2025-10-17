using Discount.Grpc.Protos;

namespace Basket.Api.GrpcServices;

public class DiscountGrpcService(
    DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient = discountProtoServiceClient;

    public async Task<CouponModel> GetDiscount(string productName)
    {
        return await _discountProtoServiceClient.GetDiscountAsync(
            new GetDiscountRequest { ProductName = productName });
    }
}
