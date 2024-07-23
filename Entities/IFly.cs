using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Entities
{
    public interface IFly
    {
        public Vector3 CalculateNewPoint(Vector3 currentPosition, float angle, float v, float t, Vector3 midPoint);
    }
}
