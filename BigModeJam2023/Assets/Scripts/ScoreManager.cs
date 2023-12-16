using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    private int _score;
    private bool isGameOver = false;
    
    public TextMeshPro ScoreScreen;
    public TextMeshProUGUI GameOverFinalScore;
    public TextMeshProUGUI HighestScore;
    [SerializeField] private GameOverUI _gameOverUI;

    public delegate void ScoreEvent();
    public static event ScoreEvent OnReachSecret;
    [SerializeField] private List<MedalEntry> _medalThresholds;
    private bool _secretScoreReached;

    void Start()
    {
        PlayerShipController.OnGameOver += GiveFinalScore;
        EnemyStats.OnDeath += GiveScore;
    }

    private void OnDestroy()
    {
        EnemyStats.OnDeath -= GiveScore;
        PlayerShipController.OnGameOver -= GiveFinalScore;
    }

    void Update()
    {
        if (!isGameOver)
        {
            ScoreScreen.text = _score.ToString();
        }

        // Medals stuff
        if (_score > _medalThresholds[_medalThresholds.Count-1].Threshold && !_secretScoreReached)
        {
            _secretScoreReached = true;
            OnReachSecret?.Invoke();
        }
    }

    private void GiveScore(string nameOfEnemy)
    {
        switch (nameOfEnemy)
        {
            case "ChargingEnemy":
                _score += 200;
                break;
            case "ForwardChargingEnemy":
                _score += 100;
                break;
            case "ShootingEnemy":
                _score += 200;
                break;
            case "RandomShootingEnemy":
                _score += 100;
                break;
            case "SeekerEnemy":
                _score += 600;
                break;
            case "LaserEnemy":
                _score += 1800;
                break;
        }
    }

    private void GiveFinalScore()
    {
        isGameOver = true;
        if (_score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)_score);
        }
        GameOverFinalScore.text = Mathf.Floor(_score).ToString();
        HighestScore.text = PlayerPrefs.GetInt("HighScore", (int)_score).ToString();

        // Medals stuff
        foreach (MedalEntry entry in _medalThresholds)
        {
            Debug.Log($"Testing medal {entry.MedalName} {entry.Threshold} against {_score}");

            if(_score >= entry.Threshold)
            {
                if (PlayerPrefs.GetInt(entry.MedalName) != 1) // Give medal{
                {
                    Debug.Log(entry.MedalName + " medal earned!");
                    PlayerPrefs.SetInt(entry.MedalName, 1);
                }
            }
        }

        if(_gameOverUI != null)
            _gameOverUI.ShowMedals();
    }

    [System.Serializable]
    public class MedalEntry
    {
        public int Threshold;
        public string MedalName;
    }
}
