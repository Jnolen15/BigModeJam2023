using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // =================== Variables / Refrences ===================
    [SerializeField] private protected CockpitController.Tool _requiredTool;

    // =================== Base Functions ===================
    /*private void OnMouseDown()
    {
        OnPlayerInteact();
    }*/

    private void OnMouseEnter()
    {
        OnPlayerLookAt();
    }

    private void OnMouseExit()
    {
        OnPlayerLookAway();
    }

    // =================== Virtual Functions ===================

    // =================== Abstract Functions ===================
    public abstract void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController);

    public abstract void OnPlayerLookAt();

    public abstract void OnPlayerLookAway();
}
