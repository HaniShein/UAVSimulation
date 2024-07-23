using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public static class Utilities
    {
        public static float ConvertAzimuthToAngle(double azimuthInDegrees)
        {
            //Convert the azimuth to radians (for use in trigonometric functions)
            double azimuthInRadians = azimuthInDegrees * Math.PI / 180;

            //Calculate the angle (assuming that north is in the direction of the positive y-axis)
            double angleInRadians = Math.PI / 2 - azimuthInRadians;

            //Convert the angle back to degrees
            double angleInDegrees = angleInRadians * 180 / Math.PI;

            //Adjusting the angle to the range -180 to 180 degrees (can be changed as needed)
            while (angleInDegrees > 180)
            {
                angleInDegrees -= 360;
            }
            while (angleInDegrees <= -180)
            {
                angleInDegrees += 360;
            }

            return (float)angleInDegrees;
        }

        public static float CalculateAzimuth(Vector3 point1, Vector3 point2)
        {
            //Calculate delta
            float deltaX = point2.X - point1.X;
            float deltaY = point2.Y - point1.Y;

            //Calulate  angle in radians
            float angleRadians = (float)Math.Atan2(deltaY, deltaX);

            //convert to degrees
            float angleDegrees = (float)(angleRadians * 180 / Math.PI);

            return angleDegrees;
        }

        public static double CalculateDistance(Vector3 point1, Vector3 point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
