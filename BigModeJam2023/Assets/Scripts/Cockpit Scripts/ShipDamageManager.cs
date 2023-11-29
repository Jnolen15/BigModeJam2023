using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDamageManager : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private List<Damage> _damageObjectList;

    // ====================== Setup ======================
    private void Awake()
    {
        ShipEventManager.OnTakeDamage += OnTakeDamage;
    }

    private void OnDestroy()
    {
        ShipEventManager.OnTakeDamage -= OnTakeDamage;
    }

    // ====================== Function ======================
    private void OnTakeDamage()
    {
        //Debug.Log("Ship taking damage!");
        int rand = Random.Range(0, _damageObjectList.Count);
        _damageObjectList[rand].ActiavteDamage();
    }
}
