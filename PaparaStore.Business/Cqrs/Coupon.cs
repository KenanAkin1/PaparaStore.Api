using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record CreateCouponCommand(CouponRequest Request) : IRequest<ApiResponse<CouponResponse>>;
public record UpdateCouponCommand(long CouponId, CouponRequest Request) : IRequest<ApiResponse>;
public record DeleteCouponCommand(long CouponId) : IRequest<ApiResponse>;

public record GetAllCouponQuery() : IRequest<ApiResponse<List<CouponResponse>>>;
public record GetCouponByIdQuery(long CouponId) : IRequest<ApiResponse<CouponResponse>>;
