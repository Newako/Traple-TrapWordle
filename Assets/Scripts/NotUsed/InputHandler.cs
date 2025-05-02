using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    private readonly KeyCode[] supportedKeys = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
        KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U,
        KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
    };

    public int rowIndex;
    public int columnIndex;

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

    private void Update()
    {
        if (!BoardRenderer.Instance || rowIndex >= BoardRenderer.Instance.Rows.Length)
            return;

        Row currentRow = BoardRenderer.Instance.Rows[rowIndex];

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleBackspace(currentRow);
        }
        else if (columnIndex >= currentRow.tiles.Length && Input.GetKeyDown(KeyCode.Return))
        {
            WordValidator.Instance.SubmitRow(currentRow);
        }
        else
        {
            HandleLetterInput(currentRow);
        }
    }

    private void HandleBackspace(Row row)
    {
        columnIndex = Mathf.Max(columnIndex - 1, 0);
        row.tiles[columnIndex].SetLetter('\0');
        BoardRenderer.Instance.SetTileState(row.tiles[columnIndex], BoardRenderer.Instance.States[0]); // Empty state
    }

    private void HandleLetterInput(Row row)
    {
        foreach (KeyCode key in supportedKeys)
        {
            if (Input.GetKeyDown(key) && columnIndex < row.tiles.Length)
            {
                char letter = key.ToString()[0];
                row.tiles[columnIndex].SetLetter(letter);
                BoardRenderer.Instance.SetTileState(row.tiles[columnIndex], BoardRenderer.Instance.States[1]); // Occupied state
                columnIndex++;
                break;
            }
        }
    }

    public void ResetInput()
    {
        rowIndex = 0;
        columnIndex = 0;
    }
}