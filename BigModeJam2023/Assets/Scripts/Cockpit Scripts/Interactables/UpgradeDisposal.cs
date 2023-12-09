using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpgradeDisposal : Interactable
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private Transform _doorHinge;
    [SerializeField] private AudioSource _audioSource;

    // ====================== Override Functions ======================
    public override void OnPlayerInteact(CockpitController.Tool heldItem, CockpitController cockpitController)
    {
        if (heldItem == _requiredTool || heldItem == CockpitController.Tool.Upgrade)
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
        _doorHinge.DOKill();

        cockpitController.SetdownTool();

        _audioSource.Play();

        _doorHinge.localRotation = Quaternion.Euler(20, 0, 0);
        _doorHinge.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.OutBounce);
    }
}
