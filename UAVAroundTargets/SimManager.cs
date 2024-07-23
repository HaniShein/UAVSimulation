using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using System.Diagnostics;
using System.Timers;

namespace UAVAroundTargets
{
    public class SimManager
    {
        private static SimManager instance;

        private SimParams _simParams;
        private LinkedList<SimCommand> _simCommands;
        private System.Timers.Timer _cycleTimer = new System.Timers.Timer();
        private int _currentCycleNum = 0;
        private List<FlightRoute> _flightRouteList = new List<FlightRoute>();
        private DateTime _simulationStartTime;
        private DateTime _simulationEndTime;
        private bool _done = false;
        private int _eventsCount = 0;
        private string SaveAtDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Routes " + DateTime.Now.ToString("ddMMyyyy HHmmss");

        private SimManager()
        {
            //Read simulation inputs
            _simParams = IniInputReader.Read();
            _simCommands = CommandsReader.Read();

            //Create output directory
            if (!Directory.Exists(SaveAtDirectory))
            {
                Directory.CreateDirectory(SaveAtDirectory);
            }

            //Initialize the flight routes
            InitUAVs();

            //Run simulation
            StartCycles();
        }

        //Singleton
        public static SimManager GetInstance()
        {
            if (instance == null)
            {
                instance = new SimManager();
            }
            return instance;
        }

        private void InitUAVs()
        {
            List<UAV> uavList = new List<UAV>();

            foreach (SimCommand command in _simCommands)
            {
                if (!uavList.Exists(uav => uav.Id == command.Num))
                {
                    UAV uav = new UAV()
                    {
                        Id = command.Num/*uavList.Count + 1*/,
                        Radius = _simParams.R,
                        Velocity = _simParams.V0,
                        Location = new(_simParams.X0, _simParams.Y0, _simParams.Z0),
                        Angle = Utilities.ConvertAzimuthToAngle(_simParams.Az)
                    };
                    uavList.Add(uav);

                    Console.WriteLine("uav id" + uav.Id);
                    FlightRoute flightRoute = new(uav, _simParams.Dt, _simParams.R, SaveAtDirectory);
                    _flightRouteList.Add(flightRoute);
                }
            }
        }

        private void StartCycles()
        {
            _cycleTimer.Elapsed += new ElapsedEventHandler(OnCycleTimerCallBack);
            _cycleTimer.Interval = _simParams.Dt * 1000;
            _cycleTimer.Enabled = true;
        }

        private void OnCycleTimerCallBack(object? sender, ElapsedEventArgs e)
        {
            _eventsCount++;
            lock (this)
            {
                int localCycleNum = _currentCycleNum;
                _currentCycleNum++;

                try
                {
                    if (!_done && _simulationEndTime.Ticks > 0 && _simulationEndTime.Ticks < DateTime.Now.Ticks)
                    {
                        _done = true;
                        Dispose();

                        Console.WriteLine("- - - Simulation ended - - - ");
                        Console.WriteLine("Route files location: " + SaveAtDirectory);
                    }
                    else
                    {
                        if (_simCommands.Count > 0 && ((SimCommand)_simCommands.First?.Value).Time <= (DateTime.Now.Second - _simulationStartTime.Second))
                        {
                            FlightRoute? flightRoute = _flightRouteList?.Where(fr => fr.UAVId == _simCommands.First.Value.Num).FirstOrDefault();
                            flightRoute?.SetTarget(new Vector3(((SimCommand)_simCommands.First.Value).X, ((SimCommand)_simCommands.First.Value).Y, -1));
                            _simCommands.RemoveFirst();
                        }
                        _flightRouteList.ForEach(fr => fr.MoveForward(_simParams.Dt, _simParams.Dt * localCycleNum));
                    }
                }
                catch (Exception ex)
                {
                    //TODO log exception, or some other error handling
                    Console.WriteLine("Exception on SimManager: " + ex.Message);
                }
                finally
                {
                    _eventsCount--;
                }

                if (_done && _eventsCount == 0)
                {
                    _flightRouteList?.ForEach(fr => fr.End());
                }
            }
        }

        internal void Go()
        {
            _simulationStartTime = DateTime.Now;
            _simulationEndTime = DateTime.Now.AddSeconds(_simParams.TimeLim);
            _cycleTimer.Start();
        }

        private void Dispose()
        {
            _cycleTimer.Stop();
            _cycleTimer.Dispose();
        }
    }
}

