using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AudioUIController : MonoBehaviour
{
  public LinuxAudioManager audioManager;

  public Button refreshButton;
  public Transform listParent;
  public GameObject buttonPrefab;
  public TextMeshProUGUI currentDeviceText;
  public TextMeshProUGUI statusText;

  void Start()
  {
    refreshButton.onClick.AddListener(Refresh);
    Refresh();
  }

  void Refresh()
  {
    ClearList();
    statusText.text = "Loading...";

    StartCoroutine(audioManager.GetOutputs(OnOutputsLoaded));
    StartCoroutine(audioManager.GetCurrent(OnCurrentLoaded));
  }

  void OnOutputsLoaded(List<AudioSink> sinks)
  {
    foreach (AudioSink sink in sinks)
    {
      GameObject obj = Instantiate(buttonPrefab, listParent);
      obj.GetComponentInChildren<TextMeshProUGUI>().text = sink.Description;

      Button btn = obj.GetComponent<Button>();
      btn.onClick.AddListener(() =>
      {
        statusText.text = "Switching...";
        StartCoroutine(audioManager.SetOutput(sink.Name, OnSwitchComplete));
      });
    }

    statusText.text = "Select Output Device";
  }

  void OnCurrentLoaded(string current)
  {
    currentDeviceText.text = "Current: " + current;
  }

  void OnSwitchComplete(string result)
  {
    statusText.text = result;
    StartCoroutine(audioManager.GetCurrent(OnCurrentLoaded));
  }

  void ClearList()
  {
    foreach (Transform child in listParent)
      Destroy(child.gameObject);
  }
}
