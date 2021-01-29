using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Vulcan.IMetal.ViewModelObjects;

namespace Vulcan.IMetal.ViewFilterObjects
{
    public class EnhancedStringFilterBase<TEntity>: StringFilterBase
    {
        public string Name
        {
            get
            {
                var result = this.GetType().Name;
                var entityName = typeof(TEntity).Name;
                return result.Replace(entityName, "");
            }
        }

        public bool HasSuggestions { get; set; } = false;

        public virtual List<string> GetSuggestions(string coid)
        {
            return new List<string>();
        }

        [JsonIgnore]
        public Expression<Func<TEntity, bool>> InListExpression { get; set; }
        [JsonIgnore]
        public Expression<Func<TEntity, bool>> ContainsExpression { get; set; }
        [JsonIgnore]
        public Expression<Func<TEntity, bool>> StartsWithExpression { get; set; }
        [JsonIgnore]
        public Expression<Func<TEntity, bool>> EqualToExpression { get; set; }

        public IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> entities)
        {
            if (IsActive)
            {
                if (StringFilterType == StringFilterType.InList)
                {
                    entities = entities.Where<TEntity>(InListExpression);
                }
                if (StringFilterType == StringFilterType.Contains)
                {
                    entities = entities.Where<TEntity>(ContainsExpression);
                }
                if (StringFilterType == StringFilterType.StartsWith)
                {
                    entities = entities.Where<TEntity>(StartsWithExpression);
                }
                if (StringFilterType == StringFilterType.EqualTo)
                {
                    entities = entities.Where<TEntity>(EqualToExpression);
                }
            }
            return entities;
        }

        public StringFilterViewComponent<TEntity> ToViewModel()
        {
            var result = new StringFilterViewComponent<TEntity>
            {
                Name = Name,
                Value = Value,
                Values = Values,
                StringFilterType = StringFilterType,
                StringFilterName = StringFilterName,
                HasSuggestions = HasSuggestions
            };
            return result;
        }

    }
}