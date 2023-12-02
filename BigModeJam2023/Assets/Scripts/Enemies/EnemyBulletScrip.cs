using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScrip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += (transform.up * -1) * (1 * Time.deltaTime);
    }
}
