using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private string _system;
    [SerializeField] private ParticleSystem _vfx;
    [SerializeField] private GameObject _fixed;
    [SerializeField] private GameObject _broken;
    [SerializeField] private AudioSource _loopingAudioSource;
    [SerializeField] private AudioSource _fixAudioSource;
    private CockpitDamageManager _damageManager;
    private bool _isBroken;

    public delegate void SystemEvent(string system);
    public static event SystemEvent OnSystemDamaged;
    public static event SystemEvent OnSystemRepaired;


    private void Start()
    {
        _damageManager = this.GetComponentInParent<CockpitDamageManager>();
    }

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if(heldItem == _requiredTool && _isBroken)
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
    public bool GetIsBroken()
    {
        return _isBroken;
    }

    public void ActivateDamage()
    {
        _isBroken = true;

        _vfx.Play();
        _loopingAudioSource.Play();
        if (_broken)
        {
            _fixed.SetActive(false);
            _broken.SetActive(true);
        }
        OnSystemDamaged?.Invoke(_system);
    }

    private void RepairDamage()
    {
        Debug.Log("Repairing damage " + this.name);
        _isBroken = false;

        _vfx.Stop();
        _loopingAudioSource.Pause();
        _fixAudioSource.Play();
        if (_broken)
        {
            _fixed.SetActive(true);
            _broken.SetActive(false);
        }

        _damageManager.OnRepair(_system);
        OnSystemDamaged?.Invoke(_system);
    }
}
