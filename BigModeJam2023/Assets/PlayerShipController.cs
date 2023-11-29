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


    [SerializeField] private GameObject Projectile;

    private GameplayManager _gameplayManager;
    private float _shotTimeStamp = 0;


    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
    }

    // ====================== Function ======================
    void Update()
    {
        // Movement
        float speed = _moveSpeed * Time.timeScale; // adjusting for slow-mo
        Vector3 translation = Vector3.zero;
        if (Input.GetKey(KeyCode.D)) translation.x += speed; // Right
        if (Input.GetKey(KeyCode.A)) translation.x -= speed; // Left 
        if (Input.GetKey(KeyCode.W)) translation.y += speed; // Up
        if (Input.GetKey(KeyCode.S)) translation.y -= speed; // Down
        transform.Translate(translation);

        // Applying Boundries
        Vector3 currentPos = transform.position;
        float xLim = _xLimit + _xOffset; // adjusting for offset
        float yLim = _yLimit + _yOffset;
        if (transform.position.x > xLim) currentPos.x = xLim;
        if (transform.position.x < -xLim) currentPos.x = -xLim;
        if (transform.position.y > yLim) currentPos.y = yLim;
        if (transform.position.y < -yLim) currentPos.y = -yLim;
        transform.position = currentPos;

        // Shooting
        if (Input.GetMouseButton(0))
            Shoot();


        // adjusting time stamped variables
        if (_gameplayManager.GamePaused) 
            _shotTimeStamp += Time.deltaTime;
    }

    public void TakeDamage()
    {
        _gameplayManager.CurrentHealth -= 1;
    }

    private void Shoot()
    {
        if (_shotTimeStamp < Time.time)
        {
            GameObject laser = Instantiate(Projectile, transform);
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
