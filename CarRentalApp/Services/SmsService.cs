using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using RestSharp;

public class SmsService
{
    /* private readonly string _apiKey = Uri.EscapeDataString("NDU3YTcyNGY2YzU2NmY2ODYxMzQ3MTMxNDY2YTc3NDg=");
     private readonly string _adminNumber = "918208033175";


     public async Task<string> SendSmsAsync(string userNumber, string message)
     {
         using var client = new HttpClient();
         var values = new FormUrlEncodedContent(new[]
         {
             new KeyValuePair<string, string>("apikey", _apiKey),
             new KeyValuePair<string, string>("numbers", $"{_adminNumber},{userNumber}"), 
             new KeyValuePair<string, string>("sender", "TXTLCL"), 
             new KeyValuePair<string, string>("message", message)
         });

         var response = await client.PostAsync("https://api.textlocal.in/send/?", values);
         return await response.Content.ReadAsStringAsync();
     } */

    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public SmsService(IConfiguration configuration)
    {
        _apiKey = configuration["Fast2SMS:ApiKey"];
        _httpClient = new HttpClient();
    }

    /*public async Task<string> SendSmsAsync(string number, string message)
    {
        var values = new
        {
            route = "v3",
            sender_id = "TXTIND",
            message = message,
            language = "english",
            flash = 0,
            numbers = number
        };

        var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Add("authorization", _apiKey);

        var response = await _httpClient.PostAsync("https://www.fast2sms.com/dev/bulkV2", content);
        return await response.Content.ReadAsStringAsync();
    }*/
}
