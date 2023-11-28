using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private ParticleSystem _sparks;

    // ====================== Function ======================
    public void ActiavteDamage()
    {
        Debug.Log("Activating damage " + this.name);
        _sparks.Play();
    }

    private void RepairDamage()
    {
        Debug.Log("Repairing damage " + this.name);
        _sparks.Stop();
    }

    private void OnMouseDown()
    {
        RepairDamage();
    }
}
