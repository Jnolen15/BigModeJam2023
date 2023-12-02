using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public GameObject upgrade;
    public float TimeBeforeLaunching = 2;
    public float ChargeSpeed = 1f;


    [SerializeField] public RectTransform _moveSpaceRect;
    // Boundries
    [SerializeField] private float _xLimit = 10;
    [SerializeField] private float _yLimit = 10;
    // Offset for play area
    [SerializeField] private float _xOffset = 0;
    [SerializeField] private float _yOffset = 0;

    private bool _stopLooking = false;
    private bool _startCharging = false;
    private bool _reachDestination = false;
    private float _MovingSpeed;
    void Start()
    {
        Player = GameObject.Find("Player Ship");
        //StartCoroutine("Charge");
    }

    // Update is called once per frame
    void Update()
    {
        _MovingSpeed = Time.deltaTime * ChargeSpeed;

        //Rotate enemy to face where the ship currently is
        //Will stop once is going to launch
        if (_reachDestination)
        {
            if (!_stopLooking)
            {
                //transform.LookAt(Player.transform);
                _MovingSpeed = 0;
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
        else
        {
            transform.position += transform.up  * _MovingSpeed;
        }

        if(transform.position.y < (_yLimit + _yOffset) - 7 && !_reachDestination){
            _reachDestination = true;
            StartCoroutine("Charge");
        }

        if(transform.position.y < -(_yLimit + _xOffset) + 3 || transform.position.x < -(_xLimit + _xOffset) || transform.position.x > (_xLimit + _xOffset))
        {
            Destroy(gameObject);
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
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("we going in here at all in charging enemy");
        if(other.gameObject.tag == "PlayerBullet")
        {
            spawnUpgrade();
            Destroy(gameObject);
        }
    }

    private void spawnUpgrade()
    {
        if (Random.Range(0, 10) == 1)
        {
            Instantiate(upgrade, transform.position, Quaternion.identity);
        }
    }

}
