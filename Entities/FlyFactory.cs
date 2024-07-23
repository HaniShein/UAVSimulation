using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum FlyType
    {
        Straight,
        Circle
    }
    public static class FlyFactory
    {
        public static IFly CreateFly(FlyType type, UAV uav)
        {
            switch (type)
            {
                case FlyType.Straight:
                    return new StraightFly();
                case FlyType.Circle:
                    return new CircleFly(uav.Radius);
                default:
                    throw new ArgumentException("Argument is invalid");
            }
        }
    }
}
