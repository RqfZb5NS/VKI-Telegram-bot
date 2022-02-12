using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.Parsers
{
    public class GetHtml
    {
        public string html = string.Empty;
        public GetHtml (string url)
        {
            try
            {
                using (HttpClientHandler hdl = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.All})
                {
                    using (var client = new HttpClient(hdl))
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
            catch (Exception ex) { Console.WriteLine(ex.Message); return;}
        }
    }
}
