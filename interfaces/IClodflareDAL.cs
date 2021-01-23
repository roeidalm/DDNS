using System.Collections.Generic;

public interface IClodflareDAL {
    void createNewARecordes (List<string> dnsNameToCreate);
    void updateARecordes (Dictionary<string, string> dnsIDToUpdate);
    void deleteARecordes (List<string> dnsIDToUpdate);
    Dictionary<string, string> getDnsList ();
    void refreshDnsList ();
    string GetIP ();
}