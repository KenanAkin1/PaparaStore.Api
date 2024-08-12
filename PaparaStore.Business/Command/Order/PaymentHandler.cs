using PaparaStore.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Business.Command.Order;
public class PaymentHandler
{
    public async Task<bool> ProcessPaymentAsync(PaymentRequest paymentRequest, decimal amount)
    {
        //Real payment system comes here
        await Task.Delay(100);
        return true;
    }
}

