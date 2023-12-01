using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    // base stats
    [SerializeField] private float _moveSpeed = 0.01f;
    [SerializeField] private float _gunCoolDown = 0.1f;
    [SerializeField] private float _projectileSpeed = 0.01f;
    [SerializeField] private float _currentShield = 0;

    // powerups
    [SerializeField] float _GunCoolDownUpgradeMultiplier = 0.5f;
    [SerializeField] float _moveSpeedUpgradeMultiplier = 1.5f;
    [SerializeField] float _projectileSpeedUpgradeMultiplier = 1.5f;
    [SerializeField] private float _maxShield = 100;
    [SerializeField] private float _shieldDecayRate = 0.1f;
    [SerializeField] private float _shieldDecayAmount = 1;

    // GameObjects
    [SerializeField] private RectTransform _moveSpaceRect;
    [SerializeField] private GameObject Projectile;
    private GameplayManager _gameplayManager;

    // Boundries
    private float _xLimit = 10;
    private float _yLimit = 10;
    // Offset for play area
    private float _xOffset = 0;
    private float _yOffset = 0;

    private float _shotTimeStamp = 0;

    // Events
    public delegate void ShipControllerEvent();
    public static event ShipControllerEvent OnUpgradePickUp;



    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        // setting offsets and limits
        if (_moveSpaceRect != null)
        {
            _xOffset = _moveSpaceRect.position.x;
            _yOffset = _moveSpaceRect.position.y;
            _xLimit = _moveSpaceRect.rect.width / 2;
            _yLimit = _moveSpaceRect.rect.height / 2; 
        }
        else
        {
            Debug.LogFormat("Movement Area not set");
        }

        // subscribing to upgrade events
        UpgradeSlot.OnStartUpgrade += ActivateUpgrade;
        UpgradeSlot.OnEndUpgrade += EndUpgrade;
        CockpitController.OnGoToCockpit += ActivateShield;
        CockpitController.OnGoToGame += InterruptShield;
    }

    // ====================== Function ======================
    void Update()
    {
        // Movement
        float speed = _moveSpeed * Time.timeScale; // adjusting for slow-mo
        Vector3 translation = Vector3.zero;

        translation.y += Input.GetAxis("Vertical") * speed; // Up/Down
        translation.x += Input.GetAxis("Horizontal") * speed; // Left/Right
        /* old input system
        if (Input.GetKey(KeyCode.D)) translation.x += speed; // Right
        if (Input.GetKey(KeyCode.A)) translation.x -= speed; // Left 
        if (Input.GetKey(KeyCode.W)) translation.y += speed; // Up
        if (Input.GetKey(KeyCode.S)) translation.y -= speed; // Down
        */
        if (Mathf.Abs(translation.x) >= speed && Mathf.Abs(translation.y) >= speed) // adjusting diagonal speed
            translation *= 0.7f; // 0.7 is an approximation for root 0.5 cause im lazy
        transform.Translate(translation);

        // Applying Boundries
        Vector3 currentPos = transform.position;
        if (transform.position.x > _xLimit + _xOffset) currentPos.x = _xLimit + _xOffset;
        if (transform.position.x < -_xLimit + _xOffset) currentPos.x = -_xLimit + _xOffset;
        if (transform.position.y > _yLimit + _yOffset) currentPos.y = _yLimit + _yOffset;
        if (transform.position.y < -_yLimit + _yOffset) currentPos.y = -_yLimit + _yOffset;
        transform.position = currentPos;

        // Shooting
        if (Input.GetMouseButton(0))
            Shoot();


        // adjusting time stamped variables
        if (_gameplayManager.GamePaused) 
            _shotTimeStamp += Time.deltaTime;

        // temp fun with timescales
        if (Input.GetKeyDown(KeyCode.F)) Time.timeScale = 0.5f;
        if (Input.GetKeyDown(KeyCode.G)) Time.timeScale = 1;

        // testing for upgrades
        if (Input.GetKeyDown(KeyCode.Q)) _gunCoolDown *= _GunCoolDownUpgradeMultiplier;
        if (Input.GetKeyDown(KeyCode.E)) _gunCoolDown /= _GunCoolDownUpgradeMultiplier;

        if (Input.GetKeyDown(KeyCode.X)) TakeDamage(20);
    }

    // Makes player take damage, using shield before health, with no "carry over" between shield and health
    public void TakeDamage(float damageNum)
    {
        if (_currentShield > 0)
        {
            _currentShield -= damageNum;
        } else
        {
            _gameplayManager.CurrentHealth -= damageNum;
        }
        
    }
    private void Shoot()
    {
        if (_shotTimeStamp < Time.time)
        {
            GameObject laser = Instantiate(Projectile, transform.position, Quaternion.identity); // spawn projectile
            laser.GetComponent<PlayerProjectileScript>().SetSpeed(_projectileSpeed);
            _shotTimeStamp = Time.time + _gunCoolDown;
        }
    }

    private void ActivateShield()
    {
        _currentShield = _maxShield;
        StartCoroutine(ShieldDecay(_shieldDecayAmount, _shieldDecayRate));
    }

    private void InterruptShield()
    {
        _currentShield = 0;
    }


    // decays the shield at decayrate for decayamount
    IEnumerator ShieldDecay(float decayamount, float decayRate)
    {
        yield return new WaitForSeconds(decayRate);
        while (_currentShield > 0)
        {
            _currentShield -= decayamount;
            yield return new WaitForSeconds(decayRate);
        }

        yield return null;
    }

    // Event Functions
    public void ActivateUpgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            case "FireRate":
                _gunCoolDown *= _GunCoolDownUpgradeMultiplier;
                break;
            case "MoveSpeed":
                _moveSpeed *= _moveSpeedUpgradeMultiplier;
                break;
            case "ProjectileSpeed":
                _projectileSpeed *= _projectileSpeedUpgradeMultiplier;
                break;
            case "shield":
                // whatever the shield upgrade does here
                break;

            default:
                Debug.Log("Invalid upgrade name");
                break;
        }
    }

    public void EndUpgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            case "FireRate":
                _gunCoolDown /= _GunCoolDownUpgradeMultiplier;
                break;
            case "MoveSpeed":
                _moveSpeed /= _moveSpeedUpgradeMultiplier;
                break;
            case "ProjectileSpeed":
                _projectileSpeed /= _projectileSpeedUpgradeMultiplier;
                break;
            case "Shield":
                // whatever the shield upgrade does here
                break;

            default:
                Debug.Log("Invalid upgrade name");
                break;
        }
    }

    // ====================== Collisions ======================
    // TODO make sure this works
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Ship Collided with: " + other.name);
        if (collision.tag == "Enemy")
        {
            TakeDamage(20);
        }

        if (collision.tag == "Upgrade")
        {
            OnUpgradePickUp?.Invoke();
            Destroy(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        UpgradeSlot.OnStartUpgrade -= ActivateUpgrade;
        UpgradeSlot.OnEndUpgrade -= EndUpgrade;
        CockpitController.OnGoToGame -= ActivateShield;
        CockpitController.OnGoToGame -= InterruptShield;
    }
}
