using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerShipController : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [Header("Base Stats")]
    [SerializeField] private float _moveSpeed = 0.01f;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _shieldDuration = 8;
    [SerializeField] private float _shieldRegenDelay = 3;
    [SerializeField] private float _shieldGracePeriod = 0.5f; // time shield is active after being disabled
    [SerializeField] private float _invincibilityDuration = 0.6f;
    [SerializeField] private float _invincibilityFlashRate = 0.2f;
    [SerializeField] private float _screenShakeDuration = 1;
    [SerializeField] private float _screenShakeMagnitude = 0.001f;
    [SerializeField] private float _repairHealthNum = 15;
    [SerializeField] private float _damageReceiveNum = 20;

    [Header("Active Stats")]
    [SerializeField] private float _currentHealth = 100;
    [SerializeField] private float _currentShield = 8;
    [SerializeField] private float _Gun1FireChance = 100;
    [SerializeField] private float _Gun2FireChance = 100;

    [Header("Weapons")]
    [SerializeField] private float _gunCoolDown = 0.1f;
    [SerializeField] private float _projectileSpeed = 0.01f;
    [SerializeField] private float _shotWidth = 0.1f;
    [SerializeField] private float _altFireCoolDown = 1f;
    [SerializeField] private float _shotgunShots = 20;
    [SerializeField] private float _shotgunSpread = 0.01f;
    private bool _rocketEquipped = false;
    private bool _laserEquipped = false;
    private bool _shotgunEquipped = false;
    private bool _staggeredFire;
    private bool _leftShot;

    [Header("Powerups/Debuffs")]
    [SerializeField] float _GunCoolDownUpgradeMultiplier = 0.5f;
    [SerializeField] float _moveSpeedUpgradeMultiplier = 1.5f;
    [SerializeField] float _projectileSpeedUpgradeMultiplier = 1.5f;
    [SerializeField] float _gunDamagedMultiplier = 0.5f;
    [SerializeField] float _engineDamagedMultiplier = 0.8f;

    private float _shotTimeStamp = 0;
    private float _altFireTimeStamp = 0;
    private float _shieldRechargeTimeStamp = 0;
    private bool _invincible = false;
    private bool _canControl;


    [Header("GameObjects")]
    [SerializeField] private RectTransform _moveSpaceRect;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _rocket;
    [SerializeField] private GameObject _laser;
    private GameplayManager _gameplayManager;

    [Header("Audio")]
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioSource _audioSource;

    // Boundries
    private float _xLimit = 10;
    private float _yLimit = 10;
    // Offset for play area
    private float _xOffset = 0;
    private float _yOffset = 0;



    // Events
    public delegate void ShipControllerEvent();
    public static event ShipControllerEvent OnUpgradePickUp;
    public static event ShipControllerEvent OnGameOver;
    public static event ShipControllerEvent OnTakeDamage;
    public static event ShipControllerEvent OnAltFire;

    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        _currentShield = _shieldDuration;

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

        CockpitController.OnGoToCockpit += ExitScreen;
        CockpitController.OnGoToGame += EnterScreen;
        CockpitDamageManager.OnSystemDamaged += SystemDamaged;
        CockpitDamageManager.OnSystemRepaired += SystemRepaired;
    }

    // ====================== Function ======================

    private void Update()
    {
        // temp fun with timescales
        if (Input.GetKeyDown(KeyCode.F)) Time.timeScale = 0.5f;
        if (Input.GetKeyDown(KeyCode.G)) Time.timeScale = 1;

        // testing for upgrades
        //if (Input.GetKeyDown(KeyCode.Q)) _gunCoolDown *= _GunCoolDownUpgradeMultiplier;
        //if (Input.GetKeyDown(KeyCode.E)) _gunCoolDown /= _GunCoolDownUpgradeMultiplier;

        if (Input.GetKeyDown(KeyCode.X)) TakeDamage(20);

        if (Input.GetKeyDown(KeyCode.Q)) _staggeredFire = !_staggeredFire;
    }
    void FixedUpdate()
    {
        if (_canControl)
        {
            // Movement
            DoMovement();

            // Shooting
            if (Input.GetMouseButton(0))
                Shoot();
            if (Input.GetMouseButton(1))
                AltFire();
        }

        // adjusting time stamped variables
        if (_gameplayManager.GamePaused) 
            _shotTimeStamp += Time.deltaTime;

        // recharging and using shield
        if (_canControl)
        {
            if (_currentShield < _shieldDuration && Time.time > _shieldRechargeTimeStamp) _currentShield += Time.deltaTime;
            
        } else
        {
            if (_currentShield > 0) _currentShield -= Time.deltaTime;
        }            
    }

    private void DoMovement()
    {
        float speed = _moveSpeed * Time.timeScale; // adjusting for slow-mo
        Vector3 translation = Vector3.zero;

        // floaty Movement
        // translation.y += Input.GetAxis("Vertical") * speed; // Up/Down
        // translation.x += Input.GetAxis("Horizontal") * speed; // Left/Right
        // Snappy Movement
        translation.y += Input.GetAxisRaw("Vertical") * speed; // Up/Down
        translation.x += Input.GetAxisRaw("Horizontal") * speed; // Left/Right
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
    }

    private void ExitScreen()
    {
        _canControl = false;
    }

    private void EnterScreen()
    {
        _canControl = true;
        _shieldRechargeTimeStamp = Time.time + _shieldRegenDelay;
        StartCoroutine(Invincibility(_shieldGracePeriod));
    }

    // Makes player take damage, using shield before health, with no "carry over" between shield and health
    public void TakeDamage(float damageNum)
    {
        if (!(_invincible || ShieldActive()))
        {
            StartCoroutine(TempInvincibility(_invincibilityDuration, _invincibilityFlashRate));
            StartCoroutine(ScreenShake(_screenShakeDuration, _screenShakeMagnitude));
            OnTakeDamage?.Invoke();
            _currentHealth -= damageNum;

            _audioSource.PlayOneShot(_damageSound);

            if (_currentHealth <= 0)
            {
                OnGameOver?.Invoke();
                //Time.timeScale = 0;
                _gameplayManager.GameOver = true;
            }
        }
    }

    #region Weapons
    private void Shoot()
    {
        if (_staggeredFire)
        {
            if (_shotTimeStamp < Time.time)
            {
                if (_leftShot)
                    ShootBullet(_Gun1FireChance, _shotWidth);
                else
                    ShootBullet(_Gun2FireChance, -_shotWidth);

                _leftShot = !_leftShot;
                _shotTimeStamp = Time.time + (_gunCoolDown / 2);
            }
        }
        else
        {
            if (_shotTimeStamp < Time.time)
            {
                ShootBullet(_Gun1FireChance, _shotWidth);
                ShootBullet(_Gun2FireChance, -_shotWidth);

                _shotTimeStamp = Time.time + _gunCoolDown;
            }
        }
    }

    private void ShootBullet(float fireChance, float xOffset)
    {
        if (fireChance >= 100 || Random.Range(0, 100) > fireChance) // % chance to fire gun if gun system is damaged
        {
            GameObject laser = Instantiate(_projectile, transform.position + new Vector3(xOffset, 0, 0), Quaternion.identity);
            laser.GetComponent<PlayerProjectileScript>().SetSpeed(0, _projectileSpeed);

            _audioSource.PlayOneShot(_shootSound);
        }
    }

    private void AltFire()
    {
        if (_altFireTimeStamp < Time.time)
        {
            if (_shotgunEquipped) Shotgun();
            if (_laserEquipped) Laser();
            if (_rocketEquipped) Rocket();
            OnAltFire?.Invoke();
            _altFireTimeStamp = Time.time + _altFireCoolDown;
        }
    }

    private void Shotgun()
    {
        for (float i = 0; i < _shotgunShots; i++)
        {
            float drift = Random.Range(-_shotgunSpread, _shotgunSpread);
            GameObject laser1 = Instantiate(_projectile, transform.position, Quaternion.identity);
            laser1.GetComponent<PlayerProjectileScript>().SetSpeed(drift, _projectileSpeed - Mathf.Abs(drift)/2);
        }

        _audioSource.PlayOneShot(_shootSound);
    }

    private void Laser()
    {
        GameObject laser1 = Instantiate(_laser, transform.position, Quaternion.identity);

        _audioSource.PlayOneShot(_shootSound);
    }
    private void Rocket()
    {
        GameObject rocket1 = Instantiate(_rocket, transform.position, Quaternion.identity);

        _audioSource.PlayOneShot(_shootSound);
    }
    #endregion

    #region Coroutines
    IEnumerator Invincibility(float time) // obselete
    {
        _invincible = true;
        yield return new WaitForSeconds(time);
        _invincible = false;
        yield return null;
    }

    // Cooroutine for invincibility, takes in invincibility time and causes sprite to flash every 0.5 seconds
    // may result in longer invincibility if flash timer is not divisible by invincibility timer
    IEnumerator TempInvincibility(float time, float flashTime)
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        Color baseColor = sprite.color;
        Color transparentColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.5f);
        float t = time;
        bool Opaque = true;
        while (t >= 0)
        {
            Opaque = !Opaque;
            t -= flashTime;
            if (Opaque)
            {
                sprite.color = baseColor;
            }
            else
            {
                sprite.color = transparentColor;
            }
            yield return new WaitForSeconds(flashTime);
        }
        sprite.color = baseColor;
        _invincible = false;
    }

    IEnumerator ScreenShake(float time, float magnitude)
    {
        Transform t = Camera.main.transform;
        Vector3 lastPos = Vector3.zero;
        float TimeStamp = Time.time + time;
        while (TimeStamp > Time.time)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;
            // undo last transform, add new transform, save new transform as old
            t.position -= lastPos;
            t.position += new Vector3(x, y, z);
            lastPos = new Vector3(x, y, z);
            yield return null;
        }
        t.position -= lastPos;
    }
    #endregion


    #region Upgrade/Debuff Switch functions
    // Event Functions
    public void ActivateUpgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            case "Shotgun":
                _shotgunEquipped = true;
                break;
            case "Laser":
                _laserEquipped = true;
                   break;
            case "Rocket":
                _rocketEquipped = true;
                break;
            case "Time Warp":
                Time.timeScale = 0.75f;
                break;

            //old upgrades
            case "FireRate":
                _gunCoolDown *= _GunCoolDownUpgradeMultiplier;
                break;
            case "MoveSpeed":
                _moveSpeed *= _moveSpeedUpgradeMultiplier;
                break;
            case "ProjectileSpeed":
                _projectileSpeed *= _projectileSpeedUpgradeMultiplier;
                break;
            case "Shield":
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
            case "Shotgun":
                _shotgunEquipped = false;
                break;
            case "Laser":
                _laserEquipped = false;
                break;
            case "Rocket":
                _rocketEquipped = false;
                break;
            case "Time Warp":
                Time.timeScale =1;
                break;

            //old upgrades
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

    public void SystemDamaged(string damage)
    {
        switch (damage)
        {
            case "Gun1":
                _Gun1FireChance *= _gunDamagedMultiplier;
                break;
            case "Gun2":
                _Gun2FireChance *= _gunDamagedMultiplier;
                break;
            case "Engine1":
                _moveSpeed *= _engineDamagedMultiplier;
                break;
            case "Engine2":
                _moveSpeed *= _engineDamagedMultiplier;
                break;

            default:
                Debug.Log("Invalid system name");
                break;
        }
    }

    public void SystemRepaired(string damage)
    {
        _currentHealth += _repairHealthNum;
        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        switch (damage)
        {
            case "Gun1":
                _Gun1FireChance /= _gunDamagedMultiplier;
                break;
            case "Gun2":
                _Gun2FireChance /= _gunDamagedMultiplier;
                break;
            case "Engine1":
                _moveSpeed /= _engineDamagedMultiplier;
                break;
            case "Engine2":
                _moveSpeed /= _engineDamagedMultiplier;
                break;

            default:
                Debug.Log("Invalid system name");
                break;
        }
    }

    #endregion
    

    // Floats and Bools
    public float GetHealthRatio()
    {
        return _currentHealth / _maxHealth;
    }

    public float GetShieldRatio()
    {
        return _currentShield / _shieldDuration;
    }

    public bool ShieldActive()
    {
        if (!_canControl && _currentShield > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    // ====================== Collisions ======================
    // TODO make sure this works
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Ship Collided with: " + other.name);
        if (collision.tag == "Hostile")
        {
            TakeDamage(_damageReceiveNum);
            if (other.GetComponent<SeekerEnemy>() == null)
            {
                Destroy(other.gameObject);
            }
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
        CockpitController.OnGoToCockpit -= ExitScreen;
        CockpitController.OnGoToGame -= EnterScreen;
        CockpitDamageManager.OnSystemDamaged -= SystemDamaged;
        CockpitDamageManager.OnSystemRepaired -= SystemRepaired;
    }
}
