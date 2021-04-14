using System;
using System.Collections.Generic;
using System.Linq;
using Leaf.xNet;
using System.Threading;
using System.IO;

namespace Youtube_Livestream_Viewbot
{
    class Program
    {
        static int BotsViewingStream = 0;

        static void Main(string[] args)
        {

            Console.Write("Video id: ");
            string videoid = Console.ReadLine();


            Console.Write("File with HTTP Proxies: ");
            string proxyfile = Console.ReadLine();

            if (!proxyfile.Contains(".txt"))
                proxyfile = proxyfile + ".txt";

            List<string> Proxies = File.ReadLines(proxyfile).ToList();

            foreach (string proxy in Proxies)
            {
                ProxyClient prxy = HttpProxyClient.Parse(proxy);
                new Thread(delegate ()
                {
                    BotLoop(videoid, prxy);
                }).Start();
                Thread.Sleep(30);
                
            }
            Thread.Sleep(-1);
            

            Console.ReadLine();
        }

        static void BotLoop(string videoid, ProxyClient proxy)
        {
            try
            {
                using (HttpRequest request = new HttpRequest())
                {
                    request.UseCookies = true;
                    request.Proxy = proxy; 
                    request.AddHeader("Host", "m.youtube.com");
                    request.AddHeader("Proxy-Connection", "keep-alive");
                    request.AddHeader("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 Instagram 123.1.0.26.115 (iPhone11,6; iOS 13_3; en_US; en-US; scale=3.00; 1242x2688; 190542906)");
                    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");

                    string data = request.Get("https://m.youtube.com/watch?v=" + videoid).ToString();

                    string url = data.Split(new[] { "videostatsWatchtimeUrl\":{\"baseUrl\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\"}" }, StringSplitOptions.None)[0].Replace("\\u0026", "&").Replace("%2C", ",").Replace("\\/", "/");
                    string cl = url.Split(new[] { "cl=" }, StringSplitOptions.None)[1].Split('&')[0];
                    string ei = url.Split(new[] { "ei=" }, StringSplitOptions.None)[1].Split('&')[0];
                    string of = url.Split(new[] { "of=" }, StringSplitOptions.None)[1].Split('&')[0];
                    string vm = url.Split(new[] { "vm=" }, StringSplitOptions.None)[1].Split('&')[0];




                    request.AddHeader("Host", "s.youtube.com");
                    request.AddHeader("Proxy-Connection", "keep-alive");
                    request.AddHeader("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 Instagram 123.1.0.26.115 (iPhone11,6; iOS 13_3; en_US; en-US; scale=3.00; 1242x2688; 190542906)");
                    request.AddHeader("Accept", "image/png,image/svg+xml,image/*;q=0.8,video/*;q=0.8,*/*;q=0.5");
                    request.AddHeader("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                    request.AddHeader("Referer", "https://m.youtube.com/watch?v=" + videoid);
                    Console.WriteLine($"Botting with {proxy.Host}");
                    BotsViewingStream++;
                    Console.Title = $"Youtube ViewBot by Sheepy | Current bots {BotsViewingStream}";
                    int re_tries = 0;
                    while (re_tries <= 30)
                    {
                        try
                        {
                            while (true)
                            {
                                request.Get("https://s.youtube.com/api/stats/watchtime?ns=yt&el=detailpage&cpn=isWmmj2C9Y2vULKF&docid=" + videoid + "&ver=2&cmt=7334&ei=" + ei + "&fmt=133&fs=0&rt=1003&of=" + of + "&euri&lact=4418&live=dvr&cl=" + cl + "&state=playing&vm=" + vm + "&volume=100&c=MWEB&cver=2.20200319.09.00&cplayer=UNIPLAYER&cbrand=apple&cbr=Safari%20Mobile&cbrver=12.1.15E148&cmodel=iphone&cos=iPhone&cosver=12_2&cplatform=MOBILE&delay=5&hl=ru&cr=GB&rtn=1303&afmt=140&lio=1556394045.182&idpj=&ldpj=&rti=1003&muted=0&st=7334&et=7634").ToString();
                                re_tries = 0;
                                Thread.Sleep(30000);
                            }
                        }
                        catch { re_tries++; Thread.Sleep(5000); }
                    }
                    Console.WriteLine($"Leaving with {proxy.Host}");
                    BotsViewingStream--; Console.Title = $"Youtube ViewBot | Current bots {BotsViewingStream}";
                    return;

                }
            }
            catch {Console.Title = $"Youtube ViewBot | Current bots {BotsViewingStream}"; }
            
        }
    }
}
