using DTOsB.Order.PaymentDTO;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.Order
{
    public interface IPaymentService
    {
        Task<ResultView<AddOrUpdatePaymentBDTO>> CreatePaymentAsync(AddOrUpdatePaymentBDTO paymentBDTO);
        //Task<ResultView<AddOrUpdatePaymentBDTO>> UpdatePaymentAsync(AddOrUpdatePaymentBDTO paymentBDTO);
        //Task<ResultView<SelectPaymentBDTO>> DeletePaymentAsync(int id);
        //Task<ResultView<SelectPaymentBDTO>> GetPaymentByIdAsync(int id);
        //Task<ResultView<IEnumerable<SelectPaymentBDTO>>> GetAllPaymentsAsync();
    }
}
