using UnityEngine;
using TMPro;
public class MoveCounterUI : MonoBehaviour
{
    public TextMeshProUGUI moveCounterText;
    void OnEnable()
    {
        LevelManager.OnMovesChanged += UpdateMoveCounter;
    }
    void OnDisable()
    {
        LevelManager.OnMovesChanged -= UpdateMoveCounter;
    }
    void UpdateMoveCounter(int moves)
    {
        moveCounterText.text = moves.ToString();
    }
}
