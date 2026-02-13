using UnityEngine;

public static class CameraDetector
{
  public static WebCamDevice[] GetAvailableDevices()
  {
    return WebCamTexture.devices;
  }
}
