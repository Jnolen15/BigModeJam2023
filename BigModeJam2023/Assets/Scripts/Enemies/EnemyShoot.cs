using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _projectileSpeed = -0.01f;
    [SerializeField] private float _gunCoolDown = 0.1f;
    [SerializeField] private GameObject Projectile;
    


    private float _shotTimeStamp = 0;
    private GameObject _parent;
    private EnemyStats _es;
    void Start()
    {
        _parent = gameObject.transform.parent.gameObject;
        _es = _parent.GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _shotTimeStamp += Time.deltaTime;
    }
    private void Shoot()
    {
            GameObject laser = Instantiate(Projectile, transform.position, Quaternion.identity); // spawn projectile
            laser.GetComponent<EnemyBulletScrip>().SetSpeed(_projectileSpeed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Need to get parents position so script can start shooting when they show up in the screen
        if(other.gameObject.tag == "Player" && (_parent.transform.position.y < _es.ScreenBoundariesTopRight.y))
        {

            Shoot();
        }
    }
}
