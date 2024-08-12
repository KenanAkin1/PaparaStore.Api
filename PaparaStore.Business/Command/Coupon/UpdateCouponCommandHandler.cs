using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;

namespace PaparaStore.Business.Command;
public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public UpdateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var existingCoupon = await unitOfWork.CouponRepository.GetById(request.CouponId);

        if (existingCoupon == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "Coupon not found" };
        }
        mapper.Map(request.Request, existingCoupon);
        await unitOfWork.Complete();
        return new ApiResponse { IsSuccess = true, Message = "Coupon updated successfully" };
    }
}
