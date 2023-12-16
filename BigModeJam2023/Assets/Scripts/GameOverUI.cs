using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _pauseUI;

    [SerializeField] private bool _gamePause = false;

    [SerializeField] private GameObject _bronzeMedal;
    [SerializeField] private GameObject _silverMedal;
    [SerializeField] private GameObject _goldMedal;
    [SerializeField] private GameObject _coboltMedal;
    [SerializeField] private GameObject _unobtaniumMedal;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _beepSound1;
    [SerializeField] private AudioClip _beepSound2;

    public GameObject Director;
    public PlayableDirector PlayableDirector;

    public delegate void UIEvent();
    public static event UIEvent OnPause;
    public static event UIEvent OffPause;
    public static event UIEvent GameOverStarting;

    // ====================== Setup ======================
    private void Awake()
    {
        PlayerShipController.OnGameOver += GameOverShow;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        PlayerShipController.OnGameOver -= GameOverShow;
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

        if (Director.GetComponent<PlayableDirector>().state != PlayState.Playing && Director.activeInHierarchy)
        {
                GameOverIsOver();

            
        }
        
    }

    // ====================== Function ======================
    private void GameOverShow()
    {
        
        GameOverStarting?.Invoke();
        
        
    }

    private void GameOverIsOver()
    {
        
        _gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }



    private void Hide()
    {
        _gameOverUI.SetActive(false);
    }

    public void ShowMedals()
    {
        if (PlayerPrefs.GetInt("Bronze") == 1)
            _bronzeMedal.SetActive(true);
        if (PlayerPrefs.GetInt("Silver") == 1)
            _silverMedal.SetActive(true);
        if (PlayerPrefs.GetInt("Gold") == 1)
            _goldMedal.SetActive(true);
        if (PlayerPrefs.GetInt("Cobolt") == 1)
            _coboltMedal.SetActive(true);
        if (PlayerPrefs.GetInt("Unobtanium") == 1)
            _unobtaniumMedal.SetActive(true);
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

    public void PlayRandBeep()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
            _audioSource.PlayOneShot(_beepSound1);
        else if (rand == 1)
            _audioSource.PlayOneShot(_beepSound2);
    }
}
