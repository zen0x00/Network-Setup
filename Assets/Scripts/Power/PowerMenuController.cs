using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PowerMenuController : MonoBehaviour
{
  public Button powerButton;

  public Button logoutButton;
  public Button rebootButton;
  public Button shutdownButton;

  public CanvasGroup menuCanvasGroup;
  public LinuxPowerManager powerManager;

  public float fadeDuration = 0.2f;

  private bool isOpen = false;
  private Tween currentTween;

  void Start()
  {
    menuCanvasGroup.alpha = 0f;
    menuCanvasGroup.interactable = false;
    menuCanvasGroup.blocksRaycasts = false;

    powerButton.onClick.AddListener(ToggleMenu);

    logoutButton.onClick.AddListener(() =>
    {
      powerManager.Logout();
    });

    rebootButton.onClick.AddListener(() =>
    {
      powerManager.Reboot();
    });

    shutdownButton.onClick.AddListener(() =>
    {
      powerManager.Shutdown();
    });
  }

  void ToggleMenu()
  {
    isOpen = !isOpen;

    if (currentTween != null)
      currentTween.Kill();

    if (isOpen)
    {
      menuCanvasGroup.blocksRaycasts = true;
      menuCanvasGroup.interactable = true;

      currentTween = menuCanvasGroup
          .DOFade(1f, fadeDuration)
          .SetEase(Ease.OutCubic);
    }
    else
    {
      currentTween = menuCanvasGroup
          .DOFade(0f, fadeDuration)
          .SetEase(Ease.InCubic)
          .OnComplete(() =>
          {
            menuCanvasGroup.blocksRaycasts = false;
            menuCanvasGroup.interactable = false;
          });
    }
  }
}
