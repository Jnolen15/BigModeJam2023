using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLaserScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _damage = 20;
    [SerializeField] private float _lifespan = 1.5f;

    private SpriteRenderer _sprite;



    // ====================== Setup ======================
    void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();

        _sprite.DOFade(0, _lifespan).SetEase(Ease.OutSine).OnComplete(() => Destroy(gameObject));
    }

    // ====================== Function ======================
    public float GetDamage()
    {
        return _damage;
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
