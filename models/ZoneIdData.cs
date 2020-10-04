using System;
using System.Collections.Generic;

namespace DDNS.models {
    public class ZoneIdMeta {
        public int step { get; set; }
        public bool wildcard_proxiable { get; set; }
        public int custom_certificate_quota { get; set; }
        public int page_rule_quota { get; set; }
        public bool phishing_detected { get; set; }
        public bool multiple_railguns_allowed { get; set; }
    }

    public class ZoneIdOwner {
        public string id { get; set; }
        public string type { get; set; }
        public string email { get; set; }
    }

    public class ZoneIdAccount {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ZoneIdPlan {
        public string id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string currency { get; set; }
        public string frequency { get; set; }
        public bool is_subscribed { get; set; }
        public bool can_subscribe { get; set; }
        public string legacy_id { get; set; }
        public bool legacy_discount { get; set; }
        public bool externally_managed { get; set; }
    }

    public class ZoneIdResult {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public bool paused { get; set; }
        public string type { get; set; }
        public int development_mode { get; set; }
        public List<string> name_servers { get; set; }
        public List<string> original_name_servers { get; set; }
        public string original_registrar { get; set; }
        public object original_dnshost { get; set; }
        public DateTime modified_on { get; set; }
        public DateTime created_on { get; set; }
        public DateTime activated_on { get; set; }
        public ZoneIdMeta meta { get; set; }
        public ZoneIdOwner owner { get; set; }
        public ZoneIdAccount account { get; set; }
        public List<string> permissions { get; set; }
        public ZoneIdPlan plan { get; set; }
    }

    public class ZoneIdResultInfo {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total_pages { get; set; }
        public int count { get; set; }
        public int total_count { get; set; }
    }

    public class ZoneIdData {
        public List<ZoneIdResult> result { get; set; }
        public ZoneIdResultInfo result_info { get; set; }
        public bool success { get; set; }
        public List<object> errors { get; set; }
        public List<object> messages { get; set; }
    }

}