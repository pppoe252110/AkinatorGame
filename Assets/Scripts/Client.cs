using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.Events;

public class OpenRouterApiClient
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://openrouter.ai/api/v1/chat/completions";
    public UnityAction<string> onError;
    private string _model = "liquid/lfm-40b:free";
    public OpenRouterApiClient(string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(ApiUrl);

        // Set default headers
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<string> GetCompletionAsync(string message)
    {
        var requestBody = new
        {
            model = _model,
            //model = "google/gemini-flash-1.5-exp",
            //model = "liquid/lfm-40b:free",
            //model = "meta-llama/llama-3.2-3b-instruct:free",
            messages = new[]
            {
                new { role = "user", content = message }
            }
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);

        var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            onError?.Invoke(ex.Message);
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    internal void SetModel(string model)
    {
        _model = model;
    }
}

//GENERATED
public class ChatCompletionResponse
{
    public string Id { get; set; }
    public string Provider { get; set; }
    public string Model { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public List<ResponseChoice> Choices { get; set; }
    public string SystemFingerprint { get; set; }
    public ResponseUsage Usage { get; set; }

    public class ResponseChoice
    {
        public object Logprobs { get; set; } // Using object type as it can be null or an array
        public string FinishReason { get; set; }
        public int Index { get; set; }
        public ResponseMessage Message { get; set; }
        public string Refusal { get; set; }
    }

    public class ResponseMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class ResponseUsage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}
