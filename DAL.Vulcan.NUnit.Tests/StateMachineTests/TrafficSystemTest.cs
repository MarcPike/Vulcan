using System;
using System.Diagnostics;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.StateMachine;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.StateMachineTests
{
        
    [TestFixture]
    public class TrafficSystemTest
    {

        private StateMachine trafficSystem;

        [SetUp]
        public void Initialize()
        {
            if (trafficSystem == null)
            {
                trafficSystem = new StateMachine();
                trafficSystem.AddNewMachineState("GreenLight");
                trafficSystem.AddNewMachineState("YellowLight");
                trafficSystem.AddNewMachineState("RedLight");
                trafficSystem.OnCompleted += OnCompletedEvent;
                trafficSystem.OnTransition += OnTransitionEvent;
                trafficSystem.OnFail += OnFailEvent;
                trafficSystem.OnGoto += OnGotoEvent;
                trafficSystem.Reset();
                Console.WriteLine("Traffic System Initialized");
                Console.WriteLine(ObjectDumper.Dump(trafficSystem));
            }
        }

        [Test]
        public void SimpleTest()
        {
            Console.WriteLine("Starting Traffic System");

            Initialize();
            trafficSystem.Start();
            while (trafficSystem.CurrentState != null)
            {
                var onState = trafficSystem.CurrentState;
                onState.Complete();
            }
        }

        [Test]
        public void FailureOnYellow()
        {
            Initialize();
            Console.WriteLine("Failure on YellowLight");
            Console.WriteLine(ObjectDumper.Dump(trafficSystem));

            trafficSystem.Start();
            while (trafficSystem.CurrentState != null)
            {
                var onState = trafficSystem.CurrentState;
                if (onState.Label == "YellowLight")
                    onState.Fail("Light is yellow you idiot!");
                else
                    onState.Complete();
            }

        }

        [Test]
        public void GotoYellow()
        {
            Initialize();
            Console.WriteLine("Failure on YellowLight");
            Console.WriteLine(ObjectDumper.Dump(trafficSystem));

            trafficSystem.Start();

            var yellowLight = trafficSystem.FindStateByLabel("YellowLight");

            yellowLight.Goto();

            while (trafficSystem.CurrentState != null)
            {
                var onState = trafficSystem.CurrentState;
                onState.Complete();
            }

        }



        private void OnFailEvent(MachineState state)
        {
            Console.WriteLine($"Failed on {state.Label}");
        }

        private bool OnTransitionEvent(MachineState @from, MachineState to)
        {
            Console.WriteLine($"Transition from {from.Label} to {to.Label}");
            return true;
        }

        private void OnCompletedEvent(StateMachine machine)
        {
            Console.WriteLine("We are completed!");
            Console.WriteLine(ObjectDumper.Dump(machine));
        }

        private void OnGotoEvent(MachineState state)
        {
            Console.WriteLine($"Goto called on {state.Label}");
            Console.WriteLine(ObjectDumper.Dump(trafficSystem));
        }

    }
}
