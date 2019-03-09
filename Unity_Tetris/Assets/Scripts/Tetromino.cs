using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float lastFallTime = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }

    // Checks user's input
    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            // Check if movement was valid, if so, update grid
            if (CheckIsValidPosition())
            {
                GameManager.instance.UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            // Check if movement was valid, if so, update grid
            if (CheckIsValidPosition())
            {
                GameManager.instance.UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }

                // Check if movement was valid, if so, update grid
                if (CheckIsValidPosition())
                {
                    GameManager.instance.UpdateGrid(this);
                }
                else
                {
                    if (limitRotation)
                    {
                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFallTime >= fallSpeed)
        {
            transform.position += new Vector3(0, -1, 0);

            // Check if movement was valid, if so, update grid
            if (CheckIsValidPosition())
            {
                GameManager.instance.UpdateGrid(this);
            }
            else
            {
                // Revert movement and drop the tetromino piece
                transform.position += new Vector3(0, 1, 0);
                DropPiece();
            }

            // Update lastFallTime with current elapsed time
            lastFallTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // Keep going down until bottom is reached
            do
            {
                transform.position += new Vector3(0, -1, 0);
            } while (CheckIsValidPosition());

            // Revert movement and update the grid then drop the piece
            transform.position += new Vector3(0, 1, 0);
            GameManager.instance.UpdateGrid(this);

            DropPiece();
        }
    }

    // Check to make sure the tetromino is inside the grid
    bool CheckIsValidPosition()
    {
        // Check all if all mino pieces are in valid positions
        foreach (Transform mino in transform)
        {
            // Get the rounded position of the mino piece
            Vector2 pos = GameManager.instance.Round(mino.position);

            // Check if the mino is inside grid
            if (GameManager.instance.CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            // Check if there already exists a mino piece at the mino's position
            Transform transformAtGridPos = GameManager.instance.GetTransformAtGridPosition(pos);

            if (transformAtGridPos != null && transformAtGridPos.parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    // Handles when the tetromino piece has landed
    void DropPiece()
    {
        // The bottom of the grid is reached, so check for full rows
        GameManager.instance.RemoveFullRows();

        // Check if this tetromino is over the grid, indicating game over
        if (GameManager.instance.CheckIsAboveGrid(this))
        {
            GameManager.instance.GameOver();
        }

        // Disable current tetromino after landing and spawn next tetromino
        enabled = false;
        GameManager.instance.SpawnNextTetromino();
    }
}
