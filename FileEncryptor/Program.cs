using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileEncryptor
{
    internal class MainClass
    {
        static void CreateFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string toEncryptFilePath = "";
            string password = "";

            Console.Write("Encrypting file");

            while (true)
            {
                Console.Write("\n\nEnter password (only 32 characters!): "); password = Console.ReadLine();

                if (password.Length != 32)
                {
                    Console.WriteLine("Only 32 characters!");
                }
                else
                    break;
            }

            Console.Write("Enter file to encrypt: ");

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                toEncryptFilePath = openFileDialog.FileName;
                Console.Write(toEncryptFilePath + "\n");
            }

            IniFileOperations.WriteValue("config", "SaveSettings", "saving");
            IniFileOperations.WriteValue("config", "Password", password);
            IniFileOperations.WriteValue("config", "File", toEncryptFilePath);

            byte[] passwordBytes = Encryptor.ConvertToByteArray(password);
            Encryptor.EncryptFile(toEncryptFilePath, passwordBytes);

            Console.WriteLine($"File successfully encrypted, press any key to close program...");
            Console.ReadKey();
        }

        static void OpenFile()
        {
            string password = IniFileOperations.ReadValue("config", "password");
            string toEncryptFilePath = IniFileOperations.ReadValue("config", "File");
            byte[] passwordBytes = Encryptor.ConvertToByteArray(password);

            Console.WriteLine($"Saved Data\n\nPassword: {password}\nFile: {toEncryptFilePath}\n\nPress any key to continue...");

            Console.ReadKey();

            Encryptor.DecryptFile(toEncryptFilePath, passwordBytes);

            Console.WriteLine($"\nFile decrypted, open...");

            Process.Start(toEncryptFilePath);

            Console.WriteLine("\nPress any key to encrypt file and close program...");
            Console.ReadKey();

            Encryptor.EncryptFile(toEncryptFilePath, passwordBytes);
        }

        [STAThread]
        static void Main(string[] args)
        {
            ConfigFile.CheckConfigFileExists();

            if (IniFileOperations.ReadValue("config", "SaveSettings") == "saving")
            {
                OpenFile();
            }
            else
            {
                CreateFile();
            }
        }
    }
}
