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
        private string path = "S:\\CS\\semester2\\OOP\\Lab\\code\\PlantsVsZombies\\Progress.txt";

        public int GetLevel(string playerName)
        {
            Dictionary<string, int> records = LoadAllRecords();
            if (records.ContainsKey(playerName))
                return records[playerName];
            return 0;
        }

       
        public void Save(int level, string playerName)
        {
            Dictionary<string, int> records = LoadAllRecords();

            if (records.ContainsKey(playerName))
            {
              
                if (level > records[playerName])
                    records[playerName] = level;
            }
            else
            {
                records[playerName] = level; 
            }

            WriteAllRecords(records);
        }

        private Dictionary<string, int> LoadAllRecords()
        {
            Dictionary<string, int> records = new Dictionary<string, int>();
            if (!File.Exists(path))
                File.Create(path).Close();

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(',');
                if (parts.Length != 2) continue;

                string name = parts[0];
                if (!int.TryParse(parts[1], out int level)) continue;

                if (!records.ContainsKey(name))
                    records.Add(name, level);
            }
            return records;
        }

        private void WriteAllRecords(Dictionary<string, int> records)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (var kvp in records)
                {
                    writer.WriteLine($"{kvp.Key},{kvp.Value}");
                }
            }
        }
    }
}