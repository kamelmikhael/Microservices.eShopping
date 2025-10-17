using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Extensions;

public static class CouponExtensions
{
    public static CouponModel MapToCouponModel(this Coupon coupon)
    {
        return new CouponModel
        {
            Id = coupon.Id,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount
        };
    }

    public static Coupon MapToCoupon(this CouponModel couponModel)
    {
        return new Coupon
        {
            Id = couponModel.Id,
            ProductName = couponModel.ProductName,
            Description = couponModel.Description,
            Amount = couponModel.Amount
        };
    }
}
