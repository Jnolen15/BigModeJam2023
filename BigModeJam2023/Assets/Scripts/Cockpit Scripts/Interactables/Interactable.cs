using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
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
    public abstract void OnPlayerInteact(string heldItem, CockpitController cockpitController);

    public abstract void OnPlayerLookAt();

    public abstract void OnPlayerLookAway();
}
