using System;

namespace Mrp.Prototype.MrpClasses
{
    public class MrpObject
    {
        public Guid Id => Guid.NewGuid();
        public DateTime CreatedAt { get; }

        public MrpObject()
        {
            CreatedAt = DateTime.Now;
        }
    }
}