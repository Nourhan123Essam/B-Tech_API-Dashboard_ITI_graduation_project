using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System.Threading.Tasks;
using DTOsB.Order.OrderDTO;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ModelsB.Order_B;
using System.Text.Json.Nodes;

namespace ApplicationB.Services_B.Order
{
    public class PayPalService
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PayPalService(System.Net.Http.HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var clientId = _configuration["PayPal:ClientId"];
            var secret = _configuration["PayPal:Secret"];
            var baseUrl = _configuration["PayPal:BaseUrl"];

            //var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            //var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            //var response = await _httpClient.PostAsync($"{baseUrl}/v1/oauth2/token", content);

            //if (response.IsSuccessStatusCode)
            //{
            //    var json = await response.Content.ReadAsStringAsync();
            //    var token = JsonDocument.Parse(json).RootElement.GetProperty("access_token").GetString();
            //    return token;
            //}

            //throw new Exception("Could not retrieve access token from PayPal");

            string accessToken = "";
            string url = baseUrl + "/v1/oauth2/token";
            using (var client = new System.Net.Http.HttpClient())
            {
                string credetials = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + secret));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credetials);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");
                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }
            return accessToken;
        }
        /////////////////////

        public async Task<string> CreateOrderAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.sandbox.paypal.com/v2/checkout/orders");

            // Set the authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Define the request body
            var requestBody = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
            new
            {
                amount = new
                {
                    currency_code = "USD",
                    value = "100.00"
                }
            }
        }
            };

            // Serialize the request body to JSON
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);

            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the request to PayPal
            var response = await _httpClient.SendAsync(request);

            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to create order with PayPal.");
            }

            // Parse the response content to extract the orderId
            var responseBody = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseBody);
            var orderId = document.RootElement.GetProperty("id").GetString();

            return orderId;
        }

            /////////////////////////////////
            public async Task<bool> CaptureOrderAsync(string token, string orderId)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync($"{_configuration["PayPal:BaseUrl"]}/v2/checkout/orders/{orderId}/capture", null);

                return response.IsSuccessStatusCode;
            }



}
}
