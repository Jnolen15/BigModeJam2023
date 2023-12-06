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
    [SerializeField] private GameObject _extinguisher;
    [SerializeField] private GameObject _upgrade;
    [SerializeField] private GameObject _deadUpgrade;
    [SerializeField] private Tool _heldItem;

    private bool _cockpitControls = true;
    private float _verticalRotation = 0;
    private float _horizontalRotation = 0;

    private Camera cam;

    public enum Tool
    {
        None,
        Blowtorch,
        Extinguisher,
        Upgrade,
        DeadUpgrade
    }

    public delegate void CockpitEvent();
    public static event CockpitEvent OnGoToGame;
    public static event CockpitEvent OnGoToCockpit;

    // ====================== Setup ======================
    void Start()
    {
        cam = this.GetComponent<Camera>();

        ShipScreen.OnInteractWithScreen += ChangePerspective;
        PlayerShipController.OnGameOver += UnlockCursor;

        LockCursor();
    }

    private void OnDestroy()
    {
        ShipScreen.OnInteractWithScreen -= ChangePerspective;
        PlayerShipController.OnGameOver -= UnlockCursor;
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

    private void SetMouseLook(Vector2 pos)
    {
        _verticalRotation = pos.y;
        _horizontalRotation = pos.x;

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
            {
                Debug.Log("Clicked on " + interactable.gameObject.name);
                interactable.OnPlayerInteact(_heldItem, this);
            }
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

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetCockpitControls(false);
    }

    private void SetCockpitControls(bool canLook)
    {
        _cockpitControls = canLook;
    }

    private void TransitionToGame()
    {
        OnGoToGame?.Invoke();

        SetCockpitControls(false);

        transform.DOMove(_screenCamPos.position, 0.2f).SetEase(Ease.OutSine);
        transform.DORotate(_screenCamPos.rotation.eulerAngles, 0.2f).SetEase(Ease.OutSine);
    }

    private void TransitionToCockpit()
    {
        OnGoToCockpit?.Invoke();

        SetMouseLook(Vector2.zero);

        transform.DOMove(_playerCamPos.position, 0.2f).SetEase(Ease.OutSine);
        transform.DORotate(_playerCamPos.rotation.eulerAngles, 0.2f).SetEase(Ease.OutSine).OnComplete( () => SetCockpitControls(true) );
    }

    public void PickupTool(Tool toolName)
    {
        switch (toolName)
        {
            case Tool.None:
                _heldItem = Tool.None;
                break;
            case Tool.Blowtorch:
                _blowtorch.SetActive(true);
                _heldItem = Tool.Blowtorch;
                break;
            case Tool.Extinguisher:
                _extinguisher.SetActive(true);
                _heldItem = Tool.Extinguisher;
                break;
            case Tool.Upgrade:
                _upgrade.SetActive(true);
                _heldItem = Tool.Upgrade;
                break;
            case Tool.DeadUpgrade:
                _deadUpgrade.SetActive(true);
                _heldItem = Tool.DeadUpgrade;
                break;
            default:
                Debug.LogWarning("Pickup tool did not recognze tool name. " + toolName);
                _heldItem = Tool.None;
                break;
        }
    }

    public void SetdownTool()
    {
        _heldItem = Tool.None;

        _blowtorch.SetActive(false);
        _extinguisher.SetActive(false);
        _upgrade.SetActive(false);
        _deadUpgrade.SetActive(false);
    }
}
