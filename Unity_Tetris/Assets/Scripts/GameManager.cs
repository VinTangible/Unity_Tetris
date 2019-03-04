using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton GameManager instance
    public static GameManager instance = null;
    
    public GridManager gridManager;

    void Awake()
    {
        // If singleton does not exist yet, set it
        if (instance == null) {
            instance = this;

            // Don't destroy the GameManager instance on scene load
            DontDestroyOnLoad(gameObject);
        }
        // Otherwise destroy this gameObject being created
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
