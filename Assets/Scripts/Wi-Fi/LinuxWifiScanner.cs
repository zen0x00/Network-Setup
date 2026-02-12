using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinuxWifiScanner : MonoBehaviour
{
  [SerializeField] private string scriptPath = "/usr/local/bin/wifi_scan.sh";

  public IEnumerator Scan(System.Action<List<WifiNetwork>> callback)
  {
    yield return null;

    string result = LinuxProcessBridge.Run(scriptPath);
    List<WifiNetwork> networks = Parse(result);

    callback?.Invoke(networks);
  }

  private List<WifiNetwork> Parse(string raw)
  {
    List<WifiNetwork> networks = new List<WifiNetwork>();
    string[] lines = raw.Split('\n');

    foreach (string line in lines)
    {
      if (string.IsNullOrWhiteSpace(line)) continue;

      string[] parts = line.Split(':');
      if (parts.Length >= 3)
      {
        networks.Add(new WifiNetwork
        {
          SSID = parts[0],
          Signal = parts[1],
          Security = parts[2]
        });
      }
    }

    return networks;
  }
}

public class WifiNetwork
{
  public string SSID;
  public string Signal;
  public string Security;
}
