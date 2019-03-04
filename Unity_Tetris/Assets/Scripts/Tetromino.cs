using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float lastFallTime = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }

    // Checks user's input
    void CheckUserInput()
    {
    }

    // Check to make sure the tetrimino is inside the grid
    bool CheckIsValidPosition()
    {
        return true;
    }
}
