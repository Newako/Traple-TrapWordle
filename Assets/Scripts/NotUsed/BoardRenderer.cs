using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    public static BoardRenderer Instance { get; private set; }

    public Row[] Rows { get; private set; }
    public Tile.State[] States; // Assign in Inspector: [0] Empty, [1] Occupied, [2] Correct, etc.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Rows = GetComponentsInChildren<Row>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearBoard()
    {
        foreach (Row row in Rows)
        {
            foreach (Tile tile in row.tiles)
            {
                tile.SetLetter('\0');
                SetTileState(tile, States[0]); // Empty state
                tile.SetTextColor(Color.white);
            }
        }
    }

    public void SetTileState(Tile tile, Tile.State state)
    {
        tile.SetState(state);
    }
}