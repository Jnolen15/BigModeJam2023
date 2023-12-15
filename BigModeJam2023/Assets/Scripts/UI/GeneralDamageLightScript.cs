using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GeneralDamageLightScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private string _systemName;
    [SerializeField] private float _flashFrequency = 0.5f;

    [SerializeField] private GameObject _cubeLight;
    private MeshRenderer _mesh;
    private TMP_Text _text;
    private float _hullDamage;

    [SerializeField] private Material _unDamagedLight;
    [SerializeField] private Material _damagedLight;

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

    // ====================== Function ======================
    public void ActivateLight(string system)
    {
        if (system == _systemName)
        {
            _hullDamage += 1;
            // start flashing if its the first system damaged
            if (_hullDamage == 1)
            {
                _mesh.material = _damagedLight;
                _mesh.material.DOColor(color2, _flashFrequency).OnComplete(() => _mesh.material.DOColor(color1, _flashFrequency)).SetLoops(-1, LoopType.Yoyo);
            }
            
        }
    }

    public void DeactivateLight(string system)
    {
        if (system == _systemName)
        {
            _hullDamage -= 1;
            // stops flashing if all systems are fixed
            if (_hullDamage == 0)
            {
                _mesh.material.DOKill();
                _mesh.material = _unDamagedLight;
            }
        }
    }

    private void OnDestroy()
    {
        CockpitDamageManager.OnSystemDamaged -= ActivateLight;
        CockpitDamageManager.OnSystemRepaired -= DeactivateLight;
    }
}