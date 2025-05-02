using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameObject newWordButton;
    public GameObject retryButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NewGame()
    {
        BoardRenderer.Instance.ClearBoard();
        WordValidator.Instance.SetRandomWord();
        InputHandler.Instance.ResetInput();
        ToggleUI(false);
    }

    public void Retry()
    {
        BoardRenderer.Instance.ClearBoard();
        InputHandler.Instance.ResetInput();
        ToggleUI(false);
    }

    public void ToggleUI(bool gameEnded)
    {
        retryButton.SetActive(gameEnded);
        newWordButton.SetActive(gameEnded);
    }

    public void HandleGameOver(bool won)
    {
        ToggleUI(true);
        Debug.Log(won ? "You won!" : "Game over!");
    }
}