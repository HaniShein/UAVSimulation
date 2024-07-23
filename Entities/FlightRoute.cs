using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class FlightRoute
    {
        private int _id;
        private Vector3 _nextLocation;
        private Vector3 _targetLocation = Vector3.Zero;
        private StreamWriter _writer;
        private UAV _uav;
        public int UAVId
        { get { return _uav.Id; } }

        private float _dt;
        private float _radius;
        private int _cycleNum = 1;
        private IFly _fly;
        private double _distance;
        private int _distanceIncreased;

        public FlightRoute(UAV uav, float dt, float radius, string saveAtPath)
        {
            _uav = uav;
            _id = uav.Id;
            _radius = radius;
            _fly = FlyFactory.CreateFly(FlyType.Straight, uav);
            _nextLocation = uav.Location;
            
            string file = saveAtPath + String.Format("\\UAV{0}.csv", _id);

            if(File.Exists(file))
            {
                File.Delete(file);
            }
            _writer = new StreamWriter(file);
        }
        public void SetTarget(Vector3 targetLocation)
        {
            float newAzimuth = Utilities.CalculateAzimuth(_nextLocation, targetLocation);
            _uav.Angle = newAzimuth;

            _targetLocation = targetLocation;
            
            _fly = FlyFactory.CreateFly(FlyType.Straight, _uav);
        }

        public void MoveForward(float dtParam, float cycleTime) 
        {
            Console.WriteLine("MoveForward. uav: " + _uav.Id);

            //Calculate next point
            Vector3 prevLocation = _nextLocation;
            _nextLocation = _fly.CalculateNewPoint(prevLocation, _uav.Angle, _uav.Velocity, dtParam, _targetLocation);

            double newDistance = Utilities.CalculateDistance(_nextLocation, _targetLocation);
            
            //Write to file (time x y azimuth)
            _writer.WriteLine($"{cycleTime},{_nextLocation.X.ToString("F2")},{_nextLocation.Y.ToString("F2")},{_uav.Angle.ToString("F2")}");

            //Beyond circular motion, if close to the point of interest, at a distance of a radius
            if (_targetLocation != Vector3.Zero && newDistance > _distance)
            {
                _distanceIncreased++;
            }
            else
            {
                _distanceIncreased = 0;
            }
            
            if (_distanceIncreased > 2 && newDistance >= _uav.Radius)
            {
                _fly = FlyFactory.CreateFly(FlyType.Circle, _uav);
            }

            _distance = newDistance;
        }

        private bool ArrivedToTarget(Vector3 prevLocation, Vector3 currLocation)
        {
            double prevDistance = Math.Sqrt(Math.Pow((double)prevLocation.X - (double)_targetLocation.X, 2) + Math.Pow((double)prevLocation.Y - (double)_targetLocation.Y, 2));
            double currDistance = Math.Sqrt(Math.Pow((double)currLocation.X - (double)_targetLocation.X, 2) + Math.Pow((double)currLocation.Y - (double)_targetLocation.Y, 2));

            return (prevDistance - currDistance) <= 0;
        }

        public void End()
        {
            _fly = FlyFactory.CreateFly(FlyType.Straight, _uav);
            Dispose();
        }

        private void Dispose()
        {
            _writer.Close();
            _writer.Dispose();
        }
    }
}
