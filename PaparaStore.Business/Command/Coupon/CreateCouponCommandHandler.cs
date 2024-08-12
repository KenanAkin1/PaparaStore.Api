using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Command;
public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, ApiResponse<CouponResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CreateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<CouponResponse>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<CouponRequest, Coupon>(request.Request);
        await unitOfWork.CouponRepository.Insert(mapped);
        await unitOfWork.Complete();
        var response = mapper.Map<CouponResponse>(mapped);
        return new ApiResponse<CouponResponse>(response);
    }
}
