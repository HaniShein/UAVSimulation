using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SimParams
    {
        public float Dt { set; get; }
        public int N_uav { set; get; }
        public float R { set; get; }
        public float X0 { set; get; }
        public float Y0 { set; get; }
        public float Z0 { set; get; }
        public float V0 { set; get; }
        public float Az { set; get; }
        public float TimeLim { set; get; }

        public SimParams(float dt,
        int n_uav,
        float radius,
        float x,
        float y,
        float z,
        float v,
        float az,
        float timeLim) 
        {
            Dt = dt;
            N_uav = n_uav;
            R = radius;
            X0 = x;
            Y0 = y;
            Z0 = z;
            V0 = v;
            Az = az;
            TimeLim = timeLim;
        }
    }
}
