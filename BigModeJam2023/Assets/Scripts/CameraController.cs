using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _mouseSensitivity;
    private float _verticalRotation = 0;
    private float _horizontalRotation = 0;

    // ====================== Setup ======================
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ====================== Update ======================
    void Update()
    {
        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity; // Invert Y-axis

        _verticalRotation += mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90, 90);

        _horizontalRotation += mouseX;
        _horizontalRotation = Mathf.Clamp(_horizontalRotation, -90, 90);

        //transform.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);
    }
}
