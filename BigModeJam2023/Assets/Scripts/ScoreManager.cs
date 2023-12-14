using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    private int _score;
    private bool isGameOver = false;
    
    public GameObject ScoreScreen;
    public GameObject GameOverFinalScore;
    public GameObject HighestScore;
    void Start()
    {
        PlayerShipController.OnGameOver += GiveFinalScore;
        EnemyStats.OnDeath += GiveScore;
        ScoreScreen = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            ScoreScreen.GetComponent<TextMeshPro>().text = _score.ToString();
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
        GameOverFinalScore.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(_score).ToString();
        HighestScore.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("HighScore", (int)_score).ToString();
    }
    private void OnDestroy()
    {
        EnemyStats.OnDeath -= GiveScore;
        PlayerShipController.OnGameOver -= GiveFinalScore;
    }
}
