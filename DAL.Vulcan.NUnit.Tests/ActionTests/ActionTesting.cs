using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.ActionTests
{
    [TestFixture]
    public class ActionTesting
    {
        [Test]
        public void DoIt()
        {
            List<Action> myActions = new List<Action>();

            myActions.Add(() => WithParameters("one"));
            myActions.Add(() => WithParameters("one","two"));
            myActions.Add(() => WithParameters("one","two","three"));
            myActions.Add(() => OneParameter("A single parameter"));
            myActions.Add(() => NoParameters());

            foreach (var action in myActions)
            {
                action.Invoke();
            }

        }
        public void NoParameters()
        {
            Trace.WriteLine("No Parameters");
        }

        public void WithParameters(params string[] parameters)
        {
            Trace.WriteLine(string.Join(",", parameters));
        }

        public void OneParameter(string oneParameter)
        {
            Trace.WriteLine(oneParameter);
        }

    }

}
