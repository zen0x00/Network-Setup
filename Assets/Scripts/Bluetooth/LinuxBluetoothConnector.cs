using UnityEngine;
using System.Collections;

public class LinuxBluetoothConnector : MonoBehaviour
{
    [SerializeField] private string scriptPath = "/usr/local/bin/bluetooth_connect.sh";

    public IEnumerator Connect(string mac, System.Action<string> callback)
    {
        yield return null;

        string result = LinuxProcessBridge.Run(scriptPath, mac);
        callback?.Invoke(result);
    }
}
