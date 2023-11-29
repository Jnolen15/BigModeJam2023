using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : Interactable
{
    // ====================== Refrences / Variables ======================
    public delegate void ScreenEvent();
    public static event ScreenEvent OnInteractWithScreen;

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if (heldItem == _requiredTool)
            OnInteractWithScreen?.Invoke();
    }

    public override void OnPlayerLookAt()
    {
        //Debug.Log("Looking at " + gameObject.name);
    }

    public override void OnPlayerLookAway()
    {
        //Debug.Log("Looking away " + gameObject.name);
    }
}
