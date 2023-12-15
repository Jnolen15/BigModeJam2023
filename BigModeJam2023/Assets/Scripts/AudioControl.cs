using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private AudioMixerSnapshot _gameView;
    [SerializeField] private AudioMixerSnapshot _cockpitView;
    [SerializeField] private AudioSource _menuMusicSource;
    [SerializeField] private AudioSource _gameMusicSource;

    private void Start()
    {
        CockpitController.OnGoToCockpit += TransitionCockpit;
        CockpitController.OnGoToGame += TransitionGame;
        MainMenuUI.OnGameStarted += SwapMusic;
    }

    private void OnDestroy()
    {
        CockpitController.OnGoToCockpit -= TransitionCockpit;
        CockpitController.OnGoToGame -= TransitionGame;
        MainMenuUI.OnGameStarted += SwapMusic;
    }

    // ====================== Function ======================
    public void TransitionGame()
    {
        _gameView.TransitionTo(0.2f);
    }

    public void TransitionCockpit()
    {
        _cockpitView.TransitionTo(0.2f);
    }

    private void SwapMusic()
    {
        _menuMusicSource.Stop();
        _gameMusicSource.Play();
    }
}
