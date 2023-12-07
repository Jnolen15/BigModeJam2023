using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int _enemyHealth = 10;
    public float _enemySpeed = 1;
    public GameObject upgrade;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_enemyHealth <= 0)
            Destroy(gameObject);


    }

    public void _enemyTakeDamage(int damageTaken)
    {
        _enemyHealth -= damageTaken;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            spawnUpgrade();
            Destroy(gameObject);
            Destroy(other.gameObject);
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
