using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;

namespace Studies.ApiAi
{
    class Program
    {
        private static ApiAiSDK.ApiAi _ApiAi;
        private static RequestExtras _RequestExtras;
        private static string _SessionId = "thiagoamarante";

        static void Main(string[] args)
        {
            var config = new AIConfiguration("5a0161c9367f4951a61a0ef52b49c2d6", SupportedLanguage.PortugueseBrazil);
            _ApiAi = new ApiAiSDK.ApiAi(config);
            Console.WriteLine("Digite uma mensagem...");
            WaitText();
            Console.WriteLine("Terminou");
            Console.ReadLine();
        }

        static void WaitText()
        {
            string text = Console.ReadLine();
            RequestExtras extras = new RequestExtras();
            AIRequest request = new AIRequest(text);
            request.SessionId = _SessionId;
            AIResponse response = _ApiAi.TextRequest(request);
            HandleResponse(response);
        }

        static void HandleResponse(AIResponse response)
        {
            try
            {
                if (response.IsError)                
                    throw new Exception($"Details: {response.Status.ErrorDetails}, ErrorID: {response.Status.ErrorID}, ErrorType: {response.Status.ErrorType}, Code: {response.Status.Code}");

                if (!string.IsNullOrEmpty(response.Result.Fulfillment.Speech))
                    Console.WriteLine($"bot: {response.Result.Fulfillment.Speech}");

                if(string.IsNullOrEmpty(response.Result.Action) && string.IsNullOrEmpty(response.Result.Fulfillment.Speech))
                    Console.WriteLine($"bot: estou meio confuso!");

                if (!string.IsNullOrEmpty(response.Result.Action) && string.IsNullOrEmpty(response.Result.Fulfillment.Speech))
                    HandleAction(response);


                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                Console.ResetColor();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"bot: vixeeeeeee, algo deu errado: {ex.Message}");
            }
            WaitText();
        }

        static void HandleAction(AIResponse response)
        {
            switch (response.Result.Action)
            {
                case "wisdom.person":
                    Console.WriteLine($"sei la quem é {response.Result.GetStringParameter("q")}");
                    break;
            }
        }
    }
}
