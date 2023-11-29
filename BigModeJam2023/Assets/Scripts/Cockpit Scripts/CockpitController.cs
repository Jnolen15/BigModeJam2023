using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CockpitController : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private Transform _playerCamPos;
    [SerializeField] private Transform _screenCamPos;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private GameObject _blowtorch;
    [SerializeField] private string _heldItemName = "none";

    private bool _cockpitControls = true;
    private float _verticalRotation = 0;
    private float _horizontalRotation = 0;

    private Camera cam;

    // ====================== Setup ======================
    void Start()
    {
        cam = this.GetComponent<Camera>();

        Screen.OnInteractWithScreen += ChangePerspective;

        LockCursor();
    }

    private void OnDestroy()
    {
        Screen.OnInteractWithScreen -= ChangePerspective;
    }

    // ====================== Update ======================
    #region Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ChangePerspective();

        if (!_cockpitControls)
            return;

        MouseLook();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            MouseRaycast();
    }

    private void ChangePerspective()
    {
        if (_cockpitControls)
            TransitionToGame();
        else
            TransitionToCockpit();
    }

    private void MouseLook()
    {
        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _verticalRotation += mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90, 90);

        _horizontalRotation += mouseX;
        _horizontalRotation = Mathf.Clamp(_horizontalRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);
    }

    private void MouseRaycast()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _interactableLayer))
        {
            Transform objectHit = hit.transform;
            Interactable interactable = objectHit.gameObject.GetComponent<Interactable>();

            if (interactable != null)
                interactable.OnPlayerInteact(_heldItemName, this);
            else
                Debug.LogWarning("Clicked object does not have Interactable script " + objectHit.gameObject, objectHit.gameObject);
        }
    }
    #endregion

    // ====================== Function ======================
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetCockpitControls(bool canLook)
    {
        _cockpitControls = canLook;
    }

    private void TransitionToGame()
    {
        SetCockpitControls(false);

        transform.DOMove(_screenCamPos.position, 0.5f).SetEase(Ease.OutSine);
        transform.DORotate(_screenCamPos.rotation.eulerAngles, 0.5f).SetEase(Ease.OutSine);
    }

    private void TransitionToCockpit()
    {
        transform.DOMove(_playerCamPos.position, 0.5f).SetEase(Ease.OutSine);
        transform.DORotate(_playerCamPos.rotation.eulerAngles, 0.5f).SetEase(Ease.OutSine).OnComplete( () => SetCockpitControls(true) );
    }

    public void PickupTool(string toolName)
    {
        switch (toolName)
        {
            case "blowtorch":
                _blowtorch.SetActive(true);
                _heldItemName = "blowtorch";
                break;
            default:
                Debug.LogWarning("Pickup tool did not recognze tool name. " + toolName);
                _heldItemName = "none";
                break;
        }
    }

    public void SetdownTool()
    {
        _heldItemName = "none";

        _blowtorch.SetActive(false);
    }
}
