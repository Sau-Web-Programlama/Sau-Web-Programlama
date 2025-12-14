<<<<<<< Updated upstream
﻿using FitnessCenter.Models;
=======
﻿using SporSalonu2.Models;
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace FitnessCenter.Controllers
{
    public class AIController : Controller
    {
        private readonly OpenAIService _openAI;

        public AIController(IConfiguration config)
        {
            _openAI = new OpenAIService(new OpenAiOptions
            {
                ApiKey = config["OpenAI:ApiKey"]
            });
        }


        public IActionResult DietAssistant()
        {
            return View(new AIDietViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDiet(AIDietViewModel model)
        {
            if (!ModelState.IsValid)
                return View("DietAssistant", model);

            // 🚀 Hızlı + ucuz + tablo formatında çalışan prompt
            string prompt =
            $"{model.Age} yaşında, {model.HeightCm} cm boyunda ve {model.WeightKg} kg ağırlığında, " +
            $"hedefi '{model.Goal}' olan kişi için 5 günlük diyet programı hazırla. " +
            $"Her gün 3 öğün olacak: Kahvaltı, Öğle, Akşam. " +
            $"Tüm çıktıyı HTML tablo formatında üret. " +
            $"Her öğün şu kolonlarla olacak: Öğün, İçerik, Kalori. " +
            $"Her günün sonunda ayrıca günlük toplam makroları  ver. " +
            $"Kesinlikle açıklama yazma—sadece HTML tablolar üret.";


            var request = new ChatCompletionCreateRequest
            {
                Model = "gpt-4o-mini",  // ⚡ En hızlı + en ucuz model
                Messages = new List<ChatMessage>
                {
                    new ChatMessage("system", "Sen hızlı ve düzenli yanıt üreten uzman bir diyetisyensin."),
                    new ChatMessage("user", prompt)
                },
                Temperature = 0.1f,
                MaxTokens = 2000
            };

            var response = await _openAI.ChatCompletion.CreateCompletion(request);

            if (response.Successful)
            {
                model.DietPlanResult = response.Choices[0].Message.Content.Trim();
            }
            else
            {
                model.DietPlanResult = "API Hatası: " + response.Error?.Message;
            }

            return View("DietAssistant", model);
        }
    }
}
