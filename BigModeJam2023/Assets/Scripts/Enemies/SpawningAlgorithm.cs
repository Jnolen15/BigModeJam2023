using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningAlgorithm : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Enemies;
    public int SpawnAmount = 0;
    public float SpawnCooldown = 1f;


    [SerializeField] private RectTransform _moveSpaceRect;
    [SerializeField] private float _xLimit = 10;
    [SerializeField] private float _yLimit = 10;
    // Offset for play area
    [SerializeField] private float _xOffset = 0;
    [SerializeField] private float _yOffset = 0;

    private GameObject _leftPoint;
    private GameObject _rightPoint;
    void Start()
    {
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
        _leftPoint = new GameObject("LeftPoint");
        _leftPoint.transform.position = new Vector3(-(_xLimit + _xOffset), (_yLimit + _yOffset) + 5, 0);

        _rightPoint = new GameObject("RightPoint");
        _rightPoint.transform.position = new Vector3(_xLimit + _xOffset, (_yLimit + _yOffset) + 5, 0);

        StartCoroutine("spawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator spawn()
    {
        while (true)
        {
            int i = 0;
            while(i < SpawnAmount)
            {
                Enemies = Instantiate(Enemies, Vector3.Lerp(_leftPoint.transform.position, _rightPoint.transform.position, (i + 1f) / (SpawnAmount + 1f)), Quaternion.identity);
                i++;
                yield return null;
            }
            yield return new WaitForSeconds(SpawnCooldown);
        }
    }
}
