using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _moveSpeed = 0.1f;

    private GameplayManager _gameplayManager;


    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("Gameplay Manager").GetComponent<GameplayManager>();
    }

    // ====================== Function ======================
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        _gameplayManager.CurrentHealth -= 1;
    }

    // ====================== Collisions ======================
    // TODO make sure this works
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Ship Collided with: " + other.name);
 
    }

}
