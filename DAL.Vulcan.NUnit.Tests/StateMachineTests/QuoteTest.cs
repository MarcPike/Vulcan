using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Base.StateMachine;
using DAL.Vulcan.Mongo.DocClass.CRM;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.StateMachineTests
{
    [TestFixture()]
    public class QuoteTest
    {
        Random _quoteAmount = new Random(10000);
        Random _creditLimit = new Random(6000);

        private readonly StateMachine _quoteTest = new StateMachine();

        [SetUp]
        public void SetUp()
        {
            _quoteTest.AddNewMachineState("Created");

            _quoteTest.AddNewMachineState("Submit");
            _quoteTest.AddNewMachineState("Review");
            _quoteTest.AddNewMachineState("Approved");
            _quoteTest.AddNewMachineState("Sent");
            _quoteTest.AddNewMachineState("Order");

        }

    }

    public class TestQuote: BaseDocument
    {
        public int Amount { get; set; }
        public Mongo.DocClass.Companies.Company Customer { get; set; }
        public int CreditLimit { get; set; }
        //public Manager ApprovedBy { get; set; } = new RepositoryBase<Manager>();

        //public TestQuote GenerateForTest()
        //{

        //    //return new TestQuote()
        //    //{
                   
        //    //} 
        //}

    }

}
