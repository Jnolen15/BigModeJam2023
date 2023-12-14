using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // ================== Refrences ==================
    public delegate void MainMenuEvent();
    public static event MainMenuEvent OnGameStarted;

    // ================== Setup ==================
    void Start()
    {
        //
    }

    // ================== Function ==================

    public void StartGame()
    {
        Debug.Log("StartingGame");
        OnGameStarted?.Invoke();

        gameObject.SetActive(false);
    }
}
