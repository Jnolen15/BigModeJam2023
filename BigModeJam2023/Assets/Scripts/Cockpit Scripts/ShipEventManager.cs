using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEventManager : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _testDamageTimerMax;
    private float _testDamageTimer;

    public delegate void ShipEvent();
    public static event ShipEvent OnTakeDamage;
    public static event ShipEvent OnGetUpgrade;

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
            OnGetUpgrade?.Invoke();
            _testDamageTimer = _testDamageTimerMax;
        }
    }

    // ====================== Function ======================
}
