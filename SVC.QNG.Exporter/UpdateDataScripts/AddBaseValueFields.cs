using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SVC.QNG.Exporter.UpdateDataScripts
{
    [TestFixture()]
    public class AddBaseValueFields
    {
        private List<string> newProductionFields = new List<string>()
        {
            "BaseCurrency",
            "BaseInternalCost",
            "BaseMinimumCost",
            "BaseProductionCost",
            "BaseTotalInternalCost",
            "BaseTotalProductionCost"
        };

        private List<string> kerfFields = new List<string>()
        {
            "BaseKerfTotalCost",
            "BaseKerfTotalPrice"
        };

        private List<string> quickQuoteFields = new List<string>()
        {
            "BaseQuickQuoteItEmprice"
        };

        private List<string> newFieldList = new List<string>
        {
            "BaseCostPerInch",
            "BaseCostPerKg",
            "BaseCostPerPiece",
            "BaseCostPerPound",
            "BaseCutCostPerPiece",
            "BaseCutCostPerPieceOverride",
            "BaseFinalMargin",
            "BaseFinalPrice",
            "BaseFinalPriceOverrideValue",
            "BaseMaterialOnlyCost",
            "BaseMaterialTotalPrice",
            "BaseMaterialTotalPriceOverride",
            "BasePricePerEach",
            "BasePricePerFoot",
            "BasePricePerFootOverride",
            "BasePricePerInch",
            "BasePricePerInchOverride",
            "BasePricePerKilogram",
            "BasePricePerKilogramOverride",
            "BasePricePerPound",
            "BasePricePerPoundOverride",
            "BaseProductionCostTotal",
            "BaseProductionPriceTotal",
            "BaseTotalCost",
            "BaseTotalCutCost"


        };

        [Test]
        public void GenerateAlterTableScript()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var fieldName in newFieldList)
            {
                stringBuilder.AppendLine(
                    $"ALTER TABLE dbo.[Vulcan.CrmQuoteItem] ADD {fieldName} decimal NOT NULL DEFAULT (0)");
            }

            stringBuilder.AppendLine("GO");
            Console.WriteLine(stringBuilder.ToString());
        }

        [Test]
        public void GenerateKerfScript()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var fieldName in kerfFields)
            {
                stringBuilder.AppendLine(
                    $"ALTER TABLE dbo.[Vulcan.CrmQuoteItem] ADD {fieldName} decimal NOT NULL DEFAULT (0)");
            }

            stringBuilder.AppendLine("GO");
            Console.WriteLine(stringBuilder.ToString());
        }

        [Test]
        public void GenerateQuickQuoteScript()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var fieldName in quickQuoteFields)
            {
                stringBuilder.AppendLine(
                    $"ALTER TABLE dbo.[Vulcan.CrmQuoteItem] ADD {fieldName} decimal NOT NULL DEFAULT (0)");
            }

            stringBuilder.AppendLine("GO");
            Console.WriteLine(stringBuilder.ToString());
        }



        [Test]
        public void GenerateProductionAlterTableScript()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var fieldName in newProductionFields)
            {
                stringBuilder.AppendLine(
                    $"ALTER TABLE dbo.[Vulcan.CrmQuoteItem.ProductionCost] ADD {fieldName} decimal NOT NULL DEFAULT (0)");
            }

            stringBuilder.AppendLine("GO");
            Console.WriteLine(stringBuilder.ToString());
        }

    }
}
