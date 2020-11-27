using hu.czompisoftware.libraries.general;
#if NETCOREAPP3_1
//using hu.czompisoftware.notifications;
#endif
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hu.czompisoftware.libraries.networkproxy
{
    public class ProxyHandler
    {
        /**
         * <summary>Return true if proxy detected.</summary>
         */
        public static Boolean DetectProxy()
        {
            try
            {
                var wc = new WebClient();
                var teszt = wc.DownloadString("https://google.com");
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    switch (((HttpWebResponse)ex.Response).StatusCode)
                    {
                        case HttpStatusCode.UseProxy:
                        case HttpStatusCode.ProxyAuthenticationRequired:
                            return true;
                        default:
                            break;
                    }
                }
            }
            Logger.Info("No proxy detected. Continuing without proxy set.");
            return false;
        }

        /*internal static void ProxySetup(ref WebClient wc)
        {
            if (DetectProxy())
            {
#if NETCOREAPP3_1
                wc.Proxy = Notifications.ProxyNotification("", "");
                Logger.Info("Proxy settings presented. Continuing with those set.");
#else
                    if (File.Exists(Globals.ProxyFile))
                    {
                        var ps = JsonConvert.DeserializeObject<ProxyConfig>(File.ReadAllText(Globals.ProxyFile));
                        var proxy = new WebProxy(ps.Address, ps.Port);
                        proxy.Credentials = new NetworkCredential(ps.Username, ps.Password);
                        proxy.UseDefaultCredentials = false;
                        proxy.BypassProxyOnLocal = ps.BypassProxyOnLocal;
                        wc.Proxy = proxy;
                    }
                    Logger.Error("Proxy detectet, but you're using an unsupported platform! The program might be crash after this message", NotifyUser: true, Debug: true);
#endif
            }
        }*/
    }
}