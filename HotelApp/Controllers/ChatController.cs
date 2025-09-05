using HotelApp.Hubs;
using HotelApp.Models;
using HotelApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HotelApp.Controllers
{
    public class ChatController : Controller
    {

        private readonly IConfiguration _config;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly HttpClient _httpClient;

        public ChatController(IConfiguration config, IHubContext<ChatHub> hubContext, HttpClient httpClient)
        {
            _config = config;
            _hubContext = hubContext;
            _httpClient = httpClient;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Route("/Chat")]
        public async Task<IActionResult> Chat()
        {
            return View();
        }

        [Route("/Chat")]
        [HttpPost]
        public async Task<IActionResult> Chat(string content)
        {
            const string SessionKey = "ChatHistory";
            var session = HttpContext.Session;

            await SendMessageAsync("User", content);

            var chatHistory = LoadChatHistory(session, SessionKey);
            chatHistory.Add(new() { ["role"] = "user", ["message"] = content });

            var roomInfo = await _httpClient.GetStringAsync("https://localhost:7050/LoaiPhong");
            var historyForPrompt = JsonSerializer.Serialize(chatHistory, new JsonSerializerOptions { WriteIndented = false });

            var requestPrompt = BuildInitialPrompt(content, roomInfo, historyForPrompt);
            var aiReply = await CallAIAsync(requestPrompt);

            if (string.IsNullOrEmpty(aiReply))
            {
                await SendMessageAsync("UTEHotel AI", "Xin lỗi, tôi chưa thể trả lời câu hỏi đó.");
                return View();
            }

            // Check nếu AI đề xuất URL gọi API
            if (aiReply.Contains("https://localhost:7050/List/"))
            {
                var url = ExtractUrl(aiReply);
                if (!string.IsNullOrEmpty(url))
                {
                    var filteredRoomJson = await _httpClient.GetStringAsync(url);

                    var followUpPrompt = requestPrompt + $@" Dữ liệu phòng cụ thể theo ngày check in và check out đã được lấy từ API: {filteredRoomJson}";
                    var finalReply = await CallAIAsync(followUpPrompt);
                    if (!string.IsNullOrEmpty(finalReply))
                    {
                        finalReply = finalReply.Replace("```html", "").Replace("```", "").Trim();
                        await SendMessageAsync("UTEHotel AI", finalReply);
                        chatHistory.Add(new() { ["role"] = "ai", ["message"] = finalReply });
                        session.SetString(SessionKey, JsonSerializer.Serialize(chatHistory));
                        return View();
                    }
                }
            }
            aiReply = aiReply.Replace("```html", "").Replace("```", "").Trim();
            await SendMessageAsync("UTEHotel AI", aiReply);
            chatHistory.Add(new() { ["role"] = "ai", ["message"] = aiReply });
            session.SetString(SessionKey, JsonSerializer.Serialize(chatHistory));

            return View();
        }

        private async Task SendMessageAsync(string sender, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", sender, message);
        }

        private List<Dictionary<string, string>> LoadChatHistory(ISession session, string key)
        {
            var json = session.GetString(key);
            return string.IsNullOrEmpty(json)
                ? new List<Dictionary<string, string>>()
                : JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
        }

        private string BuildInitialPrompt(string userMessage, string roomInfo, string chatHistoryJson)
        {
            return $@"
                Bạn là tư vấn viên AI của UTEHotel, có nhiệm vụ giới thiệu loại phòng phù hợp theo nhu cầu khách hàng.

                Dưới đây là danh sách các loại phòng (ở dạng JSON):
                {roomInfo}

                Dưới đây là lịch sử trò chuyện (dưới dạng JSON):
                {chatHistoryJson}

                Câu hỏi mới nhất của khách: ""{userMessage}""

                Nếu khách hàng hỏi về phòng theo ngày check in và check out, hãy **trích xuất rõ ràng** các thông tin ngày đó và yêu cầu hệ thống gọi API theo dạng sau: 
                'https://localhost:7050/List/checkIn/checkOut'.

                Sau đó, từ kết quả API, hãy tư vấn loại phòng phù hợp.

                Khi muốn khách muốn đặt phòng hãy hướng dẫn Khách: 1. Vào mục đặc phòng, 2. Nhập ngày check in/ check out, 3. Chọn phòng phù hợp và đặt phòng.

                Khi khách hàng muốn xem ảnh sản phẩm hãy trình bày ra bằng thẻ img để khách hàng thấy được.

                Lưu ý: Chỉ trả về đoạn HTML hoàn chỉnh không có mã JSON hay markdown để thôi thêm vào bên trong 1 thẻ div. Xin nhấn mạnh là không được có markdown";
        }

        private async Task<string> CallAIAsync(string prompt)
        {
            var apiKey = _config["GeminiAI:ApiKey"];
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            var requestData = new
            {
                contents = new[]
                {
            new
            {
                role = "user",
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };

            var json = JsonSerializer.Serialize(requestData);
            var contentData = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, contentData);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                return doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();
            }
            catch
            {
                return null;
            }
        }

        private string ExtractUrl(string text)
        {
            var match = Regex.Match(text, @"https:\/\/localhost:7050\/List\/[\w-]+\/[\w-]+");
            return match.Success ? match.Value : null;
        }


    }
}
