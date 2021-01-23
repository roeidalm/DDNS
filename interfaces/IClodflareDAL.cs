using System.Collections.Generic;
using System.Threading.Tasks;
using DDNS.models;

public interface IClodflareDAL
{
    void createNewARecordes(List<string> dnsNameToCreate);
    void updateARecordes(Dictionary<string, string> dnsIDToUpdate);
    void deleteARecordes(List<string> dnsIDToUpdate);
    Dictionary<string, DnsRecord> getDnsList();
    void refreshDnsList();
    string GetIP();
}