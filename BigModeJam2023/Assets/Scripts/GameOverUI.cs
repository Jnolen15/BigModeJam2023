using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _gameOverUI;

    // ====================== Setup ======================
    private void Awake()
    {
        PlayerShipController.OnGameOver += Show;
    }

    private void OnDestroy()
    {
        PlayerShipController.OnGameOver -= Show;
    }

    // ====================== Function ======================
    private void Show()
    {
        _gameOverUI.SetActive(true);
    }

    private void Hide()
    {
        _gameOverUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
