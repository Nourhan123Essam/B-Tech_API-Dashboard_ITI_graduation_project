using ApplicationB.Services_B.Order;
using AutoMapper;
using DTOsB.Order.OrderDTO;
using DTOsB.Order.PaymentDTO;
using DTOsB.Order.ShippingDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

[Route("api/paypal")]
public class PayPalController : ControllerBase
{
    private readonly PayPalService _payPalService;
    private readonly IConfiguration _configuration;

    public PayPalController(PayPalService payPalService, IConfiguration configuration)
    {
        _payPalService = payPalService;
        _configuration = configuration;
    }

    //[HttpPost("create-order")]
    //public async Task<IActionResult> CreateOrder([FromBody] JsonObject data)
    //{
    //    var totalAmount = data?["amount"]?.ToString();
    //    if (totalAmount == null)
    //    {
    //        return new JsonResult(new { Id = "" });
    //    }
    //    JsonObject createOrderRequest = new JsonObject();
    //    createOrderRequest.Add("intent", "CAPTURE");

    //    JsonObject amount = new JsonObject();
    //    amount.Add("currency_code", "USD");
    //    amount.Add("value", totalAmount);

    //    JsonObject purchaseUnit = new JsonObject();
    //    purchaseUnit.Add("amount", amount);

    //    JsonArray purchases = new JsonArray();
    //    purchases.Add(purchaseUnit);

    //    createOrderRequest.Add("purchase_units", purchases);

    //    var token = await _payPalService.GetAccessTokenAsync();
    //    var baseUrl = _configuration["PayPal:BaseUrl"];
    //    string url = baseUrl + "/v2/checkout/orders";

    //    using (var client = new System.Net.Http.HttpClient())
    //    {
    //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    //        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
    //        requestMessage.Content = new StringContent(createOrderRequest
    //            .ToString(), null, "application/json");
    //        var httpResponse = await client.SendAsync(requestMessage);

    //        if (httpResponse.IsSuccessStatusCode)
    //        {
    //            var strResponse = await httpResponse.Content.ReadAsStringAsync();
    //            var jsonResponse = JsonNode.Parse(strResponse);
    //            if (jsonResponse != null)
    //            {
    //                string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
    //                return new JsonResult(new { Id = paypalOrderId });
    //            }
    //        }
    //    }
    //    return new JsonResult(new { Id = "", Url = url });

    //    var orderId = await _payPalService.CreateOrderAsync(token);
    //    return Ok(new { orderId });
    //}

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromBody] JsonObject data)
    {
        var totalAmount = data?["amount"]?.ToString();
        if (totalAmount == null)
        {
            return new JsonResult(new { Id = "" });
        }
        JsonObject createOrderRequest = new JsonObject();
        createOrderRequest.Add("intent", "CAPTURE");

        JsonObject amount = new JsonObject();
        amount.Add("currency_code", "USD");
        amount.Add("value", totalAmount);

        JsonObject purchaseUnit = new JsonObject();
        purchaseUnit.Add("amount", amount);

        JsonArray purchases = new JsonArray();
        purchases.Add(purchaseUnit);

        createOrderRequest.Add("purchase_units", purchases);

        var token = await _payPalService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Failed to retrieve PayPal access token.");
            return StatusCode(500, new { Id = "", Error = "Failed to retrieve access token" });
        }

        var baseUrl = _configuration["PayPal:BaseUrl"];
        string url = baseUrl + "/v2/checkout/orders";

        using (var client = new System.Net.Http.HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

            var httpResponse = await client.SendAsync(requestMessage);

            if (httpResponse.IsSuccessStatusCode)
            {
                var strResponse = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine("PayPal API response: " + strResponse);
                var jsonResponse = JsonNode.Parse(strResponse);
                if (jsonResponse != null)
                {
                    string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                    return new JsonResult(new { Id = paypalOrderId });
                }
            }
            else
            {
                var errorResponse = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Error from PayPal: " + errorResponse);
                return StatusCode((int)httpResponse.StatusCode, new { Id = "", Error = errorResponse });
            }
        }
        return new JsonResult(new { Id = "" });
    }


    [HttpPost("capture-order")]
    public async Task<IActionResult> CaptureOrder([FromBody] JsonObject data)
    {
        var orderId = data?["orderID"]?.ToString();
        if (orderId == null)
        {
            return new JsonResult("error: Missing order ID");
        }
        string token = await _payPalService.GetAccessTokenAsync();
        var url = $"{_configuration["PayPal:BaseUrl"]}/v2/checkout/orders/{orderId}/capture";

        using (var client = new System.Net.Http.HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent("", null, "application/json")
            };
            var httpResponse = await client.SendAsync(requestMessage);

            if (httpResponse.IsSuccessStatusCode)
            {
                var strResponse = await httpResponse.Content.ReadAsStringAsync();
                var jsonResponse = JsonNode.Parse(strResponse);
                string paypalStatus = jsonResponse?["status"]?.ToString() ?? "";

                if (paypalStatus == "COMPLETED")
                {
                    return new JsonResult("success");
                }
                return new JsonResult($"error: Unexpected PayPal status {paypalStatus}");
            }
            else
            {
                var errorResponse = await httpResponse.Content.ReadAsStringAsync();
                return new JsonResult($"error: PayPal capture failed - {errorResponse}");
            }
        }
    }


    // [HttpPost("capture-order")]
    //public async Task<IActionResult> CaptureOrder([FromBody] JsonObject data)
    //{
    //    var orderId = data?["orderID"]?.ToString();
    //    if (orderId == null)
    //    {
    //        return new JsonResult("error");
    //    }
    //    string token = await _payPalService.GetAccessTokenAsync();
    //    var baseUrl = _configuration["PayPal:BaseUrl"];

    //    var url = baseUrl + "/v2/checkout/orders/" + orderId + "/capture";
    //    var u = "https://api-m.sandbox.paypal.com/v2/checkout/orders/" + orderId + "/capture";

    //    using (var client = new System.Net.Http.HttpClient())
    //    {
    //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    //        var requestMessage = new HttpRequestMessage(HttpMethod.Post, u);
    //        requestMessage.Content = new StringContent(""
    //            , null, "application/json");
    //        var httpResponse = await client.SendAsync(requestMessage);

    //        if (httpResponse.IsSuccessStatusCode)
    //        {
    //            var strResponse = await httpResponse.Content.ReadAsStringAsync();
    //            var jsonResponse = JsonNode.Parse(strResponse);

    //            if (jsonResponse != null)
    //            {
    //                string paypalStatus = jsonResponse["status"]?.ToString() ?? "";
    //                if(paypalStatus == "COMPLETED")
    //                {
    //                    return new JsonResult("success");
    //                }
    //            }
    //        }
    //    }

    //    return new JsonResult("error");
    //}



}
