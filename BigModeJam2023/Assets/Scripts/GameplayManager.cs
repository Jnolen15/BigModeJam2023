using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
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
