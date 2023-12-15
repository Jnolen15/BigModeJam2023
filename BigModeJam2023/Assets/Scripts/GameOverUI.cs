using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _pauseUI;

    [SerializeField] private bool _gamePause = false;

    public delegate void UIEvent();
    public static event UIEvent OnPause;
    public static event UIEvent OffPause;

    // ====================== Setup ======================
    private void Awake()
    {
        PlayerShipController.OnGameOver += Show;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        PlayerShipController.OnGameOver -= Show;
    }

    private void Update()
    {
        //For Pausing Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_gamePause)
                ShowPause();
            else
                HidePause();
        }
    }

    // ====================== Function ======================
    private void Show()
    {
        Time.timeScale = 0;
        _gameOverUI.SetActive(true);
    }

    private void Hide()
    {
        _gameOverUI.SetActive(false);
    }

    public void ShowPause()
    {
        OnPause?.Invoke();
        _pauseUI.SetActive(true);
        _gamePause = true;
        Time.timeScale = 0;
    }
    public void HidePause()
    {
        OffPause?.Invoke();
        _pauseUI.SetActive(false);
        _gamePause = false;
        Time.timeScale = 1;
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
