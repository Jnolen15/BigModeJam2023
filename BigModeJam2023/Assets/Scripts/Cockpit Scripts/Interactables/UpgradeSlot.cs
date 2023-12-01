using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _upgradePref;
    [SerializeField] private Transform _upgradePos;
    [SerializeField] private float _upgradeTimerMax;
    [SerializeField] private float _upgradeTimer;
    private GameObject _upgrade;
    [SerializeField] private bool _upgradeActive;

    public delegate void UpgradeEvent(string upgradeName);
    public static event UpgradeEvent OnStartUpgrade;
    public static event UpgradeEvent OnEndUpgrade;

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if (heldItem == _requiredTool)
            SlotUpgrade(cockpitController);
        else if (heldItem == CockpitController.Tool.None)
            UnslotUpgrade(cockpitController);
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
    private void Update()
    {
        if (!_upgradeActive)
            return;

        if (_upgradeTimer > 0)
            _upgradeTimer -= Time.deltaTime;
        else
        {
            _upgradeActive = false;
            OnEndUpgrade?.Invoke("FireRate");
        }
    }

    public void SlotUpgrade(CockpitController cockpitController)
    {
        if (_upgradeActive)
            return;

        _upgrade = Instantiate(_upgradePref);

        //upgrade.transform.SetParent(transform);
        _upgrade.transform.position = _upgradePos.position;
        _upgrade.transform.rotation = _upgradePos.rotation;

        cockpitController.SetdownTool();

        OnStartUpgrade?.Invoke("FireRate");
        _upgradeActive = true;
        _upgradeTimer = _upgradeTimerMax;
    }

    public void UnslotUpgrade(CockpitController cockpitController)
    {
        if(_upgradeActive)
            return;
        
        cockpitController.PickupTool(CockpitController.Tool.DeadUpgrade);

        Destroy(_upgrade);
    }
}
