using System;
using System.Collections.Generic;
using System.Net;
using Azure;
using Azure.AI.OpenAI;
using ServiceStack;

namespace DevPrime.Extensions.IntelligenceService
{
    // Main class implementing the IIntelligenceService interface
    public class IntelligenceService : DevPrimeExtensions, IIntelligenceService
    {
        // Private properties to store credentials and intelligence service configurations
        private string Credential { get; set; }
        private string Url { get; set; }
        private string DeploymentModel { get; set; }
        private string Platform { get; set; }


        // Constructor initializing properties based on DpExtensions settings
        public IntelligenceService(IDpExtensions dp) : base(dp)
        {
            Credential = Dp.Settings.Default("ai.credential");
            Url = Dp.Settings.Default("ai.url");
            DeploymentModel = Dp.Settings.Default("ai.deploymentmodel");
            Platform = ValidatePlatformSetting(Dp.Settings.Default("ai.platform") ?? "azure");
        }

        // Method to start a conversation with the AI service
        public string Conversation(string prompt)
        {
            // Using DpExtensions pipeline to execute conversation logic
            return Dp.Pipeline(ExecuteResult: () =>
            {
                // Log to indicate the start of interaction with OpenAI service
                Dp.Observability.Log("Starting OpenAI");

                // Settings for interacting with OpenAI service
                var chatCompletionsOptions = new ChatCompletionsOptions()
                {

                    Temperature = (float)0.7,
                    MaxTokens = 800,
                    NucleusSamplingFactor = (float)0.95,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0,
                    DeploymentName = DeploymentModel
                };

                // Creating OpenAI client with provided credentials
                //var openAiClient = new OpenAIClient(new System.Uri(URL), new AzureKeyCredential(Credential));

                var openAiClient = Platform == "azure" ?
                new OpenAIClient(new System.Uri(Url), new AzureKeyCredential(Credential)) :
                new OpenAIClient(Credential);


                Dp.Observability.Log($"Starting OpenAI using platform {Platform}");

                // Adding user's message to the conversation
                // ChatRequestSystemMessage / ChatRequestUserMessage / ChatRequestAssistantMessage  
                chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(prompt));

                // Preparation for interaction with OpenAI service
                Response<ChatCompletions> response = openAiClient.GetChatCompletions(chatCompletionsOptions);

                // Processing OpenAI response
                var airesponse = response.GetRawResponse().Content.ToString();
                var root = System.Text.Json.JsonSerializer.Deserialize<Root>(airesponse);
                Message message = root.choices[0].message;
                var airesult = message.content;

                // Log to indicate the end of interaction with OpenAI service
                Dp.Observability.Log("Finalizing OpenAI");

                // Returning OpenAI response
                return airesult;
            });
        }

        // Definition of inner classes to structure OpenAI response
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

        private string ValidatePlatformSetting(string setting)
        {
            Dp.Observability.Log($"[Extensions]ValidatePlatformSetting:{setting}");
            var platform = setting.ToLower();
            if (platform != "azure" && platform != "openai")
            {
                Dp.Observability.Log($"[Extensions]ERROR: Platform must be either 'azure' or 'openai'.");
                throw new System.Exception("Platform must be either 'azure' or 'openai'.");
            }
            return platform;
        }
    }
}
