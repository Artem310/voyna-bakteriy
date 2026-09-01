using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }
    
    [SerializeField] private GameConfig gameConfig;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadBattle()
    {
        if (gameConfig != null)
        {
            int completedLevel = PlayerPrefs.GetInt("CompletedLevel", 0);
            gameConfig.currentLevel = Mathf.Clamp(completedLevel + 1, 1, 6);
        }
        
        SceneManager.LoadScene("Battle");
    }
    
    public void RestartBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
