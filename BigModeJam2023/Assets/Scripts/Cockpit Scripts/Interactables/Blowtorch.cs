using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowtorch : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private GameObject _blowtorch;
    private bool _toolHere = true;

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(string heldItem, CockpitController cockpitController)
    {
        if (heldItem == "none")
        {
            PickupTool(cockpitController);
        }
        else if (heldItem == "blowtorch")
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
        cockpitController.PickupTool("blowtorch");
        _toolHere = false;
        _blowtorch.SetActive(false);
    }

    private void SetdownTool(CockpitController cockpitController)
    {
        cockpitController.SetdownTool();
        _toolHere = true;
        _blowtorch.SetActive(true);
    }
}
