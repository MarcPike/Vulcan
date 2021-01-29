using System;
using System.Collections.Generic;
using System.Linq;
using BI.DAL.Mongo.BiQueries;
using BI.DAL.Mongo.BiUserObjects;
using BI.DAL.Mongo.Models;
using MongoDB.Bson;

namespace BI.DAL.Mongo.Helpers
{
    public class HelperQueries : IHelperQueries
    {
        private readonly HelperUser _helperUser = new HelperUser();
        public List<BiQueryBaseModel> GetMyQueriesForType(string userId, string type)
        {
            var biUser = _helperUser.GetBiUser(userId);
            CheckIfUserFound(biUser);
            return biUser.Queries.Where(x=>x.QueryType == type).Select(x => new BiQueryBaseModel(x)).ToList();
        }

        private static void CheckIfUserFound(BiUser biUser)
        {
            if (biUser == null) throw new Exception("User not found");
        }

        public BiQueryBaseModel SaveMyQuery(BiQueryBaseModel model)
        {
            var biUser = _helperUser.GetBiUser(model.User.Id);
            CheckIfUserFound(biUser);

            var queryId = ObjectId.Parse(model.Id);
            var query = biUser.Queries.FirstOrDefault(x => x.Id == queryId) ?? new BiQueryBase() {Id = queryId};

            query.QueryType = model.QueryType;
            query.Name = model.Name;
            query.Locations = model.Locations;
            query.CoidList = model.CoidList;
            query.MinDate = model.MinDate;
            query.MaxDate = model.MaxDate;
            query.AdditionalStringValues = model.AdditionalStringValues;
            query.AdditionalDateValues = model.AdditionalDateValues;
            query.AdditionalIntegerValues = model.AdditionalIntegerValues;
            query.Warehouses = model.Warehouses;
            query.User = biUser.AsBiUserRef();

            BiQueryBase.Helper.Upsert(query);

            var originalQuery = biUser.Queries.FirstOrDefault(x => x.Id == queryId);
            if (originalQuery != null)
            {
                biUser.Queries[biUser.Queries.IndexOf(originalQuery)] = query;
            }
            else
            {
                biUser.Queries.Add(query);
            }

            BiUser.Helper.Upsert(biUser);
            return new BiQueryBaseModel(query);

        }

        public void RemoveMyQuery(string userId, string queryId)
        {
            var biUser = _helperUser.GetBiUser(userId);
            CheckIfUserFound(biUser);

            var id = ObjectId.Parse(queryId);
            var query = biUser.Queries.FirstOrDefault(x => x.Id == id);
            if (query != null)
            {
                biUser.Queries.Remove(query);
                BiUser.Helper.Upsert(biUser);
            }

        }

        public BiQueryBaseModel GetMyQuery(string userId, string queryId)
        {
            var biUser = _helperUser.GetBiUser(userId);
            CheckIfUserFound(biUser);

            var id = ObjectId.Parse(queryId);
            var query = biUser.Queries.FirstOrDefault(x => x.Id == id);
            if (query == null) throw new Exception("Query not found");

            return new BiQueryBaseModel(query);
        }
    }
}