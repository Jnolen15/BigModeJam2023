using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDisposal : Interactable
{
    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if (heldItem == _requiredTool)
            Dispose(cockpitController);
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
    private void Dispose(CockpitController cockpitController)
    {
        cockpitController.SetdownTool();
    }
}
