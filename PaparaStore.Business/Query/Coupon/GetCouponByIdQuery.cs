using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Query;
public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, ApiResponse<CouponResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCouponByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<CouponResponse>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.CouponRepository.GetById(request.CouponId);
        var mapped = mapper.Map<CouponResponse>(entity);
        return new ApiResponse<CouponResponse>(mapped);
    }
}