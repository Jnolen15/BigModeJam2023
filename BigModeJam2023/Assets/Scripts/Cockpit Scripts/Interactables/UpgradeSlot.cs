using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _upgradePref;
    [SerializeField] private Transform _upgradePos;

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if (heldItem == _requiredTool)
            SlotUpgrade(cockpitController);
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
    public void SlotUpgrade(CockpitController cockpitController)
    {
        GameObject upgrade = Instantiate(_upgradePref);

        //upgrade.transform.SetParent(transform);
        upgrade.transform.position = _upgradePos.position;
        upgrade.transform.rotation = _upgradePos.rotation;

        cockpitController.SetdownTool();
    }
}
