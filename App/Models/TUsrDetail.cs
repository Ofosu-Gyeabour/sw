using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class TUsrDetail
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the user Id (foreign key)
        /// </summary>
        public int? TuserId { get; set; }
        /// <summary>
        /// office telephone
        /// </summary>
        public string? OfficeTel { get; set; }
        /// <summary>
        /// mobile telephone
        /// </summary>
        public string? MobileTel { get; set; }

        public virtual Tusr? Tuser { get; set; }
    }
}
