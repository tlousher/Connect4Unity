using UnityEngine;
using UnityEngine.Serialization;

public class Connect4Cell : MonoBehaviour
{
    internal Connect4Grid Connect4Grid;

    private void OnMouseDown()
    {
        Connect4Grid.MarkCell(gameObject);
    }
}