using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Schema;

namespace PaparaStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IMediator mediator;

        public WalletsController(IMediator mediator)
        {
            this.mediator = mediator;

        }

        [HttpGet("Wallet")]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse<WalletResponse>> GetById()
        {
            var operation = new GetWalletByUserIdQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("Balance")]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse> Put([FromBody] AddWalletBalanceCommand value)
        {
            var result = await mediator.Send(value);
            return result;
        }

        [HttpGet("transactions")]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse<List<WalletTransactionResponse>>> GetAllWalletTransactions()
        {
            var operation = new GetAllWalletTransactionsByUserIdQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("transactions/{referenceNumber}")]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse<List<WalletTransactionResponse>>> GetWalletTransactionsByReferenceNumber(long referenceNumber)
        {
            var operation = new GetWalletTransactionsByReferenceNumberQuery(referenceNumber);
            var result = await mediator.Send(operation);
            return result;
        }

    }
}
