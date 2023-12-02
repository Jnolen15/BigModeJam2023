using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitDamageManager : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private List<Damage> _damageObjectList;

    public delegate void DamageEvent();
    public static event DamageEvent OnRepairDamage;

    // ====================== Setup ======================
    private void Awake()
    {
        PlayerShipController.OnTakeDamage += OnTakeDamage;
    }

    private void OnDestroy()
    {
        PlayerShipController.OnTakeDamage -= OnTakeDamage;
    }

    // ====================== Function ======================
    private void OnTakeDamage()
    {
        //Debug.Log("Ship taking damage!");
        int rand = Random.Range(0, _damageObjectList.Count);
        _damageObjectList[rand].ActiavteDamage();
    }

    public void OnRepair()
    {
        Debug.Log("OnRepair");
        OnRepairDamage?.Invoke();
    }
}
