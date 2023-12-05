using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _projectileSpeed = 0.01f;
    [SerializeField] private float _gunCoolDown = 0.1f;
    [SerializeField] private GameObject Projectile;
    


    private float _shotTimeStamp = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _shotTimeStamp += Time.deltaTime;
    }
    private void Shoot()
    {
            GameObject laser = Instantiate(Projectile, transform.position, Quaternion.identity); // spawn projectile
            //laser.GetComponent<PlayerProjectileScript>().SetSpeed(_projectileSpeed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("is the player entering line of sight");
            Shoot();
        }
    }
}
