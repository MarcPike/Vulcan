using System;

namespace Mrp.Prototype.MrpClasses
{
    public class ShopWorker: MrpObject
    {
        public string Name { get; set; }
        public int Pin { get; set; }
        public string GetPinCode
        {
            get
            {
                int places = 4;
                var result = new String('0', places) + Pin;
                var startAt = result.Length - places;
                return result.Substring(startAt, places);
            }
        }
    }
}