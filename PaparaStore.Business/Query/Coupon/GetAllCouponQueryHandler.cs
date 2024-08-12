using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;


namespace PaparaStore.Business.Query;
public class GetAllCouponQueryHandler : IRequestHandler<GetAllCouponQuery, ApiResponse<List<CouponResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetAllCouponQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<List<CouponResponse>>> Handle(GetAllCouponQuery request, CancellationToken cancellationToken)
    {
        List<Coupon> entityList = await unitOfWork.CouponRepository.GetAll();
        var mappedList = mapper.Map<List<CouponResponse>>(entityList);
        return new ApiResponse<List<CouponResponse>>(mappedList);
    }
}
