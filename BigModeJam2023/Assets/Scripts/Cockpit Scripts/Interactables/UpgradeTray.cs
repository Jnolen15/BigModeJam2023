using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTray : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _upgradePref;
    [SerializeField] private List<Transform> _upgradeSlots;
    [SerializeField] private List<GameObject> _upgradeList;

    // ====================== Setup ======================
    private void Awake()
    {
        CockpitEventManager.OnGetUpgrade += GetUpgrade;
    }

    private void OnDestroy()
    {
        CockpitEventManager.OnGetUpgrade -= GetUpgrade;
    }

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if (heldItem == _requiredTool)
            GrabUpgrade(cockpitController);
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
    public void GrabUpgrade(CockpitController cockpitController)
    {
        if (_upgradeList.Count <= 0)
            return;

        int index = _upgradeList.Count - 1;

        Destroy(_upgradeList[index]);
        _upgradeList.RemoveAt(index);
        cockpitController.PickupTool(CockpitController.Tool.Upgrade);
    }

    public void GetUpgrade()
    {
        if (_upgradeList.Count >= _upgradeSlots.Count)
            return;

        GameObject upgrade = Instantiate(_upgradePref);
        _upgradeList.Add(upgrade);

        upgrade.transform.SetParent(transform);
        upgrade.transform.localPosition = _upgradeSlots[_upgradeList.Count-1].localPosition;
    }
}
