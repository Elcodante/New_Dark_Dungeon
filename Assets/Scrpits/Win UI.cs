using UnityEngine;
using UnityEngine.SceneManagement;
public class WinUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        winPanel.SetActive(false);
    }
    private void OnEnable()
    {
        LevelManager.OnWin += ShowWinPanel;
    }
    private void OnDisable()
    {
        LevelManager.OnWin -= ShowWinPanel;
    }
    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }
    public void OnNextLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
