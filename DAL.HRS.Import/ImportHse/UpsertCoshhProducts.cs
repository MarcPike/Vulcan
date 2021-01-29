using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using CoshhProduct = DAL.HRS.Mongo.DocClass.Hse.CoshhProduct;
using CoshhRiskAssessment = DAL.HRS.Mongo.DocClass.Hse.CoshhRiskAssessment;
using GhsClassificationType = DAL.HRS.Mongo.DocClass.Hse.GhsClassificationType;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;

namespace DAL.HRS.Import.ImportHse
{
    [TestFixture]
    public class UpsertCoshhProducts
    {
        private HseHelperMethods _helperMethods = new HseHelperMethods();
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<CoshhProduct>();
            var repDisposalMethod = new RepositoryBase<CoshhDisposalMethod>();
            var repFirstAidMeasure = new RepositoryBase<CoshhFirstAidMeasure>();
            var repHazardType = new RepositoryBase<CoshhHazardType>();
            var repProductUsages = new RepositoryBase<CoshhProductUsage>();
            var repStorageMethods = new RepositoryBase<CoshhStorageMethod>();
            var repRiskAssessments = new RepositoryBase<CoshhRiskAssessment>();
            var repGhsClassType = new RepositoryBase<GhsClassificationType>();
            var repManufacturer = new RepositoryBase<CoshhProductManufacturer>();
            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                var ghsClassifications = ImportGhsClassificationTypes(context);
                var manufacturers = ImportManufacturers(context);
                foreach (var product in context.CoshhProduct.ToList())
                {
                    var newRow = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == product.OID) ?? new CoshhProduct()
                    {
                        OldHrsId = product.OID
                    };

                    newRow.ApplicationMethod = product.ApplicationMethod;
                    newRow.Description = product.Description;
                    newRow.DurationOfExposure = product.DurationOfExposure;

                    newRow.GhsClassifications.Clear();
                    foreach(var ghsClass in product.GhsClassificationGhsClassifications_CoshhProductProducts.ToList())
                    {
                        var ghsProductClass = new CoshhProductGhsClassification()
                        {
                            Comment = ghsClass.GhsClassification.Comment,
                            ClassificationType = repGhsClassType.AsQueryable().First(x=>x.OldHrsId == ghsClass.GhsClassification.GhsClassificationType.OID).AsGhsClassificationTypeRef()
                        };
                    }

                    newRow.GhsDescription = product.GhsDescription;
                    var thisManufacturer = manufacturers.FirstOrDefault(x => x.OldHrsId == product.Manufacturer1.OID);
                    if (thisManufacturer != null)
                    {
                        newRow.Manufacturer = thisManufacturer.AsCoshhProductManufacturerRef();
                    }
                    
                    
                    newRow.DisposalMethods.Clear();
                    foreach (var method in product.CoshhProducts_DisposalMethods.Select(x => x.DisposalMethod.Method))
                    {

                        newRow.DisposalMethods.Add(CoshhDisposalMethod.CreateAndReturnRef(repDisposalMethod,method));
                    }

                    newRow.FirstAidMeasures.Clear();
                    foreach (var coshhProductsFirstAidMeasurese in product.CoshhProducts_FirstAidMeasures.ToList())
                    {
                        var action = coshhProductsFirstAidMeasurese.FirstAidMeasure.Action;
                        var exposureType = PropertyBuilder.New(
                            "CoshhExposureType",
                            "Coshh First Aid Exposure Type", 
                            coshhProductsFirstAidMeasurese.FirstAidMeasure.RouteOfExposureType.Name, string.Empty );

                        newRow.FirstAidMeasures.Add(CoshhFirstAidMeasure.CreateAndReturnRef(repFirstAidMeasure, 
                            action, exposureType));
                    }

                    newRow.HazardTypes.Clear();
                    foreach (var hazardType in product.CoshhProducts_HazardTypes.ToList())
                    {
                        newRow.HazardTypes.Add(CoshhHazardType.CreateAndReturnRef(repHazardType, 
                            hazardType.HazardType.Name));
                    }

                    newRow.ProductUsages.Clear();
                    foreach (var coshhProductsProductUsageTypese in product.CoshhProducts_ProductUsageTypes.ToList())
                    {
                        newRow.ProductUsages.Add(CoshhProductUsage.CreateAndReturnRef(repProductUsages, 
                            coshhProductsProductUsageTypese.ProductUsageType.Usage));
                    }

                    newRow.StorageMethods.Clear();
                    foreach (var storageMethod in product.CoshhProducts_StorageMethods.ToList())
                    {
                        newRow.StorageMethods.Add(CoshhStorageMethod.CreateAndReturnRef(repStorageMethods, storageMethod.StorageMethod.Method));
                    }

                    newRow.RiskAssessments.Clear();
                    foreach (var riskAssessment in product.CoshhRiskAssessment.ToList())
                    {
                        var newRiskAssessment = repRiskAssessments.AsQueryable()
                            .FirstOrDefault(x => x.ProcessDescription == riskAssessment.ProcessDescription);

                        if (newRiskAssessment == null)
                        {
                            newRiskAssessment = new CoshhRiskAssessment()
                            {
                                ApprovedBy = riskAssessment.ApprovedBy,
                                ApprovedOn = riskAssessment.ApprovedOn,
                                AssessmentDate = riskAssessment.AssessmentDate,
                                Department = PropertyBuilder.New("Department", "Howco Department",
                                    riskAssessment.Department1.DepartmentCode,
                                    riskAssessment.Department1.DepartmentName),
                                IsExposureControlled = riskAssessment.IsExposureControlled ?? false,
                                IsExposureControlledComment = riskAssessment.IsExposureControlledComment,
                                Location = _helperMethods.GetLocationForHrsLocation(riskAssessment.Location1.Name),
                                MaterialDisposedProperly = riskAssessment.MatlDisposedProperly ?? false,
                                MaterialDisposedProperlyComment = riskAssessment.MatlDisposedProperlyComment,
                                MaterialStoredProperly = riskAssessment.MatlStoredProperly ?? false,
                                MaterialStoredProperlyComment = riskAssessment.MatlStoredProperlyComment,
                                NextReviewDate = riskAssessment.NextReviewDate,
                                PreparedBy = riskAssessment.PreparedBy,
                                ProcessDescription = riskAssessment.ProcessDescription,
                                RiskRating = PropertyBuilder.New("RiskRating", "Coshh Risk Rating",
                                    riskAssessment.RiskRating.Name, String.Empty),
                                RiskRatingComment = riskAssessment.RiskRatingComment,
                            };
                            repRiskAssessments.Upsert(newRiskAssessment);
                            newRow.RiskAssessments.Add(newRiskAssessment.AsCoshhRiskAssessmentRef());
                        }
                    }

                    newRow.MsdsAvailable = product.MsdsAvailable ?? false;

                    newRow.ProductType = PropertyBuilder.New("CoshhProductType", "Coshh Product Type", product.ProductType1.Name, "");

                    rep.Upsert(newRow);
                }
            }
        }

        private List<CoshhProductManufacturer> ImportManufacturers(HrsContext context)
        {
            var rep = new RepositoryBase<CoshhProductManufacturer>();
            foreach (var manufacturer in context.Manufacturer)
            {
                var newRow = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == manufacturer.OID) ??
                             new CoshhProductManufacturer()
                             {
                                 OldHrsId = manufacturer.OID
                             };
                newRow.Name = manufacturer.Name;
                newRow.Address1 = manufacturer.Address1;
                newRow.Address2 = manufacturer.Address2;
                newRow.Address3 = manufacturer.Address3;
                newRow.Contact1 = manufacturer.Contact1;
                newRow.Contact2 = manufacturer.Contact2;
                newRow.EmailAddress = manufacturer.EmailAddress;
                newRow.Website = manufacturer.Website;
                newRow.Location = _helperMethods.GetLocationForHrsLocation(manufacturer.Location1.Name);
                rep.Upsert(newRow);
            }

            return rep.AsQueryable().ToList();
        }

        private List<GhsClassificationType> ImportGhsClassificationTypes(HrsContext context)
        {
            var rep = new RepositoryBase<GhsClassificationType>();
            foreach (var classificationType in context.GhsClassificationType.ToList())
            {
                var newRow = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == classificationType.OID) ??
                             new GhsClassificationType()
                             {
                                 OldHrsId = classificationType.OID
                             };
                newRow.Description = classificationType.Description;
                newRow.ImageName = classificationType.ImageName;
                newRow.Name = classificationType.Name;
                rep.Upsert(newRow);
            }

            return rep.AsQueryable().ToList();
        }
    }
}
