using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    // Start is called before the first frame update
    public GameObject Player;
    public float TimeBeforeLaunchingMin = 2;
    public float TimeBeforeLaunchingMax = 5;
    public float ChargeSpeed = 1f;
    public float _movingSpeed;

    [SerializeField] public RectTransform _moveSpaceRect;
    // Boundries
    [SerializeField] private float _xLimit = 10;
    [SerializeField] private float _yLimit = 10;
    // Offset for play area
    [SerializeField] private float _xOffset = 0;
    [SerializeField] private float _yOffset = 0;

    private EnemyStats _es;
    void Start()
    {
        Player = GameObject.Find("Player Ship");
        _es = gameObject.GetComponent<EnemyStats>();
        int chooseSide = Random.Range(0, 2);
        if(chooseSide == 0)
        {
            transform.position = new Vector3(_es.ScreenBoundariesBottomLeft.x - 1, Random.Range(_es.ScreenBoundariesBottomLeft.y, _es.ScreenBoundariesTopRight.y), 0);
        }
        if (chooseSide == 1)
        {
            transform.position = new Vector3(_es.ScreenBoundariesTopRight.x + 1, Random.Range(_es.ScreenBoundariesBottomLeft.y, _es.ScreenBoundariesTopRight.y), 0);
        }

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.LookRotation
                (Player.transform.position - transform.position, transform.TransformDirection(Vector3.forward));
                transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, _movingSpeed * Time.deltaTime);
    }
}
