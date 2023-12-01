using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitEventManager : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _testDamageTimerMax;
    private float _testDamageTimer;

    public delegate void CockpitEvent();
    public static event CockpitEvent OnTakeDamage;

    // ====================== Setup ======================
    void Start()
    {
        //
    }

    // ====================== Update ======================
    private void Update()
    {
        if (_testDamageTimer > 0)
            _testDamageTimer -= Time.deltaTime;
        else
        {
            OnTakeDamage?.Invoke();
            _testDamageTimer = _testDamageTimerMax;
        }
    }

    // ====================== Function ======================
}
