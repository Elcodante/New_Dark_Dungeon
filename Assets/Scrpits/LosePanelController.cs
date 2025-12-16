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
        LevelManager.OnLose += ShowLosePanel;
    }
    void OnDisable()
    {
        LevelManager.OnLose -= ShowLosePanel;
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
