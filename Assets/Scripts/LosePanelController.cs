using UnityEngine;
using UnityEngine.SceneManagement;
public class LosePanelController : MonoBehaviour
{
    public GameObject losePanel;
    public string mainMenuSceneName = "MainMenu";

    void Awake()
    {
        losePanel.SetActive(false);
    }
    void OnEnable()
    {
        LevelManager.OnLoseCompleted += ShowLosePanel;
    }
    void OnDisable()
    {
        LevelManager.OnLoseCompleted -= ShowLosePanel;
    }
    void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
