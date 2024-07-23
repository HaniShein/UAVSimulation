using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    internal class StraightFly : IFly
    {
        public Vector3 CalculateNewPoint(Vector3 currentPosition, float angle, float v, float t, Vector3 midPoint)
        {
            //Convert angle to radians
            double thetaRad = angle * Math.PI / 180;

            //Calculate velocity components
            double vx = v * Math.Cos(thetaRad);
            double vy = v * Math.Sin(thetaRad);

            //Calculate new position
            double x1 = currentPosition.X + vx * t;
            double y1 = currentPosition.Y + vy * t;

            return new Vector3((float)x1, (float)y1, currentPosition.Z);
        }

        private double ConvertAngleToAzimuth(double angleInDegrees)
        {
            //Converting the angle to radians (for use in trigonometric functions)
            double angleInRadians = angleInDegrees * Math.PI / 180;

            //Calculation of the azimuth (assuming that north is in the direction of the positive y-axis)
            double azimuthInRadians = Math.PI / 2 - angleInRadians;

            //Convert the azimuth back to degrees
            double azimuthInDegrees = azimuthInRadians * 180 / Math.PI;

            //Adjusting the azimuth to the range 0-360 degrees
            while (azimuthInDegrees < 0)
            {
                azimuthInDegrees += 360;
            }

            return azimuthInDegrees;
        }
    }
}
