using Newtonsoft.Json;

namespace OrganisationHierarchy.Models
{
    public class TreeDataModelView
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("parent")]
        public string Parent { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
       
    }
}
