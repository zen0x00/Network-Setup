using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BluetoothUIController : MonoBehaviour
{
  public LinuxBluetoothScanner scanner;
  public LinuxBluetoothConnector connector;

  public Button scanButton;
  public Transform listParent;
  public GameObject buttonPrefab;
  public TextMeshProUGUI statusText;

  void Start()
  {
    scanButton.onClick.AddListener(() =>
    {
      statusText.text = "Scanning...";
      ClearList();
      StartCoroutine(scanner.Scan(OnScanComplete));
    });
  }

  void OnScanComplete(List<BluetoothDevice> devices)
  {
    if (devices.Count == 0)
    {
      statusText.text = "No Devices Found";
      return;
    }

    statusText.text = "Select Device";

    foreach (BluetoothDevice device in devices)
    {
      GameObject buttonObj = Instantiate(buttonPrefab, listParent);
      buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = device.Name;

      Button btn = buttonObj.GetComponent<Button>();
      btn.onClick.AddListener(() =>
      {
        statusText.text = "Connecting...";
        StartCoroutine(connector.Connect(device.Mac, OnConnectComplete));
      });
    }
  }

  private string RemoveAnsi(string input)
  {
    return System.Text.RegularExpressions.Regex
        .Replace(input, @"\x1B\[[0-9;]*[mK]", "");
  }

  void OnConnectComplete(string result)
  {
    result = RemoveAnsi(result);
    statusText.text = result;
  }

  void ClearList()
  {
    foreach (Transform child in listParent)
      Destroy(child.gameObject);
  }
}
