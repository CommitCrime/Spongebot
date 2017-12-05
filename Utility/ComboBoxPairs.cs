using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Utility
{
    public class ComboBoxPairs
    {
        public string _Key { get; set; }
        public int _Value { get; set; }

        public ComboBoxPairs(string _key, int _value)
        {
            _Key = _key;
            _Value = _value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(typeof(ComboBoxPairs)))
                return false;

            ComboBoxPairs other = (ComboBoxPairs)obj;
            return this._Key == other._Key && this._Value == other._Value;
        }
    }
}
