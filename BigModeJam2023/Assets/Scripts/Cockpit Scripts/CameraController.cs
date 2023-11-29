using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private Transform _playerCamPos;
    [SerializeField] private Transform _screenCamPos;
    [SerializeField] private float _mouseSensitivity;

    private bool _cockpitControls = true;
    private float _verticalRotation = 0;
    private float _horizontalRotation = 0;

    // ====================== Setup ======================
    void Start()
    {
        LockCursor();

        Screen.OnInteractWithScreen += ChangePerspective;
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

        if (_cockpitControls)
            MouseLook();
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

        transform.DOMove(_screenCamPos.position, 1f);
        transform.DORotate(_screenCamPos.rotation.eulerAngles, 1f);
    }

    private void TransitionToCockpit()
    {
        transform.DOMove(_playerCamPos.position, 1f);
        transform.DORotate(_playerCamPos.rotation.eulerAngles, 1f).OnComplete( () => SetCockpitControls(true) );
    }
}
