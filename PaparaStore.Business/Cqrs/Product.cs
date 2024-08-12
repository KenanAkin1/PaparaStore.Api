using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record CreateProductCommand(ProductRequest Request) : IRequest<ApiResponse<ProductResponse>>;
public record UpdateProductCommand(long ProductId, ProductRequest Request) : IRequest<ApiResponse>;
public record DeleteProductCommand(long ProductId) : IRequest<ApiResponse>;

public record GetAllProductQuery() : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetAllProductByCategoryQuery(string CategoryName) : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetAllProductByStatusQuery(bool IsActive) : IRequest<ApiResponse<List<ProductResponse>>>;
public record GetProductByIdQuery(long ProductId) : IRequest<ApiResponse<ProductResponse>>;
