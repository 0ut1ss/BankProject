using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Bank
{
    static class FileAccess
    {
        private static List<string> Buffer = new List<string>();



        public static void AddToBuffer(string transaction)
        {
            Buffer.Add(transaction);
        }

        public static bool WriteToFile(string user)
        {
            var time = DateTime.Today;
            var formatDateTime = string.Format("{0:dd_MM_yyyy}", time);
            var fileToCreate = $"statement_{user}_{formatDateTime}.txt";

            File.WriteAllLines(fileToCreate, Buffer);
            Console.WriteLine("Statement sent successfully");
            return true;
        }
    }
}
