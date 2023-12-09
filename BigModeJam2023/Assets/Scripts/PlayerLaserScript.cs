using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLaserScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _damage = 20;
    [SerializeField] private float _lifespan = 1.5f;

    private GameObject _laserObj;
    private SpriteRenderer _sprite;



    // ====================== Setup ======================
    void Start()
    {
        _laserObj = transform.parent.gameObject;
        _sprite = GetComponent<SpriteRenderer>();

        // StartCoroutine(VisualFade(_lifespan));
        _sprite.DOFade(0, _lifespan).SetEase(Ease.OutSine).OnComplete(() => Destroy(gameObject));
    }

    // ====================== Function ======================
    IEnumerator VisualFade(float time)
    {
        float fadeStep = 5;
        Color baseColor = _sprite.color;
        for (float i = 0; i <= 100; i+= fadeStep) // fade in
        {
            _sprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, i/100);
            yield return new WaitForSeconds((time/2) / (100/fadeStep));
        }
        // sprite.color = baseColor;
        for (float i = 100; i >= 0; i-= fadeStep) // fade out
        {
            _sprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, i/100);
            yield return new WaitForSeconds((time / 2) / (100 / fadeStep));
        }

        Destroy(gameObject);
        yield return null;
    }

    IEnumerator Extension(float time)
    {
        yield return null;
    }

    public float GetDamage()
    {
        return _damage;
    }

    private void OnDestroy()
    {
        Destroy(_laserObj);
    }
}
