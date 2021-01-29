using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vulcan.IMetal.Models
{
    public class ItemChargeModel
    {
        public decimal Material { get; set; }
        public decimal Transport { get; set; }
        public decimal Production { get; set; }
        public decimal Miscellaneous { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Total => Material + Production + Transport + Miscellaneous + Surcharge;

        public void AddTo(ItemChargeModel model)
        {
            model.Material += Material;
            model.Transport += Transport;
            model.Production += Production;
            model.Miscellaneous += Miscellaneous;
            model.Surcharge += Surcharge;
        }

    }
}
