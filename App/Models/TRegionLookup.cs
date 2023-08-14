using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class TRegionLookup
    {
        public TRegionLookup()
        {
            TCountryLookups = new HashSet<TCountryLookup>();
        }

        public int RegionId { get; set; }
        /// <summary>
        /// name of region
        /// </summary>
        public string? RegionName { get; set; }

        public virtual ICollection<TCountryLookup> TCountryLookups { get; set; }
    }
}
