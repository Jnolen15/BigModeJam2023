using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    public float CurrentHealth = 100;
    public float MaximumHealth = 100;

    public bool GamePaused = false;
    public bool GameOver = false;
    public bool FlyingTheShip = true;

    // ====================== Setup ======================
    void Start()
    {
        
    }

    // ====================== Function ======================

    void Update()
    {
        
    }

    public bool GameSuspended()
    {
        return GamePaused || GameOver;
    } 
}
