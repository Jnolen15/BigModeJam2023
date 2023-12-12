using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyLaserCode : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _lifespan = 1.5f;

    private GameObject _laserObj;
    private LaserEnemy _ls;
    private EnemyStats _es;
    private bool stopAnimation = false;


    public bool isShooting = false;
    public Sprite TopPivotSprite;



    // ====================== Setup ======================
    void Start()
    {
        _ls = gameObject.transform.parent.gameObject.GetComponent<LaserEnemy>();
        _es = gameObject.transform.parent.gameObject.GetComponent<EnemyStats>();
        _laserObj = transform.parent.gameObject;

        //start charge up of beam and then make it nothing
        gameObject.transform.DOScale(0.02f, 2).OnComplete(() => circleToNothing());
    }

    // ====================== Function ======================

    private void circleToNothing()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.DOScale(0, 0.3f).OnComplete(() => gameObject.transform.DOScaleX(0.06f, 0).OnComplete(() => changeSprite()));
    }

    private void changeSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = TopPivotSprite;
        wobbleAnimationPart1();
        gameObject.transform.DOScaleY(2, 1).OnComplete(() => keepLaserGoing());
        
    }
    private void keepLaserGoing()
    {
        StartCoroutine("laserTimer");
        
    }

    private void wobbleAnimationPart1()
    {
        if(!stopAnimation)
            gameObject.transform.DOScaleX(0.065f, 0.07f).OnComplete(() => wobbleAnimationPart2());
    }
    private void wobbleAnimationPart2()
    {
        if (!stopAnimation)
            gameObject.transform.DOScaleX(0.055f, 0.07f).OnComplete(() => wobbleAnimationPart1());
    }




    IEnumerator laserTimer()
    {
        
        yield return new WaitForSeconds(3);
        StopCoroutine("wobbleAnimation");
        stopAnimation = true;
        gameObject.transform.DOScaleX(0, 1).OnComplete(() => OnDestroy());
    }


    private void OnDestroy()
    {
        _ls.IsShooting = false;
        Destroy(gameObject);
    }
}
