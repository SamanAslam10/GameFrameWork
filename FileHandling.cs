using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class FileHandling
    {
        int level = 0 ;
        string name = "";
        public void Save(int level , string PlayerName) 
        {
            if (File.Exists("Progress.txt")) 
            {
                StreamWriter writer = new StreamWriter("Progress.txt",true);
                writer.WriteLine(PlayerName,level);
                writer.Close();
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
                    name = ParseData(record, 2);
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
