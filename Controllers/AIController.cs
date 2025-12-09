using FitnessCenter.Models;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Interfaces; // IOpenAIClient için
using OpenAI.ObjectModels; // Models.Gpt_3_5_Turbo vb. için
using OpenAI.ObjectModels.RequestModels; // ChatCompletionCreateRequest için
using System.Linq; // Listelerden .First() kullanmak için

namespace FitnessCenter.Controllers
{
    public class AIController : Controller
    {
        // Yüklü olan OpenAI 2.7.0 paketinden gelen IOpenAIClient servisini alıyoruz
        private readonly IOpenAIClient _openAIClient;

        // Constructor: Servisi Dependency Injection (DI) ile alıyoruz.
        public AIController(IOpenAIClient openAIClient)
        {
            _openAIClient = openAIClient;
        }

        public IActionResult DietAssistant()
        {
            return View(new AIDietViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDiet(AIDietViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("DietAssistant", model);
            }

            // Prompt oluşturma
            string prompt = $"Bana {model.Age} yaşında, {model.HeightCm} cm boyunda ve {model.WeightKg} kg ağırlığında, hedefi '{model.Goal}' olan bir kişi için 3 günlük detaylı bir diyet programı hazırla. Günlük kalori tahmini ve makro dağılımı (protein, yağ, karbonhidrat) belirt. Cevabını temiz, sadece diyet planını içeren, kolay okunabilir bir Markdown formatında ver.";

            try
            {
                // 🚀 GERÇEK API ÇAĞRISI (CHAT COMPLETION) 🚀
                var completionResult = await _openAIClient.ChatCompletion.CreateCompletion(
                    new ChatCompletionCreateRequest
                    {
                        // En güncel model
                        Model = Models.Gpt_3_5_Turbo,
                        Messages = new List<ChatMessage>
                        {
                            ChatMessage.FromSystem("Sen, fitness ve beslenme konusunda uzman, bilgili bir yapay zekâ asistanısın. Görevin, kullanıcının hedeflerine uygun detaylı diyet programları oluşturmaktır."),
                            ChatMessage.FromUser(prompt)
                        },
                        MaxTokens = 800,
                        Temperature = 0.5
                    });

                if (completionResult.Successful && completionResult.Choices.Any()) // Any() ile kontrol ediyoruz
                {
                    // Yanıtı al
                    model.DietPlanResult = completionResult.Choices.First().Message.Content.Trim();
                }
                else
                {
                    // API hatası yakalama
                    model.DietPlanResult = $"Üzgünüz, YZ plan oluşturamadı. API hatası: {completionResult.Error?.Message ?? "Bilinmeyen Hata"}";
                }
            }
            catch (Exception ex)
            {
                // Genel hata yakalama
                model.DietPlanResult = $"API Çağrısı sırasında beklenmedik bir hata oluştu: {ex.Message}";
            }

            return View("DietAssistant", model);
        }
    }
}