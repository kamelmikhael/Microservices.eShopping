using Discount.Grpc.Extensions;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService(
    ILogger<DiscountService> logger,
    IDiscountRepository repository) : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly ILogger<DiscountService> _logger = logger;
    private readonly IDiscountRepository _repository = repository;

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, 
        ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName);

        _logger.LogInformation("Discount retrieved for ProductName: {ProductName}, Amount: {Amount}",
            coupon.ProductName, coupon.Amount);

        return coupon.MapToCouponModel();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, 
        ServerCallContext context)
    {
        var coupon = request.Coupon.MapToCoupon();

        await _repository.CreateDiscount(coupon);

        _logger.LogInformation("Discount created for ProductName: {ProductName}, Amount: {Amount}",
            coupon.ProductName, coupon.Amount);

        return request.Coupon;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, 
        ServerCallContext context)
    {
        var coupon = request.Coupon.MapToCoupon();

        await _repository.UpdateDiscount(coupon);

        _logger.LogInformation("Discount updated for ProductName: {ProductName}, Amount: {Amount}",
            coupon.ProductName, coupon.Amount);

        return request.Coupon;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, 
        ServerCallContext context)
    {
        var deleted = await _repository.DeleteDiscount(request.ProductName);

        _logger.LogInformation("Discount deleted for ProductName: {ProductName}, Deleted: {Deleted}",
            request.ProductName, deleted);

        return new DeleteDiscountResponse
        {
            Success = deleted
        };
    }
}
