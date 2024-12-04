using ApplicationB.Contracts_B.Order;
using AutoMapper;
using DTOsB.Order.PaymentDTO;
using DTOsB.Shared;
using ModelsB.Order_B;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ApplicationB.Services_B.Order
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper mapper;
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(IMapper _mapper, IPaymentRepository _paymentRepository) {
            mapper = _mapper;
            paymentRepository = _paymentRepository;
        }

        public async Task<ResultView<AddOrUpdatePaymentBDTO>> CreatePaymentAsync(AddOrUpdatePaymentBDTO paymentBDTO)
        {
            var payment = mapper.Map<PaymentB>(paymentBDTO);
            await paymentRepository.AddAsync(payment);
            return ResultView<AddOrUpdatePaymentBDTO>.Success(paymentBDTO);
        }








        //public async Task<string> CreatePayment(decimal amount, string currency)
        //{
        //    var request = new OrdersCreateRequest();
        //    request.Prefer("return=representation");
        //    request.RequestBody(BuildRequestBody(amount, currency));

        //    var response = await PayPalConfig.GetClient().Execute(request);
        //    var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

        //    // Extract the approval URL
        //    var approvalUrl = result.Links.FirstOrDefault(link => link.Rel == "approve")?.Href;
        //    return approvalUrl; // Send this URL back to the client to redirect to PayPal
        //}

        private OrderRequest BuildRequestBody(decimal amount, string currency)
        {
            return new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = currency,
                            Value = amount.ToString()
                        }
                    }
                },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = "https://yourwebsite.com/Payment/Success",  // URL after successful payment
                    CancelUrl = "https://yourwebsite.com/Payment/Cancel"   // URL if the user cancels the payment
                }
            };
        }

        public async Task CapturePayment(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());

            var response = await PayPalConfig.GetClient().Execute(request);
            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

            // Handle the result of the capture (e.g., updating order status in the database)
        }

      
    }

    public class PayPalConfig
    {
        public static PayPalEnvironment GetEnvironment()
        {
            var clientId = "ATvDFJFysYICcOZtRecpSyQbw0iwDwl6tVuTRTyYDi-aJAbFLNTIQMrY21C-xY11cB9cykkearVgb5Op";
            var secret = "EKt6Eh4B8kUQXAhq8_-fxRZJJ0leG7edvtfOJ-7fbFB3qFunAELEhG0lkV80pME_AAXAgTJjda11INpT";
            return new SandboxEnvironment(clientId, secret);
        }

        public static PayPalHttpClient GetClient()
        {
            return new PayPalHttpClient(GetEnvironment());
        }
    }
}
