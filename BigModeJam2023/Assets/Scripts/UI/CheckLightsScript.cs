using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CheckLightsScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private string _systemName;
    [SerializeField] private float _flashFrequency = 0.5f;

    [SerializeField] private GameObject _cubeLight;
    private MeshRenderer _mesh;
    private TMP_Text _text;
    private bool _systemDamaged;

    [SerializeField] private Material _unDamagedLight;
    [SerializeField] private Material _damagedLight;
    [SerializeField] private Material _damagedLight2;

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;



    // ====================== Setup ======================
    void Start()
    {
        _mesh = _cubeLight.GetComponent<MeshRenderer>();
        _text = GetComponentInChildren<TMP_Text>();
    

        _text.text = _systemName;
        _mesh.material = _unDamagedLight;
        CockpitDamageManager.OnSystemDamaged += ActivateLight;
        CockpitDamageManager.OnSystemRepaired += DeactivateLight;
    }

    private void FixedUpdate()
    {
       if (_systemDamaged)
        {
            
        }
    }

    // ====================== Function ======================
    public void ActivateLight(string system)
    {
        if (system == _systemName)
        {
            _systemDamaged = true;
            _mesh.material = _damagedLight;
            _mesh.material.DOColor(color2, _flashFrequency).OnComplete(() => _mesh.material.DOColor(color1, _flashFrequency)).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void DeactivateLight(string system)
    {
        if (system == _systemName)
        {
            _systemDamaged = false;
            _mesh.material.DOKill();
            _mesh.material = _unDamagedLight;
        }
    }

    private void OnDestroy()
    {
        CockpitDamageManager.OnSystemDamaged -= ActivateLight;
        CockpitDamageManager.OnSystemRepaired -= DeactivateLight;
    }


}
