namespace DDNS.models {
    public class DnsCreate {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public string type { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public int ttl { get; set; }
        public int priority { get; set; }
        public bool proxied { get; set; }

    }
}