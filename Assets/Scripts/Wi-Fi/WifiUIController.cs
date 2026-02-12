using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WifiUIController : MonoBehaviour
{
  public LinuxWifiScanner scanner;
  public LinuxWifiConnector connector;

  public Transform listParent;
  public GameObject buttonPrefab;
  public TMP_InputField passwordField;
  public TextMeshProUGUI statusText;

  void Start()
  {
    Scan();
  }

  public void Scan()
  {
    ClearList();
    statusText.text = "Scanning...";
    StartCoroutine(scanner.Scan(OnScanComplete));
  }

  void OnScanComplete(List<WifiNetwork> networks)
  {
    if (networks.Count == 0)
    {
      statusText.text = "No Networks Found";
      return;
    }

    statusText.text = "Select Network";

    foreach (WifiNetwork network in networks)
    {
      GameObject btnObj = Instantiate(buttonPrefab, listParent);
      btnObj.GetComponentInChildren<TextMeshProUGUI>().text = network.SSID;

      Button btn = btnObj.GetComponent<Button>();
      btn.onClick.AddListener(() =>
      {
        string password = passwordField.text;
        statusText.text = "Connecting...";
        StartCoroutine(connector.Connect(network.SSID, password, OnConnectComplete));
      });
    }
  }

  void OnConnectComplete(string result)
  {
    statusText.text = result;
  }

  void ClearList()
  {
    foreach (Transform child in listParent)
      Destroy(child.gameObject);
  }
}
