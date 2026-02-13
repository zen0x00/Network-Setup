using UnityEngine;

public static class CameraSettingsManager
{
  private const string DefaultCameraKey = "DefaultCamera";

  public static void SetDefaultCamera(string deviceName)
  {
    PlayerPrefs.SetString(DefaultCameraKey, deviceName);
    PlayerPrefs.Save();
  }

  public static string GetDefaultCamera()
  {
    return PlayerPrefs.GetString(DefaultCameraKey, "");
  }
}
