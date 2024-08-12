using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PaparaStore.Business.Query
{
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, ApiResponse<CartResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;


        public GetCartByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<CartResponse>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = long.Parse(userIdClaim.Value);
            var cart = await _unitOfWork.CartRepository.FirstOrDefaultAsync(
                c => c.UserId == userId, "CartProducts.Product");

            if (cart == null || !cart.CartProducts.Any())
            {
                return new ApiResponse<CartResponse>("Cart is empty or not found");
            }

            // Map the cart to the response model
            var cartResponse = new CartResponse
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
                ProductAmount = cart.ProductAmount,
                CartProducts = cart.CartProducts.Select(cp => new CartProductResponse
                {
                    Id = cp.Id,
                    ProductId = cp.ProductId,
                    Quantity = cp.Quantity,
                    RewardAmount = cp.RewardAmount,
                    ProductName = cp.Product.Name,
                    ProductPrice = cp.Product.Price,
                    ProductImageUrl = cp.Product.ImageUrl
                }).ToList()
            };

            return new ApiResponse<CartResponse>(cartResponse);
        }
    }
}
