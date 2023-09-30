﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Management;

namespace Luci4.PCGrabber
{
    class Grabbing
    {

        private static string GetBitVersion() // Получение битности
        {
            try
            {
                if (Registry.LocalMachine.OpenSubKey(@"HARDWARE\Description\System\CentralProcessor\0")
                    .GetValue("Identifier")
                    .ToString()
                    .Contains("x86"))
                    return "(32 Bit)";
                else
                    return "(64 Bit)";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return "(Unknown)";
        }


        public static string GetSystemVersion() // Получение версии виндовс
        {
            return GetWindowsVersionName() + " " + GetBitVersion();
        }


        public static string ScreenMetrics() // Получение разрешение экрана
        {
            Rectangle bounds = System.Windows.Forms.Screen.GetBounds(Point.Empty);
            int width = bounds.Width;
            int height = bounds.Height;
            return width + "x" + height;
        }

        public static string GetTime()
        {
            return DateTime.Now.ToString();
        }

        public static string GetCPUName() // Получение имени процессора
        {
            try
            {
                string CPU = string.Empty;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    CPU = mObject["Name"].ToString();
                }
                return CPU;
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "СистемИнфа");
                return "Error";
            }
        }


        public static string GetHWID() // Получаем Processor Id
        {
            string sProcessorID = string.Empty;
            ManagementObjectSearcher oManagementObjectSearcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
            ManagementObjectCollection oCollection = oManagementObjectSearcher.Get();
            foreach (ManagementObject oManagementObject in oCollection)
            {
                sProcessorID = (string)oManagementObject["ProcessorId"];
            }
            return (sProcessorID);
        }


        public static string GetWindowsVersionName()// Версия виндовс
        {
            string sData = "Unknown System";
            try
            {
                using (ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(@"root\CIMV2", " SELECT * FROM win32_operatingsystem"))
                {
                    foreach (ManagementObject tObj in mSearcher.Get())
                        sData = Convert.ToString(tObj["Name"]);
                    sData = sData.Split(new char[] { '|' })[0];
                    int iLen = sData.Split(new char[] { ' ' })[0].Length;
                    sData = sData.Substring(iLen).TrimStart().TrimEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return sData;
        }

    }
}
