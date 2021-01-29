﻿using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class KeyValues: BaseDocument
    {
        public int QuoteIdNext = 4999;

        public static int GetNextQuoteId()
        {
            var rep = new RepositoryBase<KeyValues>();
            var row = rep.AsQueryable().FirstOrDefault();
            if (row == null)
            {
                row = new KeyValues();
            }
            row.QuoteIdNext = row.QuoteIdNext + 1;
            row.SaveToDatabase();
            return row.QuoteIdNext;
        }
    }
}