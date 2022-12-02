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
        private static void CreateFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string toEncryptFilePath = "";
            string password = "";

            Console.Write("Encrypting file");

            while (true)
            {
                Console.Write("\n\nEnter password (only 32 characters!) or use 'gen' to auto-generate: "); 
                Console.ForegroundColor = ConsoleColor.Yellow;
                password = Console.ReadLine();
                Console.ResetColor();

                if (password == "gen")
                {
                    password = GeneratePass();
                    Console.Write("Generated password (saved): ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(password);
                    Console.ResetColor();
                    break;
                }
                else if (password.Length != 32)
                {
                    Console.WriteLine("Only 32 characters!");
                }
                else
                    break;
            }

            Console.Write("\nEnter file to encrypt: ");

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

        private static void OpenFile()
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

        private static string GeneratePass()
        {
            string password = "";
            string[] arrSymbol = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", 
                                   "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", 
                                   "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Z",
                                   "b", "c", "d", "f", "g", "h", "j", "k", "m", "n", 
                                   "p", "q", "r", "s", "t", "v", "w", "x", "z", "A", 
                                   "E", "U", "Y", "a", "e", "i", "o", "u", "y", "0", };

            Random rnd = new Random();

            for (int i = 0; i < 32; i++)
            {
                password += arrSymbol[rnd.Next(0, 58)];
            }

            return password;
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
