using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _upgradePref;
    [SerializeField] private Transform _upgradePos;
    [SerializeField] private string _upgradeName;
    [SerializeField] private float _upgradeDuration;
    private float _upgradeTimer;
    private UpgradeBattery _upgradeBattery;
    private bool _upgradeActive;
    private bool _hasUpgrade;

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
        {
            _upgradeTimer -= Time.deltaTime;
            _upgradeBattery.UpdateTimer(_upgradeTimer);
        }
        else
        {
            _upgradeActive = false;
            _upgradeBattery.Killbattery();
            OnEndUpgrade?.Invoke(_upgradeName);
        }
    }

    public void SlotUpgrade(CockpitController cockpitController)
    {
        if (_upgradeActive)
            return;

        _upgradeBattery = Instantiate(_upgradePref).GetComponent<UpgradeBattery>();

        //upgrade.transform.SetParent(transform);
        _upgradeBattery.transform.position = _upgradePos.position;
        _upgradeBattery.transform.rotation = _upgradePos.rotation;

        cockpitController.SetdownTool();

        OnStartUpgrade?.Invoke(_upgradeName);
        _upgradeActive = true;
        _hasUpgrade = true;
        _upgradeTimer = _upgradeDuration;
    }

    public void UnslotUpgrade(CockpitController cockpitController)
    {
        if(_upgradeActive || !_hasUpgrade)
            return;
        
        cockpitController.PickupTool(CockpitController.Tool.DeadUpgrade);

        _hasUpgrade = false;
        Destroy(_upgradeBattery.gameObject);
    }
}
