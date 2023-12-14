using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBar : MonoBehaviour
{
    // ====================== Refrences / Variables ======================

    [SerializeField] private Material Active;
    [SerializeField] private Material Inactive;

    private PlayerShipController _shipController;
    private MeshRenderer _mesh;
    private float _startingScale;
    private float _previousScale;

    [SerializeField] private AudioSource _shieldAudioSource;
    [SerializeField] private AudioClip _startChargeShield;
    [SerializeField] private AudioClip _endChargeShield;
    private bool _usingShield;

    // ====================== Setup ======================
    void Start()
    {
        _shipController = GameObject.Find("Player Ship").GetComponent<PlayerShipController>();
        _mesh = GetComponent<MeshRenderer>();
        _startingScale = transform.localScale.y;
        _previousScale = _startingScale;
    }

    // ====================== Function ======================

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.y = _shipController.GetShieldRatio() * _startingScale;

        transform.Translate(new Vector3(0, (scale.y - _previousScale) / 2, 0));
        transform.localScale = scale;

        _previousScale = scale.y;

        if (_shipController.ShieldActive())
        {
            if (!_usingShield)
            {
                _usingShield = true;
                _mesh.material = Active;
                _shieldAudioSource.PlayOneShot(_startChargeShield);
            }
        } else
        {
            if (_usingShield)
            {
                _usingShield = false;
                _mesh.material = Inactive;
                _shieldAudioSource.PlayOneShot(_endChargeShield);
            }
        }

        //Start use shield
        
    }
}
