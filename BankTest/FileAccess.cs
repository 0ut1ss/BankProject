using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Bank
{
    static class FileAccess
    {
        //List to store user transactions
        private static List<string> Buffer = new List<string>();


        //Adds transactions to list
        public static void AddToBuffer(string transaction)
        {
            Buffer.Add(transaction);
        }

        //Creates a file with the active user's transactions
        public static void WriteToFile(string user)
        {
            var time = DateTime.Today;
            var formatDateTime = string.Format("{0:dd_MM_yyyy}", time);
            var fileToCreate = $"statement_{user}_{formatDateTime}.txt";

            File.WriteAllLines(fileToCreate, Buffer);
            Console.WriteLine("Statement sent successfully");
            
        }
    }
}
