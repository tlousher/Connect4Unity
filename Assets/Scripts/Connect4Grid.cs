using UnityEngine;

public class Connect4Grid : MonoBehaviour
{
    [Tooltip("Número de filas del grid")]
    public int rows = 6;
    [Tooltip("Número de columnas del grid")]
    public int columns = 7;
    [Tooltip("Número de fichas consecutivas para ganar")]
    public int targetCount = 4;
    [Tooltip("Prefab de la casilla del grid")]
    public GameObject cellPrefab;

    private GameObject[,] _grid; // Matriz bidimensional para almacenar las fichas del juego

    private int _currentPlayer = 1; // Variable para llevar registro del jugador actual


    private void Start()
    {
        CreateGrid();
    }

    // Crear el grid inicial del juego
    private void CreateGrid()
    {
        _grid = new GameObject[columns, rows];

        // Crear cada casilla del grid
        for (var col = 0; col < columns; col++)
        {
            for (var row = 0; row < rows; row++)
            {
                var gridTransform = transform;
                var gridPosition = gridTransform.position;
                var position = new Vector3(gridPosition.x + col, gridPosition.y + row, 0);
                var cell = Instantiate(cellPrefab, position, Quaternion.identity, gridTransform);
                cell.name = $"Cell {col} - {row}";
                _grid[col, row] = cell;
                
                // Agregar componente de detección de clics
                cell.AddComponent<Connect4Cell>().Connect4Grid = this;
            }
        }
    }

    /** Método para marcar una casilla con el jugador actual
     * @param cell Casilla a marcar
     */
    public void MarkCell(GameObject cell)
    {
        if (cell == null) return;
        
        var cellRenderer = cell.GetComponent<Renderer>();
        // Marcar la casilla con el jugador actual
        cellRenderer.material.color = _currentPlayer == 1 ? Color.red : Color.blue;

        if (CheckWin(cellRenderer))
        {
            Debug.Log("Player " + _currentPlayer + " wins!");
        }
        // Cambiar al siguiente jugador
        _currentPlayer = _currentPlayer == 1 ? 2 : 1;
    }
    
    private bool CheckWin(Renderer cellRenderer)
    {
        for (var col = 0; col < columns; col++)
        {
            for (var row = 0; row < rows; row++)
            {
                if (_grid[col, row].GetComponent<Renderer>().material.color != cellRenderer.material.color) continue;
                if (CheckHorizontalWin(col, row, cellRenderer) ||
                    CheckVerticalWin(col, row, cellRenderer) ||
                    CheckDiagonalWin(col, row, cellRenderer))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckHorizontalWin(int col, int row, Renderer cellRenderer)
    {
        var count = 1;

        // Check to the right
        for (var i = 1; i < targetCount; i++)
        {
            if (col + i >= columns ||_grid[col + i, row].GetComponent<Renderer>().material.color != cellRenderer.material.color)
            {
                break;
            }
            count++;
        }

        return count >= targetCount;
    }

    private bool CheckVerticalWin(int col, int row, Renderer cellRenderer)
    {
        var count = 1;

        // Check upward
        for (var i = 1; i < targetCount; i++)
        {
            if (row + i >= rows ||_grid[col, row + i].GetComponent<Renderer>().material.color != cellRenderer.material.color)
            {
                break;
            }
            count++;
        }

        return count >= targetCount;
    }

    private bool CheckDiagonalWin(int col, int row, Renderer cellRenderer)
    {
        // Check diagonal up-right
        var count = 1;
        for (var i = 1; i < targetCount; i++)
        {
            if (col + i >= columns || row + i >= rows ||_grid[col + i, row + i].GetComponent<Renderer>().material.color != cellRenderer.material.color)
            {
                break;
            }
            count++;
        }

        if (count >= targetCount)
        {
            return true;
        }

        // Check diagonal down-right
        count = 1;
        for (var i = 1; i < targetCount; i++)
        {
            if (col + i >= columns || row - i < 0 ||_grid[col + i, row - i].GetComponent<Renderer>().material.color != cellRenderer.material.color)
            {
                break;
            }
            count++;
        }

        return count >= targetCount;
    }

}