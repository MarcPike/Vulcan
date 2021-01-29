using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.LostReasons
{
    [TestFixture]
    class LossReasonCleanup
    {
        private class NewLostReason
        {
            public string OldReason { get; set; }
            public LostReason NewReason { get; set; }

            public NewLostReason()
            {
                
            }

            public NewLostReason(string oldReason, LostReason newReason)
            {
                OldReason = oldReason;
                NewReason = newReason;
            }

        }

        private Dictionary<int, LostReason> _newLossReasonList = new Dictionary<int, LostReason>();
        private List<NewLostReason> _conversionList = new List<NewLostReason>();
        private LostReason _defaultLostReason;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
            AddNewLossReasons();
            LoadConversionList();
        }

        [Test]
        public void DoCleanup()
        {
            AssignNewReasonsToLostQuotes();
            AssignNewReasonsToLostQuoteItems();

            RemoveAllUnusedReasons();

            void AssignNewReasonsToLostQuotes()
            {
                var lostQuotes = CrmQuote.Helper.Find(x => x.LostDate != null).ToList();
                var onQuote = 0;
                foreach (var lostQuote in lostQuotes)
                {
                    onQuote++;
                    var thisLostReason = LostReason.Helper.FindById(lostQuote.LostReasonId);
                    if (thisLostReason == null)
                    {
                        SetQuoteToOther(lostQuote, "LostReason no longer in system");
                        continue;
                    }

                    var newLostReason = _conversionList.FirstOrDefault(x => x.OldReason == thisLostReason.Reason);

                    if (newLostReason == null)
                    {
                        SetQuoteToOther(lostQuote, thisLostReason.Reason);
                        continue;
                    }

                    SetQuoteToThisReason(lostQuote, newLostReason.NewReason, thisLostReason.Reason);
                }
            }

            void AssignNewReasonsToLostQuoteItems()
            {
                var lostQuoteItems = CrmQuoteItem.Helper.Find(x => x.LostDate != null).ToList();
                var onQuoteItem = 0;
                foreach (var lostQuoteItem in lostQuoteItems)
                {
                    onQuoteItem++;
                    var thisLostReason = LostReason.Helper.FindById(lostQuoteItem.LostReasonId);
                    if (thisLostReason == null)
                    {
                        LogLostQuoteItemUpdate(lostQuoteItem, _defaultLostReason);
                        SetQuoteItemToOther(lostQuoteItem, "LostReason no longer in system");
                        continue;
                    }

                    var newLostReason = _conversionList.FirstOrDefault(x => x.OldReason == thisLostReason.Reason);

                    if (newLostReason == null)
                    {
                        SetQuoteItemToOther(lostQuoteItem, thisLostReason.Reason);
                        continue;
                    }

                    SetQuoteItemToThisReason(lostQuoteItem, newLostReason.NewReason, thisLostReason.Reason);
                }
            }

            void SetQuoteItemToThisReason(CrmQuoteItem lostQuoteItem, LostReason lostReason, string lostComments)
            {
                LogLostQuoteItemUpdate(lostQuoteItem, lostReason);

                lostQuoteItem.LostReasonId = lostReason.Id.ToString();
                lostQuoteItem.LostComments = lostComments;
                CrmQuoteItem.Helper.Upsert(lostQuoteItem);
            }

            void SetQuoteItemToOther(CrmQuoteItem lostQuoteItem, string lostComments)
            {
                LogLostQuoteItemUpdate(lostQuoteItem, _defaultLostReason);

                lostQuoteItem.LostReasonId = _defaultLostReason.Id.ToString();
                lostQuoteItem.LostComments = lostComments;
                CrmQuoteItem.Helper.Upsert(lostQuoteItem);
            }


            void SetQuoteToThisReason(CrmQuote lostQuote, LostReason lostReason, string lostComments)
            {
                LogLostQuoteUpdate(lostQuote, lostReason);

                lostQuote.LostReasonId = lostReason.Id.ToString();
                lostQuote.LostComments = lostComments;
                CrmQuote.Helper.Upsert(lostQuote);
            }

            void SetQuoteToOther(CrmQuote lostQuote, string lostComments)
            {
                LogLostQuoteUpdate(lostQuote, _defaultLostReason);

                lostQuote.LostReasonId = _defaultLostReason.Id.ToString();
                lostQuote.LostComments = lostComments;
                CrmQuote.Helper.Upsert(lostQuote);

            }

            void LogLostQuoteUpdate(CrmQuote lostQuote, LostReason newLostReason)
            {
                var lostReasonUpdate = new LostReasonUpdate()
                {
                    NewLostReasonId = newLostReason.Id,
                    NewLostReason = newLostReason.Reason,
                    OldLostReasonId = (lostQuote.LostReasonId != string.Empty) ? ObjectId.Parse(lostQuote.LostReasonId) : ObjectId.Empty,
                    OldLostReason = LostReason.Helper.FindById(lostQuote.LostReasonId)?.Reason,
                    QuoteId = lostQuote.Id
                };
                LostReasonUpdate.Helper.Upsert(lostReasonUpdate);
            }

            void LogLostQuoteItemUpdate(CrmQuoteItem lostQuoteItem, LostReason newLostReason)
            {
                var lostReasonUpdate = new LostReasonUpdate()
                {
                    NewLostReasonId = newLostReason.Id,
                    NewLostReason = newLostReason.Reason,
                    OldLostReasonId = (lostQuoteItem.LostReasonId != string.Empty) ? ObjectId.Parse(lostQuoteItem.LostReasonId) : ObjectId.Empty,
                    OldLostReason = LostReason.Helper.FindById(lostQuoteItem.LostReasonId)?.Reason,
                    QuoteItemId = lostQuoteItem.Id
                };
                LostReasonUpdate.Helper.Upsert(lostReasonUpdate);
            }

            void RemoveAllUnusedReasons()
            {
                var validLostReasons = _newLossReasonList.Values;

                var lostReasons = LostReason.Helper.GetAll();
                foreach (var lostReason in lostReasons)
                {
                    if (validLostReasons.All(x => x.Id != lostReason.Id))
                    {
                        var quoteCountForThisReason =
                            CrmQuote.Helper.Find(x => x.LostReasonId == lostReason.Id.ToString()).CountDocuments();
                        var quoteItemCountForThisReason =
                            CrmQuoteItem.Helper.Find(x => x.LostReasonId == lostReason.Id.ToString()).CountDocuments();
                        if ((quoteItemCountForThisReason + quoteItemCountForThisReason) == 0)
                        {
                            LostReason.Helper.DeleteOne(lostReason.Id);
                        }
                    }
                }
            }


        }


        private void AddNewLossReasons()
        {
            var lossReasonsNew = new List<string>()
            {
                "1 - Price",
                "2 - Lead Time",
                "3 - No Stock",
                "4 - Did not meet technical requirements",
                "5 - Customer did not win order",
                "6 - Credit / Payment issues",
                "7 - Other"
            };

            var index = 0;
            foreach (var reason in lossReasonsNew)
            {
                var lostReason = LostReason.Helper.Find(x => x.Reason == reason).FirstOrDefault();
                if (lostReason == null)
                {
                    lostReason = new LostReason() {
                        Reason = reason
                    };
                    LostReason.Helper.Upsert(lostReason);
                }

                index++;
                _newLossReasonList.Add(index,lostReason);
            }

            _defaultLostReason = _newLossReasonList.Last().Value;
        }

        private void LoadConversionList()
        {
            var spreadSheetList = new Dictionary<string, int>()
            {
                {"Price",1},
                {"Lead Time",2},
                {"Inventory",3},
                {"Specification",4},
                {"OD too larger. Need less than 1\" OD",3},
                {"ONLY ORDER QTY. NEEDED",7},
                {"No Demand",5},
                {"Found .1.75\" @ $13.25/in",1},
                {"OD not on size (1.5\")",3},
                {"Size & Price not what needed",3},
                {"unknown",7},
                {"Quote was 25% higher than winning bid.",1},
                {"Customer not awarded job",5},
                {"Stock quoted no longer available",3},
                {"Accounting/Credit",7},
                {"Needed 1.125\" OD",3},
                {"Job Cancelled",5},
                {"Not size needed",3},
                {"Customer inventory",7},
                {"Price & lead-time",1},
                {"No Stock",3},
                {"Bid Only at this Stage",5},
                {"Out on Price 10-15 %",1},
                {"turn key through sub contractor",5},
                {"Re Quote - Increased Qty",7},
                {"Item 4 - out on price under 10%, item 5 out on price - over 10% -",1},
                {"They don't have the PO to place yet, but we are under 10% out on price on item 4 and over 10% out on price on item 5",1},
                {"Placed Elsewhere",7},
                {"No stock - Can't wait on incoming",3},
                {"Still under review at this time - lost the items with concessions",4},
                {"+ 20% OUT ON PRICE",1},
                {"Hi",1},
                {"12% HIGH ON PRICE UNABLE TO FULFIL ALL QUANTITY REQUIRED",1},
                {"CUSTOMER CANNOT ACCEPT CHINESE",4},
                {"No longer required by customer.",5},
                {"PURCHASED AS RANDOM LENGTHS ON PO ODA0561887-2",7},
                {"placed with competitor who had customers ideal size",3},
                {"Customer wanted to add 20% penalty charge for late delivery",1},
                {"They prefer CG we have no Carpenter only SMC",4},
                {"Lost item 1 on Price - Placed item 2 with us",1},
                {"lead time - GIR material held up in QA",2},
                {"PRICE AND DELIVERY",1},
                {"Price - 5% high",1},
                {"Btw 5-10% high",1},
                {"Min order amount",1},
                {"Sales person out of office",7},
                {"order placed via Cumbernauld by Belmar",7},
                {"high on price",1},
                {"Placed with historical supplier despite our price being competitive",4},
                {"buyer error",5},
                {"Placed with Howco but with another Customer.",5},
                {"Went for 2\" OD on same quote instead",3},
                {"Prices in in line with other suppliers, RB quoted Diego but Paolo placed with other supplier",7},
                {"lb test",4},
                {"test",4},
                {"Bought 4140 5.25 ANN on Q# 58269",7},
                {"Won Q# 56306 instead (Turn and Drill)",7},
                {"Won Q# 56306 (Turn and Drill)",7},
                {"14% high on price",1},
                {"10% high on price",1},
                {"No Chinese steel",4},
                {"Won on another quote.",7},
                {"Revised quote after finding that we were too high.",1},
                {"UNABLE TO SUPPORT CREDIT WISE",7},
                {"Offered Alternative Grades to achieve properties, concessions on chemistry and CEV values, price on F6NM item",4},
                {"Regular buyer on hols, someone else covering, because we could not offer all items placed with competitor. Usual buyer splits orders",3},
                {"moved to a different customer plant",5},
                {"cost was high",1},
                {"drops at cheaper price",7},
                {"PLACED W/ ANOTHER SUPPLIER, BUT OUR PRICE WAS LOWER.",2},
                {"NOT THE SIZE THEY NEEDED",3},
                {"Found tubing size needed as stock",3},
                {"doesnt meet customers hardness requirement",4},
                {"Placed with an Italian forgemasters",1},
                {"8% high on price",1},
                {"Wrong Condition in stock. Must Heat Treat.",4},
                {"Cannot wait for incoming material.",3},
                {"Yld must be 110 or less. No variance granted.",4},
                {"87% Out on Price",1},
                {"+30% Out on Price",1},
                {"+35% Out on Price",1},
                {"14% HIGHER",1},
                {"Offered as alternate to grade requested. Cannot use.",4},
                {"Grade switched to 4330V",4},
                {"material would require ID drill - found to size elswhere",3},
                {"LOST NEARLY 70% OUT ON PRICE, THESE PARTS WERE BASED ON RE-FORGING AS NO SUITABLE STOCK AVAILABLE TO BASE ON.",1},
                {"LOST ON PRICE, NEARLY 70% OUT, BASED ON RE-FORGING AS WE DO NOT HAVE ANY SUITABLE TO MACHINE FROM",1},
                {"DUMOING STOCK",7},
                {"TST would not release stock",3},
                {"high on price 15% approx",1},
                {"placed on item 3",7},
                {"Orders have already been placed",2},
                {"Not Required - 1 pc of each part placed under quote 78578",7},
                {"ORDER PLACED ON CRIBALLET BY CAMERON, HOWCO HAVE WON FROM CRIBALLET",7},
                {"Can use a small size",3},
                {"Advised due to testing requirements to decline to offer by Technical",4},
                {"INTERNAL COST",1},
                {"ORDER PLACED VIA. KSW ENGINEERING",7},
                {"MTR REJECTED",4},
                {"concession offered on chemistry - competition offering to spec",4},
                {"OD not in stock; found 14\" elsewhere",3},
                {"Belmar placed Order",7},
                {"Ordered on item 2",7},
                {"ALTERNATE SIZE OFFERED",3},
                {"Lead Time - can't wait",2},
                {"placed under different quote number/part number",7},
                {"Ordered on item 1",7},
                {"prices/concessions",1},
                {"Competitor had on size in full qty",3},
                {"Order minimum",1},
                {"15% out of price",1},
                {"Found material in stock",5},
                {"Placed with Howco",7},
                {"cannot meet target price per inch of €450.00. We quoted € 1676.66",1},
                {"Out Of Stock",3},
                {"spec changed - condition not avail.",4},
                {"Concessions",4},
                {"Customer would not accept to pay Pro-foma",1},
                {"Placed with Competitor they offered Forgings - lead time not an issue - delivery late 2020 and early 2021",2},
                {"Pricing for quote purposes",1},
                {"PRICE",1}
            };

            foreach (var dictValue in spreadSheetList)
            {
                _conversionList.Add(new NewLostReason(dictValue.Key, _newLossReasonList[dictValue.Value] ));
            }
        }
    }
}
