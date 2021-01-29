using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Models
{
    public class GlobalHeadcountModel
    {
        public string Name { get; set; }  //Employee Location

        public GlobalHeadcountStatusModel[] Series { get; set; }


        public GlobalHeadcountModel()
        {

        }


    }

    public class GlobalHeadcountStatusModel
    {
        public string Name { get; set; }
        public int Value { get; set; }

    }

}
