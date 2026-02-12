using UnityEngine;

public class LinuxPowerManager : MonoBehaviour
{
  [SerializeField] private string shutdownPath = "/usr/local/bin/power_shutdown.sh";
  [SerializeField] private string rebootPath = "/usr/local/bin/power_reboot.sh";
  [SerializeField] private string logoutPath = "/usr/local/bin/power_logout.sh";

  public void Shutdown()
  {
    LinuxProcessBridge.Run(shutdownPath);
  }

  public void Reboot()
  {
    LinuxProcessBridge.Run(rebootPath);
  }

  public void Logout()
  {
    LinuxProcessBridge.Run(logoutPath);
  }
}
