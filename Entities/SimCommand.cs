using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SimCommand
    {
        public float Time { get; set; }
        public int Num { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public SimCommand() { }
        public SimCommand(float time, int num, float x, float y) 
        {
            Time = time;
            Num = num;
            X = x;
            Y = y;
        }
    }
}

