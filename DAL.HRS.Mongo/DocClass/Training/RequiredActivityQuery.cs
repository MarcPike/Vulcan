using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class RequiredActivityQuery
    {
        private MongoRawQueryHelper<RequiredActivity> _queryHelper = new MongoRawQueryHelper<RequiredActivity>();
        private List<PropertyValueRef> _requiredActivityCompleteStatus = new List<PropertyValueRef>();

        public RequiredActivityQuery()
        {
            InitializeCompleteStatusProperty();
        }

        private void InitializeCompleteStatusProperty()
        {
            var queryHelper = new MongoRawQueryHelper<PropertyValue>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Type == "RequiredActivityCompleteStatus");
            _requiredActivityCompleteStatus = queryHelper.Find(filter).ToList().Select(x => x.AsPropertyValueRef()).ToList();
        }

        public List<RequiredActivityModel> GetIncompletePriorTo(DateTime dueDate)
        {
            var filter = _queryHelper.FilterBuilder.Where(x =>
                x.CompletionDeadline <= dueDate && x.CompleteStatus.Code == "Incomplete");
            var project = _queryHelper.ProjectionBuilder.
                Expression(x => new RequiredActivityModel(x));
            return _queryHelper.FindWithProjection(filter, project).ToList();

        }
    }
}