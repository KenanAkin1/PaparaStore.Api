using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;

namespace PaparaStore.Business.Command;
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IDistributedCache distributedCache;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.distributedCache = distributedCache;
    }

    public async Task<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetById(request.ProductId);

        if (product == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "Product not found" };
        }
        await unitOfWork.ProductRepository.Delete(request.ProductId);
        await unitOfWork.Complete();
        await distributedCache.RemoveAsync("productList");
        return new ApiResponse { IsSuccess = true, Message = "Product deleted successfully" }; 
    }
}