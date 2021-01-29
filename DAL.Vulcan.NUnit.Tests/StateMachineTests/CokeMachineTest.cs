using System;
using System.Diagnostics;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.StateMachine;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.StateMachineTests
{
    [TestFixture]
    public class CokeMachineTest
    {
        public const decimal CokeCosts = (decimal) 0.75;
        public decimal TotalDeposit = (decimal) 0;
        public decimal MyPocketChange = (decimal) 0.75;

        private readonly StateMachine _cokeMachine = new StateMachine();
        private MachineState _deposit;
        private MachineState _makeChange;
        private MachineState _getCoke;
        private MachineState _drink;

        [SetUp]
        public void Initialize()
        {

            _deposit = _cokeMachine.AddNewMachineState("Deposit");
            _deposit.OnCheckIfCanComplete += IsEnoughDeposited;
            _deposit.OnWorkDone += DepositWasMade;

            _makeChange = _cokeMachine.AddNewMachineState("GiveChange");
            _makeChange.OnCompleteAfterThis += GiveChange;

            _getCoke = _cokeMachine.AddNewMachineState("GetCoke");
            _getCoke.OnCompleteAfterThis += GotMyCoke;

            _drink = _cokeMachine.AddNewMachineState("Drink");
            _drink.OnCompleteAfterThis += DrinkingMyCoke;

        }

        [Test]
        public void TestWithExactChange()
        {
            _cokeMachine.Start();
            while (_deposit.Action != StateAction.Complete)
            {
                TotalDeposit += (decimal)0.50;
                _deposit.OnWorkDone("$0.50 was deposited.");

                _deposit.Complete();
            }
            _makeChange.Complete();
            _getCoke.Complete();
            _drink.Complete();
            _cokeMachine.SaveToDatabase();
            Console.WriteLine(ObjectDumper.Dump(_cokeMachine.ActivityLog));

        }

        private void DepositWasMade(string workmessage)
        {
            Console.WriteLine(workmessage);
        }

        private void DrinkingMyCoke(MachineState state)
        {
            Console.WriteLine("Drinking my Coke. Yummy!");
        }

        private void GotMyCoke(MachineState state)
        {
            Console.WriteLine("I got my coke!");
        }

        private void GiveChange(MachineState state)
        {
            var requiredChange = TotalDeposit - CokeCosts;
            Console.WriteLine($"Gave Back ${requiredChange}");
            MyPocketChange += TotalDeposit - CokeCosts;
            TotalDeposit = 0;
        }

        private bool IsEnoughDeposited(MachineState state)
        {
            Console.WriteLine($"MaterialTotalPrice Deposited is {TotalDeposit} for Coke that costs: {CokeCosts}");
            return (TotalDeposit >= CokeCosts);
        }


    }
}
