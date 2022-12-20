using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossCooldownScript : MonoBehaviour
{
    //Laser is not an implemented feature
    //[SerializeField] private float _laserBaseCooldown;
    [SerializeField] private float _dashBaseCooldown;
    [SerializeField] private float _abilityBaseGlobalCooldown;
    [SerializeField] private BossController _controller;


    private float /*_laserCooldown,*/ _dashCooldown, _abilityGlobalCooldown;
    private bool /*_bHasLaserFired,*/ _bHasDashOccured;
    private void Start()
    {
        //_laserCooldown = _laserBaseCooldown;
        _dashCooldown = _dashBaseCooldown;
        _abilityGlobalCooldown = _abilityBaseGlobalCooldown;
        //_bHasLaserFired = false;
        _bHasDashOccured = false;
    }

    private void Update()
    {
        /*if (_bHasLaserFired)
            _laserCooldown -= Time.deltaTime;
        if (_laserCooldown <= 0)
            _bHasLaserFired = false;*/
        if (_bHasDashOccured)
            _dashCooldown -= Time.deltaTime;
        if (_dashCooldown <= 0)
            _bHasDashOccured = false;

        _abilityGlobalCooldown -= Time.deltaTime;
        if (_abilityGlobalCooldown <= 0)
        {
            /*if (!_bHasLaserFired)
            {
                _controller.LaunchLaser();
                _bHasLaserFired = true;
                _laserCooldown = _laserBaseCooldown;
                _abilityGlobalCooldown = _abilityBaseGlobalCooldown;
                return;
            }*/
            if (!_bHasDashOccured)
            {
                _controller.LaunchDash();
                _bHasDashOccured = true;
                _dashCooldown = _dashBaseCooldown;
                _abilityGlobalCooldown = _abilityBaseGlobalCooldown;
                return;
            }
        }
    }
}
