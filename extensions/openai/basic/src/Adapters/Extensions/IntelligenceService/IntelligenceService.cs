using System.Collections.Generic;
using Azure;
using Azure.AI.OpenAI;


namespace DevPrime.Extensions.IntelligenceService;
public class IntelligenceService : DevPrimeExtensions, IIntelligenceService
{
    private string Credential { get; set; }
    private string URL { get; set; }
    private string DeploymentModel { get; set; }


    public IntelligenceService(IDpExtensions dp) : base(dp)
    {
        Credential = Dp.Settings.Default("ai.credential");
        URL = Dp.Settings.Default("ai.url");
        DeploymentModel = Dp.Settings.Default("ai.deploymentmodel");
    }

    public string Conversation(string prompt)
    {


        return Dp.Pipeline(ExecuteResult: () =>
               {

                  Dp.Observability.Log("Starting OpenAI");

                   // Prepare AI Configurations
                   var chatCompletionsOptions = new ChatCompletionsOptions()
                   {
                       Messages =      {
                                       },
                       Temperature = (float)0.7,
                       MaxTokens = 800,
                       NucleusSamplingFactor = (float)0.95,
                       FrequencyPenalty = 0,
                       PresencePenalty = 0,
                   };


                   // Creating OpenAI Client
                   var openAiClient = new OpenAIClient(new System.Uri(URL), new AzureKeyCredential(Credential));

                   // Add prompt to conversation (hatRole.System / 
                   // ChatRole.User / ChatRole.Assistant)                   
                   chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.User, prompt));

                   // Prepare for interaction
                   Response<ChatCompletions> response = openAiClient.GetChatCompletions(DeploymentModel, chatCompletionsOptions);

                   //OpenAI Result
                   var airesponse = response.GetRawResponse().Content.ToString();
                   var root = System.Text.Json.JsonSerializer.Deserialize<Root>(airesponse);
                   Message message = root.choices[0].message;
                   var airesult = message.content;

                   Dp.Observability.Log("Finalizing OpenAI");
                   return airesult;
               });

    }


    public class ContentFilterResults
{
    public bool filtered { get; set; }
    public string severity { get; set; }
}

public class PromptFilterResults
{
    public int prompt_index { get; set; }
    public ContentFilterResults content_filter_results { get; set; }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
    public ContentFilterResults content_filter_results { get; set; }
}

public class Choice
{
    public int index { get; set; }
    public string finish_reason { get; set; }
    public Message message { get; set; }
    public ContentFilterResults content_filter_results { get; set; }
}

public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}

public class Root
{
    public string id { get; set; }
    public string _object { get; set; }
    public int created { get; set; }
    public string model { get; set; }
    public List<PromptFilterResults> prompt_filter_results { get; set; }
    public List<Choice> choices { get; set; }
    public Usage usage { get; set; }
}

}