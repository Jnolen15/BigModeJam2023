using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPickup : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private CockpitController.Tool _tool;
    [SerializeField] private GameObject _toolObj;
    private bool _toolHere = true;

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        // Pickup. tool should be none
        if (heldItem == _requiredTool)
        {
            PickupTool(cockpitController);
        }
        // Set down, should be the tool
        else if (heldItem == _tool)
        {
            SetdownTool(cockpitController);
        }
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
    private void PickupTool(CockpitController cockpitController)
    {
        cockpitController.PickupTool(_tool);
        _toolHere = false;
        _toolObj.SetActive(false);
    }

    private void SetdownTool(CockpitController cockpitController)
    {
        cockpitController.SetdownTool();
        _toolHere = true;
        _toolObj.SetActive(true);
    }
}
