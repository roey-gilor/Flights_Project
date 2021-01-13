using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    class Country : IPoco
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Country()
        {

        }

        public Country(long id, string name)
        {
            Id = id;
            Name = name;
        }
        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override bool Equals(object obj)
        {
            return this == obj as Country;
        }
        public static bool operator ==(Country c1,Country c2)
        {
            if (c1 is null && c2 is null)
                return true;
            if (c1 is null || c2 is null)
                return false;

            return c1.Id == c2.Id;
        }
        public static bool operator !=(Country c1, Country c2)
        {
            return !(c1 == c2);
        }
    }
}
