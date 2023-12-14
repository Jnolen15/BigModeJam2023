using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningAlgorithm : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Enemy> Enemies = new List<Enemy>();
    public int SpawnAmount = 5;
    public int StopSpawningWave = 10;
    public int spawnBreakTime = 15;
    public int DifficultyTimer = 12;
    public float SpawnCooldown = 1f;
    public bool CanSpawnOnceAgain = true;
    public Camera gameAreaCamera;



    [SerializeField] private RectTransform _moveSpaceRect;
    [SerializeField] private float _xLimit = 10;
    [SerializeField] private float _yLimit = 10;
    // Offset for play area
    [SerializeField] private float _xOffset = 0;
    [SerializeField] private float _yOffset = 0;

    private GameObject _leftPoint;
    private GameObject _rightPoint;
    private Vector3 _screenBoundariesBottomLeft;
    private Vector3 _screenBoundariesTopRight;
    private int _waveCost = 0;
    private List<GameObject> _generatedEnemy = new List<GameObject>();
    private bool gameStarting = true;
    private GameObject laserEnemyClone;
    
    void Start()
    {
        MainMenuUI.OnGameStarted += SetGameStart;

        EnemyStats.OnDeath += EnemyKill;
        gameAreaCamera = GameObject.Find("GameCam").GetComponent<Camera>();
        _screenBoundariesTopRight = gameAreaCamera.ScreenToWorldPoint(new Vector3(0, 0, gameAreaCamera.transform.position.z));
        _screenBoundariesBottomLeft = gameAreaCamera.ScreenToWorldPoint(new Vector3(gameAreaCamera.pixelRect.width, gameAreaCamera.pixelRect.height, gameAreaCamera.transform.position.z));

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
        _leftPoint.transform.position = new Vector3(_screenBoundariesBottomLeft.x, _screenBoundariesTopRight.y + 0.5f, 0);

        _rightPoint = new GameObject("RightPoint");
        _rightPoint.transform.position = new Vector3(_screenBoundariesTopRight.x, _screenBoundariesTopRight.y + 0.5f, 0);
    }

    private void OnDestroy()
    {
        MainMenuUI.OnGameStarted -= SetGameStart;
        EnemyStats.OnDeath -= EnemyKill;
    }

    // Start function
    private bool _gameStarted;
    private void SetGameStart()
    {
        Debug.Log("SpawningAlgorithm Start");

        _gameStarted = true;
        StartCoroutine("spawn");
        StartCoroutine("DiffilcultyScale");
    }

    private void EnemyKill(string nameOfEnemy)
    {
        if (nameOfEnemy == "LaserEnemy")
        {
            CanSpawnOnceAgain = true;
        }
    }
    private void GenerateEnemies()
    {
        _waveCost = SpawnAmount * 2;
        List<GameObject> enemiesGenerated = new List<GameObject>();
        while(_waveCost > 0)
        {
            int pickedEnemy = Random.Range(0, Enemies.Count);
            int pickedEnemyCost = Enemies[pickedEnemy].DangerLevel;
            if(_waveCost - pickedEnemyCost >= 0 && Enemies[pickedEnemy].SpawnCheck)
            {
                
                if (Enemies[pickedEnemy].Name == "LaserEnemy")
                {
                    if (CanSpawnOnceAgain)
                    {
                        enemiesGenerated.Add(Enemies[pickedEnemy].EnemyPrefab);
                        _waveCost -= pickedEnemyCost;
                        CanSpawnOnceAgain = false;
                    }
                }
                else
                {
                    enemiesGenerated.Add(Enemies[pickedEnemy].EnemyPrefab);
                    _waveCost -= pickedEnemyCost;
                }
            }
            else if(_waveCost < 0)
            {
                break;
            }
        }
        _generatedEnemy.Clear();
        _generatedEnemy = enemiesGenerated;

    }
    private IEnumerator spawn()
    {
        int wave = 0;
        while (true)
        {
            if (gameStarting)
            {
                gameStarting = false;
                yield return new WaitForSeconds(5);
            }
            if (wave < StopSpawningWave)
            {
                int i = 0;
                GenerateEnemies();
                while (i < _generatedEnemy.Count)
                {
                    GameObject enemyToSpawn = Instantiate(_generatedEnemy[i], Vector3.Lerp(_leftPoint.transform.position, _rightPoint.transform.position, (i + 1f) / (_generatedEnemy.Count + 1f)), new Quaternion(0, 0, 180, 0));
                    i++;
                    yield return null;
                }
                wave++;
                yield return new WaitForSeconds(SpawnCooldown);
            }
            else
            {
                wave = 0;
                yield return new WaitForSeconds(spawnBreakTime);
            }
            yield return null;
        }
    }
    IEnumerator DiffilcultyScale()
    {

        while(SpawnAmount < 30)
        {
            if (gameStarting)
            {
                gameStarting = false;
                yield return new WaitForSeconds(5);
            }
            yield return new WaitForSeconds(DifficultyTimer);
            SpawnAmount += 1;
            //At diffilctuy seven we add charging enemy
            //At difficulty 9 we add seeker
            //At difficulty 11 we add laser enemy
            switch (SpawnAmount)
            {
                case 7:
                    Enemies[0].SpawnCheck = true;
                    break;
                case 9:
                    Enemies[4].SpawnCheck = true;
                    break;
                case 11:
                    Enemies[5].SpawnCheck = true;
                    break;
            }
        }
    }
}

[System.Serializable]
public class Enemy
{
    public string Name;
    public GameObject EnemyPrefab;
    public int DangerLevel;
    public bool SpawnCheck;
    
}

