using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsShooting = false;


    [SerializeField] private GameObject Projectile;

    private bool _reachDestination = false;
    private float _MovingSpeed;
    private EnemyStats _es;
    private Vector3 _newDestination;
    void Start()
    {
        _es = gameObject.GetComponent<EnemyStats>();
        _newDestination = new Vector3(Random.Range(_es.ScreenBoundariesBottomLeft.x + 2, _es.ScreenBoundariesTopRight.x - 2), _es.ScreenBoundariesTopRight.y - 0.5f, 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsShooting && !_reachDestination)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, _newDestination, _es._enemySpeed * Time.deltaTime);
        }

        if(transform.position == _newDestination && !_reachDestination)
        {
            _reachDestination = true;
            Shoot();
        }
    }

    private void Shoot()
    {
        IsShooting = true;
        GameObject laser = Instantiate(Projectile, transform.position + new Vector3(0, -0.4f), Quaternion.identity); // spawn projectile
        laser.transform.parent = gameObject.transform;
        _newDestination = new Vector3(Random.Range(_es.ScreenBoundariesBottomLeft.x + 2, _es.ScreenBoundariesTopRight.x - 2), _es.ScreenBoundariesTopRight.y - 0.5f, 0);
        _reachDestination = false;
        //laser.GetComponent<EnemyBulletScrip>().SetSpeed(_projectileSpeed);
    }
}
