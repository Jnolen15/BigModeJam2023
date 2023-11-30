using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _moveSpeed = 0.01f;
    [SerializeField] private float _gunCoolDown = 0.1f;
    [SerializeField] private float _projectileSpeed = 0.01f;
    // Boundries
    [SerializeField] private float _xLimit = 10;
    [SerializeField] private float _yLimit = 10;
    // Offset for play area
    [SerializeField] private float _xOffset = 0; 
    [SerializeField] private float _yOffset = 0;

    // play area
    [SerializeField] private RectTransform _moveSpaceRect;


    [SerializeField] private GameObject Projectile;

    private GameplayManager _gameplayManager;
    private float _shotTimeStamp = 0;


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
    }

    public void TakeDamage()
    {
        _gameplayManager.CurrentHealth -= 1;
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

    // ====================== Collisions ======================
    // TODO make sure this works
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Ship Collided with: " + other.name);
        if (true)
        {
            TakeDamage();
        }
    }

}
