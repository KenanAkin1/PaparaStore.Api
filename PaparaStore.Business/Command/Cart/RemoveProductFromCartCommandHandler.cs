using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using System.Security.Claims;

namespace PaparaStore.Business.Command;

public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public RemoveProductFromCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse> Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var user = await _unitOfWork.UserRepository.GetById(userId, "Cart.CartProducts.Product");

        if (user == null)
        {
            return new ApiResponse("User not found");
        }

        var cart = user.Cart;

        if (cart == null || !cart.CartProducts.Any())
        {
            return new ApiResponse("Cart is empty");
        }

        var cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == request.ProductId);
        if (cartProduct == null)
        {
            return new ApiResponse("Product not found in cart");
        }

        cart.CartProducts.Remove(cartProduct);

        // Recalculate TotalPrice and TotalRewardPoints for the entire cart
        cart.TotalPrice = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price);
        cart.ProductAmount = cart.CartProducts.Sum(cp => cp.Quantity);
        cart.TotalRewardPoints = cart.CartProducts.Sum(cp => cp.RewardAmount);


       
        _unitOfWork.CartRepository.Update(cart);
       

        await _unitOfWork.Complete();

        return new ApiResponse("Product removed from cart successfully");
    }
}
