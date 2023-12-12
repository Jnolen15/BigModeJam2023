using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitDamageManager : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private List<Damage> _damageObjectList;

    public delegate void DamageEvent(string system);
    public static event DamageEvent OnSystemDamaged;
    public static event DamageEvent OnSystemRepaired;

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
        for (int i = 0; i < 10; i++)
        {
            Damage damage = _damageObjectList[Random.Range(0, _damageObjectList.Count)];

            if (!damage.GetIsBroken())
            {
                damage.ActivateDamage();
                break;
            }
        }
    }

    public void OnDamage(string system)
    {
        Debug.Log("OnDamage: " + system);
        OnSystemDamaged?.Invoke(system);
    }

    public void OnRepair(string system)
    {
        Debug.Log("OnRepair: " + system);
        OnSystemRepaired?.Invoke(system);
    }
}
