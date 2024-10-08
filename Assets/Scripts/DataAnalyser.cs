using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using TMPro;
using System;

public class DataAnalyser : MonoBehaviour
{
    public string OPENROUTER_API_KEY = "";

    [SerializeField] private UILogic _uiLogic;

    private OpenRouterApiClient client;
    private void OnEnable()
    {
        client = new OpenRouterApiClient(OPENROUTER_API_KEY);
        client.onError += OnError;
    }

    private void OnDisable()
    {
        client.onError -= OnError;
    }

    public void OnError(string error)
    {
        _uiLogic.Error(error);
    }

    public async void SendRequest(string request)
    {
        try
        {
            StringBuilder builder = new StringBuilder();
            foreach (var character in Database.Instance.characters)
            {
                builder.Append(character.id);
                builder.Append(' ');
                builder.Append(character.characterName);
                builder.Append(' ');
                builder.Append(character.characterAttributes);
                builder.Append('\n');
            }
            string result = await client.GetCompletionAsync("Give me character id that best fits the given parameters, if there is no matching characters return -1. NO WORDS, ONLY DIGITS. Characters: \n" + builder.ToString() + "\n Parameters: " + request);
            var r = JsonConvert.DeserializeObject<ChatCompletionResponse>(result.Trim());
            _uiLogic.Answer(r.Choices[0].Message.Content);
        }
        catch (Exception e) 
        {
            _uiLogic.Error(e.Message);
        }
    }

    internal void SetModel(string model)
    {
        client.SetModel(model);
    }
}
