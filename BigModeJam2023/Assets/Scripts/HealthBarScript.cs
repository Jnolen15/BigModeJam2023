using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================

    [SerializeField] private Image _healthGraphic;
    private GameplayManager _gameplayManager;


    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
    }

    // ====================== Function ======================

    void Update()
    {
        _healthGraphic.fillAmount = _gameplayManager.CurrentHealth / _gameplayManager.MaximumHealth;
    }
}
