using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class TTemplate
    {
        public int TemplateId { get; set; }
        /// <summary>
        /// name of the template
        /// </summary>
        public string? TemplateName { get; set; }
        /// <summary>
        /// Id of the company
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// serialized data making up the template
        /// </summary>
        public string? TemplateDetails { get; set; }
    }
}
