using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    public float _enemyHealth = 10;
    public float _enemySpeed = 1;
    public GameObject upgrade;
    public Camera gameAreaCamera;


    //Screen ScreenBoundariesBottomLeft will give the coordinates based on the cameras boundaries
    // ScreenBoundariesBottomLeft: Gives us the Y and X of the bottom and left of the screen
    // ScreenBoundariesTopRight: Gives the top of the screen and right side of the screen
    public Vector3 ScreenBoundariesBottomLeft;
    public Vector3 ScreenBoundariesTopRight;
    void Start()
    {
        gameAreaCamera = GameObject.Find("GameCam").GetComponent<Camera>();
        ScreenBoundariesTopRight = gameAreaCamera.ScreenToWorldPoint(new Vector3(0, 0, gameAreaCamera.transform.position.z));
        ScreenBoundariesBottomLeft = gameAreaCamera.ScreenToWorldPoint(new Vector3(gameAreaCamera.pixelRect.width, gameAreaCamera.pixelRect.height, gameAreaCamera.transform.position.z));
    }


    public void _enemyTakeDamage(float damageTaken)
    {
        _enemyHealth -= damageTaken;
        if (_enemyHealth <= 0)
        {
            spawnUpgrade();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            Destroy(other.gameObject);
            _enemyTakeDamage(other.GetComponent<PlayerProjectileScript>().GetDamage());
        }
        if(other.gameObject.tag == "PlayerRocket")
        {
            _enemyTakeDamage(5);
        }
        if (other.gameObject.tag == "PlayerLaser")
        {
            _enemyTakeDamage(5);
        }
    }
    private void spawnUpgrade()
    {
        if (Random.Range(0, 50) == 1)
        {
            Instantiate(upgrade, transform.position, Quaternion.identity);
        }
    }
}
