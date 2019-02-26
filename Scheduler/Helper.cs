using System;
using System.IO;

namespace Scheduler
{
    public static class Helper
    {
        public static void WriteToFile(string text)
        {
            string path = "D:\\ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
    }
}
