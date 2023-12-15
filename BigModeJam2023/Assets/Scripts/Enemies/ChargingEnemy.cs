using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public float TimeBeforeLaunchingMin = 2;
    public float TimeBeforeLaunchingMax = 5;
    public float ChargeSpeed = 1f;


    [SerializeField] public RectTransform _moveSpaceRect;
    // Boundries
    [SerializeField] private float _xLimit = 10;
    [SerializeField] private float _yLimit = 10;
    // Offset for play area
    [SerializeField] private float _xOffset = 0;
    [SerializeField] private float _yOffset = 0;

    [SerializeField] private AudioSource _audioSource;

    private bool _stopLooking = false;
    private bool _startCharging = false;
    private bool _reachDestination = false;
    private float _MovingSpeed;
    private EnemyStats _es; 
    void Start()
    {
        _es = gameObject.GetComponent<EnemyStats>();
        Player = GameObject.Find("Player Ship");
        _moveSpaceRect = GameObject.Find("ShipMovementSpace").GetComponent<RectTransform>();

        if (_moveSpaceRect != null)
        {
            _xOffset = _moveSpaceRect.position.x;
            _yOffset = _moveSpaceRect.position.y;
            _xLimit = _moveSpaceRect.rect.width / 2;
            _yLimit = _moveSpaceRect.rect.height / 2;
        }
        else
        {
            Debug.LogFormat("Movement Area not set");
        }
        //StartCoroutine("Charge");
    }

    // Update is called once per frame
    void FixedUpdate()
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

        if(transform.position.y < (_es.ScreenBoundariesTopRight.y) - 1.5f && !_reachDestination){
            _reachDestination = true;
            StartCoroutine("Charge");
        }

        if(transform.position.y < _es.ScreenBoundariesBottomLeft.y - 1 || transform.position.x > _es.ScreenBoundariesTopRight.x + 1|| transform.position.x < _es.ScreenBoundariesBottomLeft.x - 1)
        {
            Destroy(gameObject);
        }

    }

    private IEnumerator Charge()
    {
        //Before launching enemy will stop looking at ship by turning _stopLooking true
        //Enemy will then charge by turning _startCharging true
        //After a couple of seconds enemy will be destroyed out of camera view
        float randTime = Random.Range(TimeBeforeLaunchingMin, TimeBeforeLaunchingMax);
        yield return new WaitForSeconds(randTime);
        _stopLooking = true;
        _startCharging = true;

        _audioSource.Play();
    }

    

    

}
