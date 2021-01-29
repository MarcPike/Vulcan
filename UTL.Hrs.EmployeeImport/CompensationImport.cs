using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.HRS.Mongo.DocClass.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Common.DocClass;
using DAL.Common.Ldap.DAL.WindowsAuthentication.MongoDb;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using MongoDB.Driver;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using System.Threading.Tasks;

namespace UTL.Hrs.EmployeeImport
{
    public class CompensationImport : BaseImportData
    {
        private readonly HelperEmployee _helperEmployee = new HelperEmployee();
        private readonly HelperProperties _helperProperties = new HelperProperties();
        private readonly HelperCompensation _helperCompensation = new HelperCompensation();
        private List<Location> _locations = new List<Location>();
        private List<CurrencyType> _currencyTypes = new List<CurrencyType>();
        private EntityRef _entity;
        private List<PayrollRegion> _payrollRegions = new List<PayrollRegion>();


        public CompensationImport(DataTable dt) : base(dt)
        {

        }

        public CompensationImport(string fileName) : base(fileName)
        {
            ConfigureEntityInfo();
            InitializePayrollRegions();
        }



        private void InitializePayrollRegions()
        {
            var emRegions = new List<string>()
            {
                "EM - Australia",
                "EM - UAE",
                "EM - Europe",
                "EM - Asia Pacific"
            };

            var queryHelper = new MongoRawQueryHelper<PayrollRegion>();
            foreach (var region in emRegions)
            {
                var filter = queryHelper.FilterBuilder.Where(x => x.Name == region);
                var payrollRegion = queryHelper.Find(filter).SingleOrDefault();
                if (payrollRegion == null)
                {
                    payrollRegion = new PayrollRegion()
                    {
                        Name = region,
                    };
                    queryHelper.Upsert(payrollRegion);
                }
                _payrollRegions.Add(payrollRegion);
            }


        }

        //private void GetCurrencyTypes()
        //{
        //    _currencyTypes = new MongoRawQueryHelper<CurrencyType>().GetAll();
        //}

        public override void Execute()
        {
            Execute(null);
        }

        public override void Execute(HrsUserRef hrsUser)
        {
            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();
            var onRow = 0;

            const bool TEST_A = false;


            Parallel.ForEach(Data.AsEnumerable(), dataRow =>
            //foreach (DataRow dataRow in Data.Rows)
            {
                onRow++;



                var payrollId = dataRow["Payroll ID"].ToString();
                //if (String.IsNullOrEmpty(payrollId)) continue;
                if (String.IsNullOrEmpty(payrollId)) return;



                var filter = Employee.Helper.FilterBuilder.Where(x => x.PayrollId == payrollId); 

                var emp = Employee.Helper.Find(x => x.PayrollId == payrollId).FirstOrDefault();

                if (emp == null)
                {
                    Console.WriteLine($"PayrollId incorrect for {dataRow["Payroll ID"]}"); 
                    //continue;
                    return;
                }

                //var hrsUser = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();
                //model.ModifiedBy = hrsUser;

                DAL.HRS.Mongo.Models.CompensationModel compModel = null;
               compModel = _helperCompensation.GetCompensationForEmployee(emp.Id.ToString());


                //var comp = emp.Compensation;
                //var enc = new DAL.Vulcan.Mongo.Base.Encryption.Encryption();
                var enc = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;




                if (Data.Columns.Contains("Rate Type") && dataRow["Rate Type"] != null)
                {
                    if (!TEST_A)
                    {
                        compModel.PayRateAmount = Convert.ToDecimal(dataRow["Pay Rate"]);

                        compModel.PayRateType = PropertyBuilder.New("PayRateType", "Type of Pay Rate",
                            dataRow["Rate Type"].ToString(), string.Empty, _entity);
                        compModel.PayFrequencyType = PropertyBuilder.CreatePropertyValue("PayFrequencyType",
                            "Type of Pay Frequency",
                            dataRow["Pay Period"].ToString(), string.Empty, _entity).AsPropertyValueRef();
                        compModel.EffectiveDate = Convert.ToDateTime(dataRow["Effective Date"]);
                        compModel.CurrencyType = PropertyBuilder.CreatePropertyValue("CurrencyType", "Type of currency",
                            dataRow["Currency"].ToString(), string.Empty, _entity).AsPropertyValueRef();
                        compModel.IncreaseType = PropertyBuilder.CreatePropertyValue("IncreaseType", "Type of Increase",
                            dataRow["Increase Type"].ToString(), string.Empty, _entity).AsPropertyValueRef();
                        compModel.BaseHours = Convert.ToDecimal(dataRow["Base Hours"]);
                    }
                    else
                    {
                        //comp.PayRateAmount = enc.Encrypt(Convert.ToDecimal(dataRow["Pay Rate"]));
                        ////comp.PayRateType = payRateType;
                        ////comp.PayFrequencyType = payFrequencyType;
                        //comp.EffectiveDate = enc.Encrypt(Convert.ToDateTime(dataRow["Effective Date"]));
                        ////comp.CurrencyType = currencyType;
                        //comp.IncreaseType = PropertyBuilder.CreatePropertyValue("IncreaseType", "Type of Increase",
                        //    dataRow["Increase Type"].ToString(), string.Empty, _entity).AsPropertyValueRef();
                        //comp.BaseHours = Convert.ToDecimal(dataRow["Base Hours"]);
                    }
                }







                //}
                if (Data.Columns.Contains("Bonus Type") && dataRow["Bonus Type"] != null)
                {
                    if (!TEST_A)
                    {
                        compModel.BonusHistory.Add(new DAL.HRS.Mongo.Models.BonusModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            DatePaid = Convert.ToDateTime(dataRow["Date Paid"]),
                            Amount = Convert.ToDecimal(dataRow["Amount"]),

                            BonusType = PropertyBuilder.New("BonusType", "Type of Bonus",
                        dataRow["Bonus Type"].ToString(), string.Empty, _entity),

                            PercentPaid = Convert.ToDecimal(dataRow["Percent Paid"]),
                            FiscalYear = Convert.ToInt16(dataRow["Fiscal Year"]),
                            Comment = dataRow["Comment"].ToString()

                        });
                    }
                    else
                    {
                        //comp.BonusHistory.Add(new Bonus
                        //{
                        //    Id = Guid.NewGuid(),
                        //    DatePaid = Convert.ToDateTime(dataRow["Date Paid"]),
                        //    Amount = Convert.ToDecimal(dataRow["Amount"]),

                        //    BonusType = PropertyBuilder.New("BonusType", "Type of Bonus",
                        //        dataRow["Bonus Type"].ToString(), string.Empty, _entity),

                        //    PercentPaid = Convert.ToDecimal(dataRow["Percent Paid"]),
                        //    FiscalYear = Convert.ToInt16(dataRow["Fiscal Year"]),
                        //    Comment = dataRow["Comment"].ToString()
                        //});
                    }

                }
                if (Data.Columns.Contains("Bonus Scheme Type") && dataRow["Bonus Scheme Type"] != null)
                {
                    if (!TEST_A)
                    {
                        compModel.BonusScheme.Add(new DAL.HRS.Mongo.Models.BonusSchemeModel
                        {

                            Id = Guid.NewGuid().ToString(),
                            EffectiveDate = Convert.ToDateTime(dataRow["Bonus Effective Date"]),

                            BonusSchemeType = PropertyBuilder.New("BonusSchemeType", "Type of Bonus Scheme",
                                dataRow["Bonus Scheme Type"].ToString(), string.Empty, _entity),

                            TargetPercentage = ((float)Convert.ToDecimal(dataRow["Bonus Target %"])) / 100,

                            Comment = dataRow["Comment"].ToString()


                        });
                    } 
                    else
                    {
                        //comp.BonusScheme.Add(new BonusScheme
                        //{
                        //    Id = Guid.NewGuid(),
                        //    EffectiveDate = Convert.ToDateTime(dataRow["Bonus Effective Date"]),
                        //    BonusSchemeType = PropertyBuilder.New("BonusSchemeType", "Type of Bonus Scheme",
                        //        dataRow["Bonus Scheme Type"].ToString(), string.Empty, _entity),
                        //    TargetPercentage = ((float)Convert.ToDecimal(dataRow["Bonus Target %"])) / 100,
                        //    Comment = dataRow["Comment"].ToString()
                        //}); 
                    }
                   

                }

                //// DO NOT USE
                ////compModel.BonusEligible = PropertyBuilder
                ////    .CreatePropertyValue("Bonus Eligible Type", "Is Bonus Eligible", "No", "Not Bonus Eligible", _entity)
                ////    .AsPropertyValueRef();
                ////compModel.PayGradeType = PropertyBuilder
                ////    .CreatePropertyValue("PayGradeType", "Type of Pay Grade", "Unspecified", "", _entity).AsPropertyValueRef();
                ///

                _helperCompensation.SaveCompensation(compModel, hrsUser: hrsUser);

                ////emp.Compensation = new Compensation()

                emp = Employee.Helper.Find(x => x.PayrollId == payrollId).FirstOrDefault();

                ////emp.PayrollRegion = _payrollRegions.First(x => x.Name == dataRow["PayrollRegion"].ToString())
                ////    .AsPayrollRegionRef();
                ////Employee.Helper.Upsert(emp); // another way to do the same thing, just for reference

                queryHelperEmployee.Upsert(emp); 

               // Employee.Helper.Upsert(emp);

            });

        }

        private void ConfigureEntityInfo()
        {
            var queryHelper = new MongoRawQueryHelper<Entity>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Name == "Edgen Murray");
            var entity = queryHelper.Find(filter).First();
            _entity = entity.AsEntityRef();


        }

    }
}