using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private ParticleSystem _vfx;
    [SerializeField] private GameObject _fixed;
    [SerializeField] private GameObject _broken;
    private CockpitDamageManager _damageManager;

    private void Start()
    {
        _damageManager = this.GetComponentInParent<CockpitDamageManager>();
    }

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if(heldItem == _requiredTool)
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
        _vfx.Play();

        if (_broken)
        {
            _fixed.SetActive(false);
            _broken.SetActive(true);
        }
    }

    private void RepairDamage()
    {
        Debug.Log("Repairing damage " + this.name);
        _vfx.Stop();

        if (_broken)
        {
            _fixed.SetActive(true);
            _broken.SetActive(false);
        }

        _damageManager.OnRepair();
    }
}
