using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    internal class CircleFly : IFly
    {
        private float _minAngle;

        public CircleFly(float minAngle)
        {
            _minAngle = minAngle;
        }
        
        public Vector3 CalculateNewPoint(Vector3 currentPosition, float angleDelta, float v, float t, Vector3 midPoint = new Vector3())
        {
            //Calculate radius
            double radius = CalculateRadius(currentPosition.X, currentPosition.Y, midPoint.X, midPoint.Y);

            //Calculate starting angle
            double theta0 = Math.Atan2(midPoint.Y - currentPosition.Y, midPoint.X - currentPosition.X);

            //Calculate angle of point at time t
            double theta = theta0 + v * t;

            //Calculate x and y coordinates
            double x = radius * Math.Cos(theta) + midPoint.X;
            double y = radius * Math.Sin(theta) + midPoint.Y;

            return new Vector3((float)x, (float)y, currentPosition.Z);
        }

        public static double CalculateRadius(double startX, double startY, double midX, double midY)
        {
            //Calculate distance between starting point and center point (radius)
            double distance = Math.Sqrt(Math.Pow(midX - startX, 2) + Math.Pow(midY - startY, 2));
            return distance;
        }
    }
}
