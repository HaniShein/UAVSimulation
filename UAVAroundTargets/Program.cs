// See https://aka.ms/new-console-template for more information
using UAVAroundTargets;

Console.WriteLine("- - - UAV circle interested points simulator started - - -");

SimManager simManager = SimManager.GetInstance();
simManager.Go();

Console.ReadLine();
