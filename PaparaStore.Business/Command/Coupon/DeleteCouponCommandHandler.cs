using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;

namespace PaparaStore.Business.Command;
public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public DeleteCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await unitOfWork.CouponRepository.GetById(request.CouponId);

        if (coupon == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "Product not found" };
        }
        await unitOfWork.CouponRepository.Delete(request.CouponId);
        await unitOfWork.Complete();
        return new ApiResponse { IsSuccess = true, Message = "Coupon deleted successfully" };
    }
}
