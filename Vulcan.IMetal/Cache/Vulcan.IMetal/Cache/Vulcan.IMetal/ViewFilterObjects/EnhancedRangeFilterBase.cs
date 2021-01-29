using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Vulcan.IMetal.ViewModelObjects;

namespace Vulcan.IMetal.ViewFilterObjects
{
    public class EnhancedRangeFilterBase<TType, TEntity>: RangeFilterBase<TType>
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

        [JsonIgnore]
        public Expression<Func<TEntity, bool>> MinExpression { get; set; }
        [JsonIgnore]
        public Expression<Func<TEntity, bool>> MaxExpression { get; set; }
        [JsonIgnore]
        public Expression<Func<TEntity, bool>> EqualToExpression { get; set; }

        public IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> entities)
        {
            if (IsActive)
            {
                if (EqualUsed)
                {
                    entities =
                        entities.Where<TEntity>(EqualToExpression);
                }
                else
                {
                    if (MinUsed)
                    {
                        entities =
                            entities.Where<TEntity>(MinExpression);
                    }
                    if (MaxUsed)
                    {
                        entities =
                            entities.Where<TEntity>(MaxExpression);
                    }
                }
            }
            return entities;

        }

        public RangeFilterViewComponent<TType, TEntity> ToViewModel()
        {
            var result = new RangeFilterViewComponent<TType, TEntity>
            {
                Name = Name,
                MinValue = MinValue,
                MinUsed = MinUsed,
                MaxValue = MaxValue,
                MaxUsed = MaxUsed,
                EqualToValue = EqualToValue,
                EqualUsed = EqualUsed
            };

            return result;

        }
    }
}