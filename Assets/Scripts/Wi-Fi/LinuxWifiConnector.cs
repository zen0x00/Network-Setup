using UnityEngine;
using System.Collections;

public class LinuxWifiConnector : MonoBehaviour
{
  [SerializeField] private string scriptPath = "/usr/local/bin/wifi_connect.sh";

  public IEnumerator Connect(string ssid, string password, System.Action<string> callback)
  {
    yield return null;

    string arguments = $"\"{ssid}\" \"{password}\"";
    string result = LinuxProcessBridge.Run(scriptPath, arguments).Trim();

    callback?.Invoke(result);
  }
}
