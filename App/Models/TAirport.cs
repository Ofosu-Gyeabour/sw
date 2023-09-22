using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class TAirport
    {
        public int Id { get; set; }
        /// <summary>
        /// name of airport
        /// </summary>
        public string? Airport { get; set; }
        /// <summary>
        /// country Id of airport
        /// </summary>
        public int? CountryId { get; set; }
        /// <summary>
        /// mnemonic or short name of airport
        /// </summary>
        public string? Mnemonic { get; set; }
    }
}
