using UnityEngine;
using System.Linq;

public class WordValidator : MonoBehaviour
{
    public static WordValidator Instance { get; private set; }

    private string[] solutions;
    private string[] validWords;
    private string currentWord;
    public char[] TrapLetters => trapLetters;

    private char[] trapLetters;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("official_wordle_all") as TextAsset;
        validWords = textFile.text.Split('\n');

        textFile = Resources.Load("official_wordle_common") as TextAsset;
        solutions = textFile.text.Split('\n');
    }

    public void SetRandomWord()
    {
        currentWord = solutions[Random.Range(0, solutions.Length)].ToLower().Trim();
        trapLetters = GenerateTrapLetters(2);
        Debug.Log($"Current word: {currentWord} | Trap letters: {string.Join(", ", trapLetters)}");
    }

    private char[] GenerateTrapLetters(int count)
    {
        char[] traps = new char[count];
        for (int i = 0; i < count; i++)
        {
            char c;
            do
            {
                c = (char)('a' + Random.Range(0, 26));
            } while (currentWord.Contains(c) || traps.Contains(c));
            traps[i] = c;
        }
        return traps;
    }

    public void SubmitRow(Row row)
    {
        if (TrapLetterManager.Instance.ContainsTrapLetter(row))
        {
            TrapLetterManager.Instance.ApplyTrapEffect(row);
            AdvanceRow();
            return;
        }

        if (!IsValidWord(row.word))
        {
            Debug.Log("Invalid word!");
            return;
        }

        EvaluateRow(row);
        AdvanceRow();
    }

    private void EvaluateRow(Row row)
    {
        string remainingLetters = currentWord;

        // Check correct letters first
        for (int i = 0; i < row.tiles.Length; i++)
        {
            if (row.tiles[i].letter == currentWord[i])
            {
                BoardRenderer.Instance.SetTileState(row.tiles[i], BoardRenderer.Instance.States[2]); // Correct state
                remainingLetters = remainingLetters.Remove(i, 1).Insert(i, " ");
            }
        }

        // Check wrong spot/incorrect letters
        for (int i = 0; i < row.tiles.Length; i++)
        {
            if (row.tiles[i].state != BoardRenderer.Instance.States[2]) // Skip correct tiles
            {
                if (remainingLetters.Contains(row.tiles[i].letter))
                {
                    BoardRenderer.Instance.SetTileState(row.tiles[i], BoardRenderer.Instance.States[3]); // Wrong spot
                    int index = remainingLetters.IndexOf(row.tiles[i].letter);
                    remainingLetters = remainingLetters.Remove(index, 1).Insert(index, " ");
                }
                else
                {
                    BoardRenderer.Instance.SetTileState(row.tiles[i], BoardRenderer.Instance.States[4]); // Incorrect
                }
            }
        }

        if (row.word == currentWord)
        {
            GameManager.Instance.HandleGameOver(true);
        }
    }

    private bool IsValidWord(string word)
    {
        return validWords.Contains(word);
    }

    private void AdvanceRow()
    {
        InputHandler.Instance.rowIndex++;
        InputHandler.Instance.columnIndex = 0;

        if (InputHandler.Instance.rowIndex >= BoardRenderer.Instance.Rows.Length)
        {
            GameManager.Instance.HandleGameOver(false);
        }
    }
}