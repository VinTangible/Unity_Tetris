using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton GameManager instance
    public static GameManager instance = null;

    // Public variables
public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Vector2 spawnPos = new Vector2(gridWidth/2, gridHeight);

    // Private variables
    private GameObject[] tetrominos;    // Array to store all tetromino prefabs
    private Transform pieces;           // Used to hold all the grid pieces
    private Transform[,] grid = new Transform[gridWidth, gridHeight];

    void Awake()
    {
        // If singleton does not exist yet, set it
        if (instance == null) {
            instance = this;
        }
        // Otherwise destroy this gameObject being created
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load prefabs from Resources directory
        tetrominos = Resources.LoadAll<GameObject>("Prefabs/Tetrominos");
        pieces = new GameObject("Pieces").transform;

        // Spawn the first tetromino
        SpawnNextTetromino();
    }

    // Spawns the next tetromino
    public void SpawnNextTetromino()
    {
        // Get a random tetromino
        GameObject tetromino = tetrominos[Random.Range(0, tetrominos.Length)];

        // Spawn the tetromino at spawn position with it's original rotation
        GameObject toSpawn = (GameObject)Instantiate(tetromino, spawnPos, Quaternion.identity);
        toSpawn.transform.SetParent(pieces.transform);
    }

    // Update grid whenever a tetromino moves
    public void UpdateGrid(Tetromino tetromino)
    {
        // Set the previous grid positions of the tetromino's minos to null
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // Look for minos in the grid
                if (grid[x, y] != null)
                {
                    // Check if the mino's parent is the tetromino to update
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        // Add each mino piece's current position to the grid
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);

            // Only add mino to grid if it's inside the grid
            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    // Returns the position with rounded coordinates
    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // Returns the transform of the mino that exists at position, otherwise null
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y >= gridHeight)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    // Checks if a row is full
    public bool CheckIsRowFullAt(int row)
    {
        for (int col = 0; col < gridWidth; col++)
        {
            if (grid[col, row] == null)
            {
                return false;
            }
        }
        return true;
    }

    // Deletes the desired row
    public void DeleteRow(int row)
    {
        for (int col = 0; col < gridWidth; col++)
        {
            // Destroy the mino
            Destroy(grid[col, row].gameObject);

            // Set pos at grid to null
            grid[col, row] = null;
        }
    }

    public void MoveRowDown(int row)
    {
        for (int col = 0; col < gridWidth; col++)
        {
            if (grid[col, row] != null)
            {
                grid[col, row].position += new Vector3(0, -1, 0);
                grid[col, row - 1] = grid[col, row];
                grid[col, row] = null;
            }
        }
    }

    public void MoveAllAboveRowsDown(int startingRow)
    {
        for (int row = startingRow + 1; row < gridHeight; row++)
        {
            MoveRowDown(row);
        }
    }

    public void RemoveFullRows()
    {
        for (int row = 0; row < gridHeight; row++)
        {
            // If the row is full delete it
            if (CheckIsRowFullAt(row))
            {
                DeleteRow(row);
                MoveAllAboveRowsDown(row);

                // After deleting the row, next row moved down, so decrement row counter
                row--;
            }
        }
    }

    // Returns true if the position is inside grid, otherwise returns false
    public bool CheckIsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    // Check if the tetromino is above the grid
    public bool CheckIsAboveGrid(Tetromino tetromino)
    {
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y >= gridHeight)
            {
                return true;
            }
        }

        return false;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
