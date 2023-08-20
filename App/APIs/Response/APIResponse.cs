#nullable disable

namespace App.APIs.Response
{
    public class APIResponse
    {
        public bool status { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public object data { get; set; }
    }

    

}
