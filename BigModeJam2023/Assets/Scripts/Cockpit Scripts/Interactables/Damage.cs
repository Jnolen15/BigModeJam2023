using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private ParticleSystem _sparks;

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(string heldItem, CockpitController cockpitController)
    {
        if(heldItem == "blowtorch")
            RepairDamage();
    }

    public override void OnPlayerLookAt()
    {
        //Debug.Log("Looking at " + gameObject.name);
    }

    public override void OnPlayerLookAway()
    {
        //Debug.Log("Looking away " + gameObject.name);
    }

    // ====================== Function ======================
    public void ActiavteDamage()
    {
        //Debug.Log("Activating damage " + this.name);
        _sparks.Play();
    }

    private void RepairDamage()
    {
        Debug.Log("Repairing damage " + this.name);
        _sparks.Stop();
    }
}
