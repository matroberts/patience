using System.Collections.Generic;
using Newtonsoft.Json;

namespace patience.core
{
    public class ApiLayout
    {
        public List<string> Stock { get; set; } = new List<string>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}