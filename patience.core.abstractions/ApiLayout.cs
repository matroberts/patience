using System.Collections.Generic;
using Newtonsoft.Json;

namespace patience.core
{
    public class ApiLayout
    {
        public List<string> Stock { get; set; } = new List<string>();
        public List<string> Foundation { get; set; } = new List<string>();
        public Dictionary<string, List<string>> Tableau { get; set; } = new Dictionary<string, List<string>>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}