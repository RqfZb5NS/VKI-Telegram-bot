namespace VKI_Telegram_bot.Parsers
{
    public abstract class Parser
    {
        private string? GetHtml(string url)
        {
            try
            {
                string html = string.Empty;
                using (HttpClientHandler hdl = new HttpClientHandler { 
                    AllowAutoRedirect = false, 
                    AutomaticDecompression = System.Net.DecompressionMethods.All 
                })
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
                return html;
            }
            catch (Exception ex) 
            {
                Log.Warn($"Не получилось получить html \nURL:{url}\nEX: {ex} ", ex);
                return null;
            }
        }
        public HtmlAgilityPack.HtmlDocument? GetDoc(string url)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument doc = new();
                string html = GetHtml(url)!;
                if (string.IsNullOrEmpty(html)) return null;
                doc.LoadHtml(html);
                return doc;
            }
            catch (Exception ex)
            {
                Log.Warn($"Не получилось разобрать html \nURL:{url}\nEX: {ex} ", ex);
                return null;
            }
        }
    }
}