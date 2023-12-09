using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private AudioMixerSnapshot _gameView;
    [SerializeField] private AudioMixerSnapshot _cockpitView;

    private void Start()
    {
        CockpitController.OnGoToCockpit += TransitionCockpit;
        CockpitController.OnGoToGame += TransitionGame;
    }

    private void OnDestroy()
    {
        CockpitController.OnGoToCockpit -= TransitionCockpit;
        CockpitController.OnGoToGame -= TransitionGame;
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
}
