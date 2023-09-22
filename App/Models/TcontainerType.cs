using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class TcontainerType
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// container type
        /// </summary>
        public string? Ctype { get; set; }
        /// <summary>
        /// container volume
        /// </summary>
        public decimal? Cvolume { get; set; }
    }
}
