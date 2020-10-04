using System;
using System.Collections.Generic;

namespace DDNS.models {

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Meta {
        public bool auto_added { get; set; }
        public bool managed_by_apps { get; set; }
        public bool managed_by_argo_tunnel { get; set; }
        public string source { get; set; }
    }

    public class Result {
        public string id { get; set; }
        public string zone_id { get; set; }
        public string zone_name { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public bool proxiable { get; set; }
        public bool proxied { get; set; }
        public int ttl { get; set; }
        public bool locked { get; set; }
        public Meta meta { get; set; }
        public DateTime created_on { get; set; }
        public DateTime modified_on { get; set; }
    }

    public class ResultInfo {
        public int page { get; set; }
        public int per_page { get; set; }
        public int count { get; set; }
        public int total_count { get; set; }
        public int total_pages { get; set; }
    }

    public class DnsListData {
        public List<Result> result { get; set; }
        public bool success { get; set; }
        public List<object> errors { get; set; }
        public List<object> messages { get; set; }
        public ResultInfo result_info { get; set; }
    }

}