using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    public static class RegisterExcelClassMaps
    {
        public static void Execute()
        {
            BsonClassMap.RegisterClassMap<ExcelQuoteCompanyCode>();
            BsonClassMap.RegisterClassMap<ExcelQuoteCompanyName>();
            BsonClassMap.RegisterClassMap<ExcelQuoteContactName>();
            BsonClassMap.RegisterClassMap<ExcelQuoteCustomerNotes>();
            BsonClassMap.RegisterClassMap<ExcelQuoteDisplayCurrency>();
            BsonClassMap.RegisterClassMap<ExcelQuoteFreightTerms>();
            BsonClassMap.RegisterClassMap<ExcelQuoteLastModified>();
            BsonClassMap.RegisterClassMap<ExcelQuoteOrderDate>();
            BsonClassMap.RegisterClassMap<ExcelQuoteOrderTotal>();
            BsonClassMap.RegisterClassMap<ExcelQuotePaymentTerms>();
            BsonClassMap.RegisterClassMap<ExcelQuotePoNumber>();
            BsonClassMap.RegisterClassMap<ExcelQuoteProspectName>();
            BsonClassMap.RegisterClassMap<ExcelQuoteQuoteId>();
            BsonClassMap.RegisterClassMap<ExcelQuoteRevision>();
            BsonClassMap.RegisterClassMap<ExcelQuoteRfqNumber>();
            BsonClassMap.RegisterClassMap<ExcelQuoteSalesPerson>();
            BsonClassMap.RegisterClassMap<ExcelQuoteShipToAddress>();
            BsonClassMap.RegisterClassMap<ExcelQuoteShipToName>();
            BsonClassMap.RegisterClassMap<ExcelQuoteSubmitDate>();
            BsonClassMap.RegisterClassMap<ExcelQuoteSubmitTime>();
            BsonClassMap.RegisterClassMap<ExcelQuoteTotalWeightKilograms>();
            BsonClassMap.RegisterClassMap<ExcelQuoteTotalWeightPounds>();
            BsonClassMap.RegisterClassMap<ExcelQuoteValidity>();
            BsonClassMap.RegisterClassMap<ExcelQuoteValidityDate>();

            BsonClassMap.RegisterClassMap<ExcelQuoteItemProductionSteps>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemCurrency>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemCustomerNotes>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemCustomerUom>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemFinishedProduct>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemLastModified>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemLeadTime>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemLineItemNumber>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemLineType>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemOemType>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemOrderUnit>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemPartNumber>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemPartSpecification>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemPieces>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemQuantity>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemStartingProduct>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemTestPieces>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemTotal>();
            BsonClassMap.RegisterClassMap<ExcelQuoteItemUnitPrice>();

        }
    }
}
