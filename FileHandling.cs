using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GameFrameWork
{
    internal class FileHandling
    {
        int level = 0 ;
        string name = "";
        public void Save(int level , string PlayerName) 
        {
            string path = "Progress.txt";

            if (!File.Exists(path))
                File.Create(path).Close();

            string[] records = File.ReadAllLines(path);
            foreach (string record in records)
            {
                string existingName = record.Split(',')[0];
                if (existingName == PlayerName)
                    return;
            }

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"{PlayerName},{level}");
            }
        }
        public void Load()
        {
            if (File.Exists("Progress.txt")) 
            {
                StreamReader read = new StreamReader("Progress.txt");
                string record;
                while ((record = read.ReadLine()) != null) 
                {
                    level = Convert.ToInt32(ParseData(record,1));
                    name = ParseData(record, 0);
                }
                read.Close();
            }
        }
        public string GetName() 
        {
            Load();
            return name;
        }
        public int GetLevel() 
        {
            Load();
            return level;
        }
        private string ParseData(string record , int field) 
        {
            int comma = 0;
            string item = "";
            for(int i = 0; i < record.Length ; i ++) 
            {
                if(record[i] == ',') 
                {
                    comma++;
                }
                else if(comma == field) 
                {
                    item = item + record[i];
                }
            }
            return item;
        }
    }
}
