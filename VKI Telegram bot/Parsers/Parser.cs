using VKI_Telegram_bot.DB;

namespace VKI_Telegram_bot.Parsers
{
    public class Parser : Table
    {
        public string html = string.Empty;
        public HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        public Parser(string url)
        {
            GetDoc(url);
        }
        private void GetHtml(string url)
        {
            try
            {
                using (HttpClientHandler hdl = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.All })
                {
                    using (HttpClient client = new HttpClient(hdl))
                    {
                        using (HttpResponseMessage resp = client.GetAsync(url).Result)
                        {
                            if (resp.IsSuccessStatusCode)
                            {
                                html = resp.Content.ReadAsStringAsync().Result;
                                if (string.IsNullOrEmpty(html))
                                {
                                    Console.WriteLine($"{url}:\nHtml is Null or Empty");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }
        }
        public HtmlAgilityPack.HtmlDocument GetDoc(string url)
        {
            GetHtml(url);
            doc.LoadHtml(html);
            return doc;
        }
    }
}
