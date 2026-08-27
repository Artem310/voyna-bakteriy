using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefeatOverlay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject overlayRoot;
    [SerializeField] private Image dimmer;
    [SerializeField] private Image cardBackground;
    
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    
    [Header("Button Labels")]
    [SerializeField] private TextMeshProUGUI restartLabel;
    [SerializeField] private TextMeshProUGUI menuLabel;

    private void Awake()
    {
        overlayRoot.SetActive(false);
    }

    public void Show()
    {
        overlayRoot.SetActive(true);
    }

    public void Hide()
    {
        overlayRoot.SetActive(false);
    }

    public void OnRestartClicked()
    {
        Hide();
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.RestartBattle();
        }
    }

    public void OnMenuClicked()
    {
        Hide();
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.LoadMainMenu();
        }
    }
}
