using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EncryptionApp
{
    public class EncryptClass
    {
        public void WordsEncrypt(List<string>WordsList, string OutputWordsFilePath, string OutputFolder)
        {
            try
            {
                List<string> EncryptedWords = new List<string>();

                for (int i = 0; i < WordsList.Count; i++)
                {
                    char[] array = WordsList[i].ToArray();
                    for (int ii = 0; ii < array.Length; ii++)
                    {
                        if (array[ii] != 'z')
                        {
                            array[ii] = (char)(array[ii] + 1);
                        }
                    }
                    string OutputWord = new string(array);
                    EncryptedWords.Add(OutputWord);
                }

                Console.WriteLine("Przekonwertowano slow: " + EncryptedWords.Count);
                SaveOutputFile(OutputWordsFilePath, EncryptedWords, OutputFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
                return;
            }
        }


        private void SaveOutputFile(string OutputWordsFilePath, List<string> EncryptedWords, string OutputFolder)
        {
            try
            {
                Directory.CreateDirectory(OutputFolder);

                using (StreamWriter streamWriter = new StreamWriter(OutputWordsFilePath))
                {
                    foreach (string word in EncryptedWords)
                        streamWriter.WriteLine(word);
                };
                Console.WriteLine("\nSukces! Plik zapisano w: " + OutputWordsFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
                return;
            }
        }
    }
}
