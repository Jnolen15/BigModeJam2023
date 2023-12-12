using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckLightsScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private string _systemName;

    [SerializeField] private GameObject _cubeLight;
    private MeshRenderer _mesh;
    private TMP_Text _text;

    [SerializeField] private Material _unDamagedLight;
    [SerializeField] private Material _damagedLight;


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
            _mesh.material = _damagedLight;
        }
    }

    public void DeactivateLight(string system)
    {
        if (system == _systemName)
        {
            _mesh.material = _unDamagedLight;
        }
    }

    private void OnDestroy()
    {
        CockpitDamageManager.OnSystemDamaged -= ActivateLight;
        CockpitDamageManager.OnSystemRepaired -= DeactivateLight;
    }


}
