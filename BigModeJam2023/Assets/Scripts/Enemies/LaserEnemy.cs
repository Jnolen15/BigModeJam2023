using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Projectile;

    private bool _reachDestination = false;
    private float _MovingSpeed;
    private EnemyStats _es;
    private Vector3 newDestination;
    void Start()
    {
        _es = gameObject.GetComponent<EnemyStats>();
        newDestination = new Vector3(Random.Range(_es.ScreenBoundariesBottomLeft.x, _es.ScreenBoundariesTopRight.x), _es.ScreenBoundariesTopRight.y - 2, 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_reachDestination)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, newDestination, _es._enemySpeed * Time.deltaTime);
        }

        if(transform.position == newDestination && !_reachDestination)
        {
            _reachDestination = true;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject laser = Instantiate(Projectile, transform.position, Quaternion.identity); // spawn projectile
        newDestination = new Vector3(Random.Range(_es.ScreenBoundariesBottomLeft.x, _es.ScreenBoundariesTopRight.x), _es.ScreenBoundariesTopRight.y - 2, 0);
        _reachDestination = false;
        //laser.GetComponent<EnemyBulletScrip>().SetSpeed(_projectileSpeed);
    }
}
