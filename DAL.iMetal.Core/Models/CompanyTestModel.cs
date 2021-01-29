using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.iMetal.Core.Models
{
    public class CompanyTestModel : BaseModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
