using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar3D : MonoBehaviour
{
    // ====================== Refrences / Variables ======================

    private GameplayManager _gameplayManager;


    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
    }

    // ====================== Function ======================

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.y = _gameplayManager.CurrentHealth / _gameplayManager.MaximumHealth;
        transform.localScale = scale;
    }
}
