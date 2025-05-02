using System.Linq;
using UnityEngine;

public class TrapLetterManager : MonoBehaviour
{
    public static TrapLetterManager Instance { get; private set; }

    public Tile.State trapState;

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

    public bool ContainsTrapLetter(Row row)
    {
        foreach (Tile tile in row.tiles)
        {
            if (WordValidator.Instance.TrapLetters.Contains(tile.letter))
                return true;
        }
        return false;
    }

    public void ApplyTrapEffect(Row row)
    {
        foreach (Tile tile in row.tiles)
        {
            tile.SetTextColor(Color.red);
        }
    }
}