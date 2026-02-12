using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinuxAudioManager : MonoBehaviour
{
  [SerializeField] private string listPath = "/usr/local/bin/audio_list.sh";
  [SerializeField] private string currentPath = "/usr/local/bin/audio_current.sh";
  [SerializeField] private string setPath = "/usr/local/bin/audio_set.sh";

  public IEnumerator GetOutputs(System.Action<List<AudioSink>> callback)
  {
    yield return null;

    string result = LinuxProcessBridge.Run(listPath);
    List<AudioSink> sinks = Parse(result);

    callback?.Invoke(sinks);
  }

  public IEnumerator GetCurrent(System.Action<string> callback)
  {
    yield return null;

    string result = LinuxProcessBridge.Run(currentPath).Trim();
    callback?.Invoke(result);
  }

  public IEnumerator SetOutput(string sink, System.Action<string> callback)
  {
    yield return null;

    string result = LinuxProcessBridge.Run(setPath, $"\"{sink}\"").Trim();
    callback?.Invoke(result);
  }

  private List<AudioSink> Parse(string raw)
  {
    List<AudioSink> sinks = new List<AudioSink>();
    string[] lines = raw.Split('\n');

    foreach (string line in lines)
    {
      if (string.IsNullOrWhiteSpace(line)) continue;

      string[] parts = line.Split('|');
      if (parts.Length >= 2)
      {
        sinks.Add(new AudioSink
        {
          Name = parts[0],
          Description = parts[1]
        });
      }
    }

    return sinks;
  }
}

public class AudioSink
{
  public string Name;
  public string Description;
}
