using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{



    [SerializeField] private RectTransform _moveSpaceRect;
    // Boundries
    [SerializeField] private float _xLimit = 10;
    [SerializeField] private float _yLimit = 10;
    // Offset for play area
    [SerializeField] private float _xOffset = 0;
    [SerializeField] private float _yOffset = 0;


    private Vector2 _screenBoundaries;
    private float _movespeed = 2;
    private string _typeOfMovement;
    private bool _waitToGoDown = true;
    void Start()
    {
        _screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        // setting offsets and limits
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
        _typeOfMovement = "GoRight";
    }

    // Update is called once per frame
    void Update()
    {
        switch (_typeOfMovement)
        {
            case "GoRight":
                if (this.transform.position.x < (_xLimit + _xOffset))
                {
                    transform.Translate(Vector3.left * _movespeed * Time.deltaTime);
                }
                else
                    _typeOfMovement = "GoDownfromRight";
                break;
            case "GoDownfromRight":
                transform.Translate((-Vector3.down) * _movespeed * Time.deltaTime);
                
                if (_waitToGoDown)
                {
                    _waitToGoDown = false;
                    StartCoroutine("_waitDownRight");

                }
                break;
            case "GoLeft":
                if (this.transform.position.x > -(_xLimit + _xOffset))
                { 
                    transform.Translate(-Vector3.left * _movespeed * Time.deltaTime);
                }
                else
                {
                    _typeOfMovement = "GoDownfromLeft";
                }
                break;
            case "GoDownfromLeft":
                transform.Translate((-Vector3.down) * _movespeed * Time.deltaTime);
                
                if (_waitToGoDown)
                {
                    _waitToGoDown = false;
                    StartCoroutine("_waitDownLeft");
                }
                
                break;
        }
        Debug.Log(_screenBoundaries.x);


    }

    private IEnumerator _waitDownRight()
    {
        yield return new WaitForSeconds(0.1f);
        _waitToGoDown = true;
        _typeOfMovement = "GoLeft";

    }

    private IEnumerator _waitDownLeft()
    {
        yield return new WaitForSeconds(0.1f);
        _waitToGoDown = true;
        _typeOfMovement = "GoRight";

    }
}
