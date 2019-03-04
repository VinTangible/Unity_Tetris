using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;              // Width of the grid
    public int height = 20;             // Height of the grid
    private GameObject[] tetrominos;    // Array to store all tetromino prefabs
    private GameObject gridPiece;       // Prefab used for grid borders
    private Transform gridBorder;

    // Sets up the grid borders
    public void SetUpGrid()
    {
        gridBorder = new GameObject("Grid Border").transform;
        int offSetWidth = width / 2;
        int offSetHeight = height / 2;
                
        // Set up left and right walls
        for (int row = -offSetHeight; row <= offSetHeight; row++)
        {
            // Left wall is one over to the left from offset
            GameObject left = Instantiate(gridPiece, new Vector2(-offSetWidth - 1, row), Quaternion.identity);
            left.transform.SetParent(gridBorder);

            GameObject right = Instantiate(gridPiece, new Vector2(offSetWidth, row), Quaternion.identity);
            right.transform.SetParent(gridBorder);
        }

        // Set up bottom wall
        for (int col = -offSetWidth; col < offSetWidth; col++)
        {
            // Bottom wall is one below the offset
            GameObject bottom = Instantiate(gridPiece, new Vector2(col, -offSetHeight), Quaternion.identity);
            bottom.transform.SetParent(gridBorder);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load prefabs from Resources directory
        tetrominos = Resources.LoadAll<GameObject>("Prefabs/Tetrominos");
        gridPiece = Resources.Load<GameObject>("Prefabs/GridPiece");

        SetUpGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
