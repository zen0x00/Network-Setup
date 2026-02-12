using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinuxBluetoothScanner : MonoBehaviour
{
    [SerializeField] private string scriptPath = "/usr/local/bin/bluetooth_scan.sh";

    public IEnumerator Scan(System.Action<List<BluetoothDevice>> callback)
    {
        yield return null;

        string result = LinuxProcessBridge.Run(scriptPath);
        List<BluetoothDevice> devices = Parse(result);

        callback?.Invoke(devices);
    }

    private List<BluetoothDevice> Parse(string raw)
    {
        List<BluetoothDevice> devices = new List<BluetoothDevice>();
        string[] lines = raw.Split('\n');

        foreach (string line in lines)
        {
            if (line.StartsWith("Device"))
            {
                string[] parts = line.Split(' ');
                if (parts.Length >= 3)
                {
                    string mac = parts[1];
                    string name = line.Substring(("Device " + mac + " ").Length);
                    devices.Add(new BluetoothDevice { Mac = mac, Name = name });
                }
            }
        }

        return devices;
    }
}

public class BluetoothDevice
{
    public string Mac;
    public string Name;
}
