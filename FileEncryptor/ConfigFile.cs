using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileEncryptor
{
    internal class IniFileOperations
    {
        public static string codeFile = Application.StartupPath + @"\config.svv";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            string key, string def, StringBuilder retVal,
            int size, string filePath);

        public static void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, codeFile);
        }

        public static string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, codeFile);
            return temp.ToString();
        }
    }
    internal class ConfigFile
    {
        public static void CheckConfigFileExists()
        {
            string configPath = Application.StartupPath + @"\config.svv";

            if (!File.Exists(configPath))
            {
                FileStream fileThread = File.Create(configPath);
                fileThread.Close();

                IniFileOperations.WriteValue("config", "SaveSettings", "");
                IniFileOperations.WriteValue("config", "Password", "");
                IniFileOperations.WriteValue("config", "File", "");
            }
        }
    }
}
