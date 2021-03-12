using System;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace EncryptionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!File.Exists(Assembly.GetEntryAssembly().Location + ".config"))
            {
                Console.WriteLine("Brak pliku konfiguracyjnego.");
                Console.ReadKey();
                return;
            }
            else
            {
                //Get settings from config file
                string InputFolder = ConfigurationManager.AppSettings.Get("InputFolder");
                string OutputFolder = ConfigurationManager.AppSettings.Get("OutputFolder");
                string InputWordsFile = ConfigurationManager.AppSettings.Get("WordsFile");
                string InputWordsExceptionFile = ConfigurationManager.AppSettings.Get("WordsExceptionFile");

                //Create variables
                string WordsFile = Path.Combine(InputFolder, InputWordsFile);
                string WordsExceptionFile = Path.Combine(InputFolder, InputWordsExceptionFile);
                string OutputWordsFileName = "Wynik_" + DateTime.Now.ToString("dd.MM.yyyy_HHmmss") + ".txt";
                string OutputWordsFilePath = Path.Combine(OutputFolder, OutputWordsFileName);

                if(!File.Exists(WordsFile))
                {
                    Console.WriteLine("Brak pliku ze slowami lub bledny format.");
                    Console.ReadKey();
                    return;
                }
                else if(!File.Exists(WordsExceptionFile))
                {
                    Console.WriteLine("Brak pliku z wyjatkami lub bledny format.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    LoadFiles(WordsFile, WordsExceptionFile, OutputWordsFilePath, OutputFolder);
                }
            }

            void LoadFiles(string WordsFile, string WordsExceptionFile, string OutputWordsFilePath, string OutputFolder)
            {
                try
                {
                    Regex regexDelim = new Regex(@"[:\/\s\""\,\.\-\;\:]+");
                    List<string> WordsExceptionList = new List<string>();
                    List<string> WordsList = new List<string>();


                    //Read exception words from file
                    using ( StreamReader streamReader = new StreamReader(WordsExceptionFile))
                    {
                        string WordsTemp = streamReader.ReadToEnd();
                        WordsExceptionList = regexDelim.Split(WordsTemp).ToList();
                    };
                    Console.WriteLine("Wczytano slow: " + WordsExceptionList.Count);

                    //Read exception words from file
                    using (StreamReader streamReader = new StreamReader(WordsFile))
                    {
                        string WordsTemp = streamReader.ReadToEnd();
                        WordsList = regexDelim.Split(WordsTemp).ToList();
                    };
                    Console.WriteLine("Wczytano wyjatkow: " + WordsList.Count);

                    WordsList = WordsList.Except(WordsExceptionList).ToList();
                    Console.WriteLine("\nWyrazy z wyjatkami:  " + WordsList.Count);

                    if (WordsList.Count == 0)
                    {
                        Console.WriteLine("Brak slow do zaszyfrowania.");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        EncryptClass encryptClass = new EncryptClass();
                        encryptClass.WordsEncrypt(WordsList, OutputWordsFilePath, OutputFolder);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.ReadKey();
                    return;
                }
            }
            Console.ReadKey();
        }
    }
}
