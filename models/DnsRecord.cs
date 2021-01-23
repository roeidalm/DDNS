namespace DDNS.models
{
    public class DnsRecord
    {
        public string ID { get; set; }

        public string IPAddr { get; set; }

        public DnsRecord(string id, string ipAddr)
        {
            ID = id;
            IPAddr = ipAddr;
        }


    }
}