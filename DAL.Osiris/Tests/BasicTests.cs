using System;
using NUnit.Framework;

namespace DAL.Osiris.Tests
{
    [TestFixture]
    public class BasicTests
    {
        [Test]
        public void GetOsirisDocuments()
        {
            var repository = new OsirisRepository();
            var docs = repository.GetOsirisDocumentList("INC", "1259973B", "");
            foreach (var osirisDocInfo in docs)
            {
                Console.Write(ObjectDumper.Dump(osirisDocInfo));
            }
        }
    }
}
