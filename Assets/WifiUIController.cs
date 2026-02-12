using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class WifiUIController : MonoBehaviour
{
    public LinuxWifiScanner scanner;
    public LinuxWifiConnector connector;

    public Transform listParent;
    public GameObject buttonPrefab;
    public TMP_InputField passwordField;
    public TextMeshProUGUI statusText;

    string selectedSSID;

    public void Scan()
    {
        foreach (Transform t in listParent)
            Destroy(t.gameObject);

        string json = scanner.Scan();
        var data = JsonUtility.FromJson<WifiData>(json);

        foreach (var net in data.networks)
        {
            if (string.IsNullOrEmpty(net.ssid)) continue;

            GameObject btn = Instantiate(buttonPrefab, listParent);
            btn.GetComponentInChildren<TMP_Text>().text =
                $"{net.ssid} ({net.signal}%)";

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectedSSID = net.ssid;
                statusText.text = "Selected: " + selectedSSID;
            });
        }
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(selectedSSID))
        {
            statusText.text = "Select a network";
            return;
        }

        statusText.text = "Connecting...";
        bool success = connector.Connect(selectedSSID, passwordField.text);

        statusText.text = success ? "Connected" : "Failed";
        passwordField.text = "";
    }
}

[System.Serializable]
public class WifiData
{
    public List<WifiNetwork> networks;
}

[System.Serializable]
public class WifiNetwork
{
    public string ssid;
    public int signal;
    public string security;
    public bool connected;
}
