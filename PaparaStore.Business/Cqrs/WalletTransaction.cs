using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Business.Cqrs;
public record GetAllWalletTransactionsByUserIdQuery : IRequest<ApiResponse<List<WalletTransactionResponse>>> { }

public record GetWalletTransactionsByReferenceNumberQuery : IRequest<ApiResponse<List<WalletTransactionResponse>>>
{
    public long ReferenceNumber { get; set; }

    public GetWalletTransactionsByReferenceNumberQuery(long referenceNumber)
    {
        ReferenceNumber = referenceNumber;
    }
}
