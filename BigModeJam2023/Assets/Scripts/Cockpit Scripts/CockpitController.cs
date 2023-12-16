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
    [SerializeField] private GameObject _screenLight;
    [SerializeField] private GameObject _mouseDot;
    [SerializeField] private GameObject _gameOverCutscene;

    private bool _inCockpit = true;
    private bool _cockpitControls = true;
    private bool _gameOver = false;
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

        MainMenuUI.OnGameStarted += SetGameStart;
        ShipScreen.OnInteractWithScreen += ChangePerspective;
        PlayerShipController.OnGameOver += OnGameEnd;
        GameOverUI.OffPause += OnUnpause;
        GameOverUI.OnPause += OnGameEnd;
        GameOverUI.GameOverStarting += PrepareForCutscene;

        LockCursor();
    }

    private void OnDestroy()
    {
        MainMenuUI.OnGameStarted -= SetGameStart;
        ShipScreen.OnInteractWithScreen -= ChangePerspective;
        PlayerShipController.OnGameOver -= OnGameEnd;
        GameOverUI.OffPause -= OnUnpause;
        GameOverUI.OnPause -= OnGameEnd;
        GameOverUI.GameOverStarting -= PrepareForCutscene;
    }

    // Start function
    private bool _gameStarted;
    private void SetGameStart()
    {
        Debug.Log("CockiptController Start");

        _gameStarted = true;
        LockCursor();
        StartCoroutine("startingShake");
    }

    // ====================== Update ======================
    #region Update
    void Update()
    {
        if (_gameOver)
            return;

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
    #region Function
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnGameEnd()
    {
        UnlockCursor();
        SetCockpitControls(false);
    }

    private void OnUnpause()
    {
        LockCursor();

        if(_inCockpit)
            SetCockpitControls(true);
    }
    private void PrepareForCutscene()
    {
        _gameOver = true;
        if (!_inCockpit)
        {
            SetMouseLook(Vector2.zero);

            transform.DOMove(_playerCamPos.position, 0.2f).SetEase(Ease.OutSine);
            transform.DORotate(_playerCamPos.rotation.eulerAngles, 0.2f).SetEase(Ease.OutSine).OnComplete(() => SetCockpitControls(true));

            LockCursor();

            _screenLight.SetActive(false);
        }
        _gameOverCutscene.SetActive(true);
    }

    private void SetCockpitControls(bool canLook)
    {
        _cockpitControls = canLook;
    }

    private void TransitionToGame()
    {
        OnGoToGame?.Invoke();

        _inCockpit = false;
        SetCockpitControls(false);

        transform.DOMove(_screenCamPos.position, 0.2f).SetEase(Ease.OutSine);
        transform.DORotate(_screenCamPos.rotation.eulerAngles, 0.2f).SetEase(Ease.OutSine);

        // Make sure cursor is unlocked on game start
        if (!_gameStarted)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        _screenLight.SetActive(true);
        _mouseDot.SetActive(false);
    }

    private void TransitionToCockpit()
    {
        OnGoToCockpit?.Invoke();

        _inCockpit = true;
        SetMouseLook(Vector2.zero);

        transform.DOMove(_playerCamPos.position, 0.2f).SetEase(Ease.OutSine);
        transform.DORotate(_playerCamPos.rotation.eulerAngles, 0.2f).SetEase(Ease.OutSine).OnComplete( () => SetCockpitControls(true) );

        LockCursor();

        _screenLight.SetActive(false);
        _mouseDot.SetActive(true);
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

    IEnumerator startingShake()
    {
        float time = 3F;
        float magnitude = 0.01F;
        Transform t = Camera.main.transform;
        Vector3 lastPos = Vector3.zero;
        float TimeStamp = Time.time + time;
        while (TimeStamp > Time.time)
        {
            float x = Random.Range(-1f, 1f) * magnitude * Time.timeScale;
            float y = Random.Range(-1f, 1f) * magnitude * Time.timeScale;
            float z = Random.Range(-1f, 1f) * magnitude * Time.timeScale;
            // undo last transform, add new transform, save new transform as old
            t.position -= lastPos;
            t.position += new Vector3(x, y, z);
            lastPos = new Vector3(x, y, z);
            yield return null;
        }
        t.position -= lastPos;
    }

    public void SetdownTool()
    {
        _heldItem = Tool.None;

        _blowtorch.SetActive(false);
        _extinguisher.SetActive(false);
        _upgrade.SetActive(false);
        _deadUpgrade.SetActive(false);
    }
    #endregion
}
