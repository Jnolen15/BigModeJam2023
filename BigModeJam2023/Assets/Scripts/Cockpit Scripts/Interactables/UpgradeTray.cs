using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTray : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _upgradePref;
    [SerializeField] private List<Transform> _upgradeSlots;
    [SerializeField] private List<GameObject> _upgradeList;

    public delegate void UpgradeTrayEvent(int num);
    public static event UpgradeTrayEvent OnUpgradeNumChanged;

    // ====================== Setup ======================
    private void Awake()
    {
        PlayerShipController.OnUpgradePickUp += GetUpgrade;
    }

    private void OnDestroy()
    {
        PlayerShipController.OnUpgradePickUp -= GetUpgrade;
    }

    // FOR TESTING
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //    GetUpgrade();
    }

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if (heldItem == _requiredTool)
            GrabUpgrade(cockpitController);
        else if (heldItem == CockpitController.Tool.Upgrade)
            PlaceUpgrade(cockpitController);
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

        OnUpgradeNumChanged?.Invoke(_upgradeList.Count);
    }

    public void PlaceUpgrade(CockpitController cockpitController)
    {
        if (_upgradeList.Count >= _upgradeSlots.Count)
            return;


        cockpitController.SetdownTool();
        GetUpgrade();
    }

    public void GetUpgrade()
    {
        if (_upgradeList.Count >= _upgradeSlots.Count)
            return;

        GameObject upgrade = Instantiate(_upgradePref);
        _upgradeList.Add(upgrade);

        upgrade.transform.SetParent(transform);
        upgrade.transform.localPosition = _upgradeSlots[_upgradeList.Count-1].localPosition;
        upgrade.transform.localRotation = _upgradeSlots[_upgradeList.Count-1].localRotation;

        OnUpgradeNumChanged?.Invoke(_upgradeList.Count);
    }
}
