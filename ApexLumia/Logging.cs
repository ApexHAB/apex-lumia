using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.IsolatedStorage;

namespace ApexLumia
{
    class Logging
    {

        IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

        public bool log(string name, string data)
        {
            if (name.Length < 1 || data.Length < 1) { return false; }
            try
            {
                IsolatedStorageFileStream fileStream = isolatedStorage.OpenFile(name + ".log", FileMode.Append, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(data);
                    writer.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string[] getLog(string name)
        {
            if (name.Length < 1) { return null; }
            List<String> wholeLog = new List<string>();
            try
            {
                IsolatedStorageFileStream fileStream = isolatedStorage.OpenFile(name + ".log", FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    
                    while (!reader.EndOfStream)
                    {
                        wholeLog.Add(reader.ReadLine());
                    }
                    reader.Close();
                }
            }
            catch
            {
                log("warning", "'" + name + "' could not be opened or read.");
                return null;
            }
            return wholeLog.ToArray();
        }

        public bool clearLog(string name)
        {
            if (name.Length < 1) { return false; }
            try
            {
                isolatedStorage.DeleteFile(name + ".log");
            }
            catch
            {
                log("warning", "'" + name + "' could not be cleared.");
                return false;
            }

            return true;
        }

    }
}
