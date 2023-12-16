using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    public float _enemyHealth = 10;
    public float _enemySpeed = 1;
    public float _collisionDamage = 5;
    public GameObject upgrade;
    public Camera gameAreaCamera;
    public string EnemyName;
    public int Score = 0;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _explodeFireFX;
    [SerializeField] private GameObject _explodeSmokeFX;

    //Screen ScreenBoundariesBottomLeft will give the coordinates based on the cameras boundaries
    // ScreenBoundariesBottomLeft: Gives us the Y and X of the bottom and left of the screen
    // ScreenBoundariesTopRight: Gives the top of the screen and right side of the screen
    public Vector3 ScreenBoundariesBottomLeft;
    public Vector3 ScreenBoundariesTopRight;

    //Events
    public delegate void EnemyEvent(string enemyName);
    public static event EnemyEvent OnDeath;

    private BoxCollider2D enemyCollider;


    void Start()
    {
        gameAreaCamera = GameObject.Find("GameCam").GetComponent<Camera>();
        enemyCollider = gameObject.GetComponent<BoxCollider2D>();
        //enemyCollider.enabled = false;
        ScreenBoundariesTopRight = gameAreaCamera.ScreenToWorldPoint(new Vector3(0, 0, gameAreaCamera.transform.position.z));
        ScreenBoundariesBottomLeft = gameAreaCamera.ScreenToWorldPoint(new Vector3(gameAreaCamera.pixelRect.width, gameAreaCamera.pixelRect.height, gameAreaCamera.transform.position.z));
    }

    private void FixedUpdate()
    {
        if(transform.position.y < ScreenBoundariesTopRight.y && transform.position.x > ScreenBoundariesBottomLeft.x && transform.position.x < ScreenBoundariesTopRight.x)
        {
            enemyCollider.enabled = true;
        }
    }

    public void _enemyTakeDamage(float damageTaken)
    {
        _enemyHealth -= damageTaken;

        _spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => _spriteRenderer.DOColor(Color.white, 0.1f));

        if (_enemyHealth <= 0)
        {
            Instantiate(_explodeFireFX, transform.position, Quaternion.identity);
            Instantiate(_explodeSmokeFX, transform.position, Quaternion.identity);

            spawnUpgrade();
            OnDeath?.Invoke(EnemyName);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            other.gameObject.GetComponent<PlayerProjectileScript>().HitEnemy();
            _enemyTakeDamage(other.GetComponent<PlayerProjectileScript>().GetDamage());
        }
        if(other.gameObject.tag == "PlayerRocket")
        {
            _enemyTakeDamage(other.transform.parent.GetComponent<PlayerRocketScript>().GetDamage()); //this is weird need to fix
        }
        if (other.gameObject.tag == "PlayerLaser")
        {
            _enemyTakeDamage(other.GetComponent<PlayerLaserScript>().GetDamage());
        }
        if (other.gameObject.tag == "Shield" )
        {
            _enemyTakeDamage(other.transform.parent.GetComponent<RotatingShieldScript>().GetDamage());
        }
        if (other.gameObject.tag == "Player")
        {
            _enemyTakeDamage(_collisionDamage);
        }
        if (other.gameObject.tag == "BuzzSaw")
        {
            _enemyTakeDamage(other.transform.parent.GetComponent<RotatingShieldScript>().GetDamage());
        }
    }

    private void spawnUpgrade()
    {
        int upgradesPerEnemies = 32;
        if (Random.Range(1, upgradesPerEnemies - 1) == 1)
        {
            Instantiate(upgrade, transform.position, Quaternion.identity);
        }
    }
}
