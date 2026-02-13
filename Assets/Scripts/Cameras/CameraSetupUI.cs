using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSetupUI : MonoBehaviour
{
  public Button checkButton;
  public TMP_Text statusText;

  public GameObject selectionPanel;
  public Transform deviceListParent;
  public GameObject deviceButtonPrefab;

  private WebCamDevice[] detectedDevices;
  private bool scanComplete = false;

  void Start()
  {
    selectionPanel.SetActive(false);
    statusText.text = "Idle";
    checkButton.onClick.AddListener(OnCheckButtonClicked);
  }

  void OnCheckButtonClicked()
  {
    if (!scanComplete)
    {
      StartCoroutine(CheckForDevices());
    }
    else
    {
      OpenSelectionPanel();
    }
  }

  System.Collections.IEnumerator CheckForDevices()
  {
    statusText.text = "Checking...";
    checkButton.interactable = false;

    yield return new WaitForSeconds(0.5f);

    detectedDevices = CameraDetector.GetAvailableDevices();

    if (detectedDevices.Length > 0)
    {
      statusText.text = "Found Cameras";
      checkButton.GetComponentInChildren<TMP_Text>().text = "Continue";
      scanComplete = true;
    }
    else
    {
      statusText.text = "No Cameras Found";
    }

    checkButton.interactable = true;
  }

  void OpenSelectionPanel()
  {
    selectionPanel.SetActive(true);

    foreach (Transform child in deviceListParent)
      Destroy(child.gameObject);

    foreach (var device in detectedDevices)
    {
      GameObject btn = Instantiate(deviceButtonPrefab, deviceListParent);
      btn.GetComponentInChildren<TMP_Text>().text = device.name;

      btn.GetComponent<Button>().onClick.AddListener(() =>
      {
        CameraSettingsManager.SetDefaultCamera(device.name);
        Debug.Log("Default camera set: " + device.name);
      });
    }
  }
}
