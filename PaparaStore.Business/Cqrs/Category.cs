using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record CreateCategoryCommand(CategoryRequest Request) : IRequest<ApiResponse<CategoryResponse>>;
public record UpdateCategoryCommand(long CategoryId, CategoryRequest Request) : IRequest<ApiResponse>;
public record DeleteCategoryCommand(long CategoryId) : IRequest<ApiResponse>;

public record GetAllCategoryQuery() : IRequest<ApiResponse<List<CategoryResponse>>>;
public record GetCategoryByIdQuery(long CategoryId) : IRequest<ApiResponse<CategoryResponse>>;
public record GetCategoryByCategoryNameQuery(string Name) : IRequest<ApiResponse<List<CategoryResponse>>>;
