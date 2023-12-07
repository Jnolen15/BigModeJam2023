using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShoot : MonoBehaviour
{
    // Start is called before the first frame update
    public float _shootTimerMin = 1;
    public float _shootTimerMax = 5;

    [SerializeField] private float _projectileSpeed = -0.01f;
    [SerializeField] private float _gunCoolDown = 0.1f;
    [SerializeField] private GameObject Projectile;



    private float _shotTimeStamp = 0;
    void Start()
    {
        StartCoroutine("shootRandomly");
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
    
    private IEnumerator shootRandomly()
    {
        while (true)
        {
            float waitToshoot = Random.Range(_shootTimerMin, _shootTimerMax);
            yield return new WaitForSeconds(waitToshoot);
            Shoot();
        }
    }
}
