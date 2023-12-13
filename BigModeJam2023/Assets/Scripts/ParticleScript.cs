using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _destroyTime;
    [SerializeField] private bool _emitOnStart;
    [SerializeField] private int _emitNum;
    private ParticleSystem _ps;

    // ====================== Setup ======================
    void Start()
    {
        _ps = this.GetComponent<ParticleSystem>();

        if (_ps == null)
            Destroy(gameObject);

        if (_emitOnStart)
            _ps.Emit(_emitNum);

        Destroy(gameObject, _destroyTime);
    }
}
