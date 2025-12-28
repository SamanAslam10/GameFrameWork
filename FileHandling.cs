using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameWork
{
    internal class FileHandling
    {
        public static void Save(int level) 
        {
            File.WriteAllText("Progress.txt", level.ToString());
        }
        public static int Load()
        {
            if (!File.Exists("Progress.txt"))
            {
                return 1;
            }
            else 
            {
                return int.Parse(File.ReadAllText("Progress.txt"));
            }
        }
    }
}
