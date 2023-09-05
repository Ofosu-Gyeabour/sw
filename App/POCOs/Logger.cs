#nullable disable

namespace App.POCOs
{
    public class Logger
    {
        public int id { get; set; } = 0;
        public int? eventId { get; set; } = 1;
        public string? actor { get; set; } = string.Empty;
        public string? entity { get; set; } = string.Empty;
        public string? entityValue { get; set; } = string.Empty;
        public int? companyId { get; set; } = 1;
        public DateTime logDate { get; set; }
    }
}
