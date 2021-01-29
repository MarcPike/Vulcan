using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Base.Solid
{
    /** One or None value type instead of nullable values or TryGet **/
    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _values;

        public Maybe()
        {
            this._values = new T[0];
        }

        public Maybe(T value)
        {
            if (value == null)
            {
                throw new ArgumentException("Null Maybe value is not allowed.",nameof(value));
            }

            this._values = new[] {value};
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
