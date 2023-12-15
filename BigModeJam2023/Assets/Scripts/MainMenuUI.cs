using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // ================== Refrences ==================
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _tutorialMenu;
    [SerializeField] private GameObject _intro;
    [SerializeField] private List<GameObject> _tutorialPages;
    private int _pageCount;
    
    public delegate void MainMenuEvent();
    public static event MainMenuEvent OnGameStarted;

    // ================== Setup ==================
    void Start()
    {
        CockpitController.OnGoToGame += Initial;
    }

    private void OnDestroy()
    {
        CockpitController.OnGoToGame -= Initial;
    }

    // ================== Function ==================

    public void StartGame()
    {
        Debug.Log("StartingGame");
        OnGameStarted?.Invoke();

        gameObject.SetActive(false);
    }

    public void ShowTutorial()
    {
        _mainMenu.SetActive(false);
        _tutorialMenu.SetActive(true);

        _pageCount = 0;
        ShowPage();
    }

    private void Initial()
    {
        if (!_intro.activeSelf)
            return;

        _intro.SetActive(false);
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _tutorialMenu.SetActive(false);
    }

    public void NextPage()
    {
        _pageCount++;
        ShowPage();
    }

    public void PrevPage()
    {
        _pageCount--;
        ShowPage();
    }

    private void ShowPage()
    {
        foreach (GameObject page in _tutorialPages)
        {
            page.SetActive(false);
        }

        _tutorialPages[_pageCount].SetActive(true);
    }
}
