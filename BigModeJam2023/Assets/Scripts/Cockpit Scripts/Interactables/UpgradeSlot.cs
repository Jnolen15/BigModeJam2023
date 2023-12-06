using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _upgradePref;
    [SerializeField] private Transform _upgradePos;
    [SerializeField] private string _upgradeName;
    [SerializeField] private bool _drains;
    [SerializeField] private float _drainRate;
    private float _upgradeCharge;
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
        if (Input.GetKeyDown(KeyCode.V) && !_drains)
            ReduceCharge(20);

        if (!_upgradeActive || !_drains)
            return;

        if (_upgradeCharge > 0)
        {
            _upgradeCharge -= Time.deltaTime * _drainRate;
            _upgradeBattery.UpdateCharge(_upgradeCharge);
        }
        else
            TestForDead();
    }

    public void ReduceCharge(int ammount)
    {
        if (!_upgradeActive)
            return;

        _upgradeCharge -= ammount;
        _upgradeBattery.UpdateCharge(_upgradeCharge);
        TestForDead();
    }

    private void TestForDead()
    {
        if (_upgradeCharge > 0)
            return;

        Debug.Log($"Battery in slot {_upgradeName} has died!");

        _upgradeActive = false;
        _upgradeBattery.Killbattery();
        OnEndUpgrade?.Invoke(_upgradeName);
    }

    public void SlotUpgrade(CockpitController cockpitController)
    {
        if (_hasUpgrade)
            return;

        _upgradeBattery = Instantiate(_upgradePref).GetComponent<UpgradeBattery>();

        //upgrade.transform.SetParent(transform);
        _upgradeBattery.transform.position = _upgradePos.position;
        _upgradeBattery.transform.rotation = _upgradePos.rotation;

        cockpitController.SetdownTool();

        OnStartUpgrade?.Invoke(_upgradeName);
        _upgradeActive = true;
        _hasUpgrade = true;
        _upgradeCharge = 100;
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
