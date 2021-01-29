using System;
using System.Collections.Generic;
using BI.DAL.Mongo.BiQueries;
using BI.DAL.Mongo.BiUserObjects;
using DAL.Common.DocClass;

namespace BI.DAL.Mongo.Models
{
    public class BiQueryBaseModel
    {
        public string Id { get; set; }
        public string QueryType { get; set; }
        public string Name { get; set; }
        public BiUserRef User { get; set; }
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();
        public List<string> CoidList { get; set; } = new List<string>();
        public List<string> Warehouses { get; set; } = new List<string>();
        public List<string> MetalTypes { get; set; } = new List<string>();
        public List<string> ProductCodes { get; set; } = new List<string>();

        public DateTime MinDate { get; set; } = DateTime.MinValue;
        public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        public Dictionary<string, string> AdditionalStringValues { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, int> AdditionalIntegerValues { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, DateTime> AdditionalDateValues { get; set; } = new Dictionary<string, DateTime>();

        public BiQueryBaseModel()
        {
            
        }

        public BiQueryBaseModel(BiQueryBase q)
        {
            Id = q.Id.ToString();
            QueryType = q.QueryType;
            Name = q.Name;
            User = q.User;
            Locations = q.Locations;
            CoidList = q.CoidList;
            Warehouses = q.Warehouses;
            MinDate = q.MinDate;
            MaxDate = q.MaxDate;
            AdditionalStringValues = q.AdditionalStringValues;
            AdditionalIntegerValues = q.AdditionalIntegerValues;
            AdditionalDateValues = q.AdditionalDateValues;
        }
    }
}