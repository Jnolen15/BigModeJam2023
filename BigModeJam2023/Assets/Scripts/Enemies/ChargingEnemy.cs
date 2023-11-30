using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public float TimeBeforeLaunching = 2;
    public float ChargeSpeed = 1f;

    private bool _stopLooking = false;
    private bool _startCharging = false;
    private float _MovingSpeed;
    void Start()
    {
        StartCoroutine("Charge");
    }

    // Update is called once per frame
    void Update()
    {
        _MovingSpeed = Time.deltaTime * ChargeSpeed;

        //Rotate enemy to face where the ship currently is
        //Will stop once is going to launch
        if (!_stopLooking)
        {
            //transform.LookAt(Player.transform);

            Quaternion rotation = Quaternion.LookRotation
            (Player.transform.position - transform.position, transform.TransformDirection(Vector3.forward));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }
        //When enemy charges it will use transform.up * -1 to make it move towards the player instead of flying off to the opposite side to keep going in the direction the player was
        if (_startCharging)
        {
            transform.position += (transform.up * -1) * _MovingSpeed;
        }
        

    }

    private IEnumerator Charge()
    {
        //Before launching enemy will stop looking at ship by turning _stopLooking true
        //Enemy will then charge by turning _startCharging true
        //After a couple of seconds enemy will be destroyed out of camera view
        yield return new WaitForSeconds(TimeBeforeLaunching);
        _stopLooking = true;
        _startCharging = true;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
