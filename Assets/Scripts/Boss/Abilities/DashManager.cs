using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DashManager : MonoBehaviour
{
    [SerializeField] private Transform _boss;
    [SerializeField] private Transform _bossBulletPrefab;
    
    [SerializeField] private float _dashDistance;
    
    [SerializeField] private float _speed;
    
    [Header("Shoots")]
    [SerializeField] private bool _shootDuringDash;
    [SerializeField] private float _intervalShootDuring;
    
    [SerializeField] private bool _shootAfterDash;
    [Tooltip("Will be multiplied by 4")]
    [Range(1, 3)][SerializeField] private int _nbShootAfter; 


    private bool _isDashing;
    
    private Vector3 _targetPosition;
    private float _timeSpend = 0;
    private void Update()
    {
        if (_isDashing)
        {
            _boss.position = Vector3.MoveTowards(_boss.position, _targetPosition, _speed * Time.deltaTime);

            // Shoot during dash (with a cooldown i guess)
            if (_shootDuringDash && _timeSpend >= _intervalShootDuring)
            {
                _timeSpend = 0;
                
                CreateBullet(2, 90);
            }
            
            if (Vector3.Distance(_boss.position, _targetPosition) < 0.001f)
            {
                _isDashing = false;
                HasArrivedAtDestination();
                _timeSpend = 0;
            }

            _timeSpend += Time.deltaTime;
        }
    }

    private void HasArrivedAtDestination()
    {
        if(!_shootAfterDash) return;

        CreateBullet(_nbShootAfter * 4, 0);
    }

    private void CreateBullet(int nbBullet, float initialAngleOffset )
    {
        float angle = 360.0f / nbBullet;
        
        for (int i = 0; i < nbBullet; i++)
        {
            Transform spawnedObject = Instantiate(_bossBulletPrefab, _boss.position, Quaternion.identity);
            spawnedObject.GetComponent<NetworkObject>().Spawn(true);
            
            // Change their rotation
            spawnedObject.rotation = Quaternion.Euler(0, initialAngleOffset + i * angle, 0);
            
            Bullet bullet = spawnedObject.GetComponent<Bullet>();
            bullet.Damage = 10;
            bullet.Life = 1;
            bullet.Speed = 10;
            
            BulletManager.Instance.AddBullet(bullet);
        }
    }

    public void StartAbility()
    {
        // Dash
        Vector3 finalPosition = _boss.position + _boss.forward * _dashDistance;
        finalPosition = VerifyFinalPosition(finalPosition);
        
        _targetPosition = finalPosition;
        _isDashing = true;
    }

    private Vector3 VerifyFinalPosition(Vector3 position)
    {
        // Verify that there is not wall 
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        
        RaycastHit hit;
        if (Physics.Raycast(_boss.position, _boss.forward, out hit, _dashDistance, layerMask))
        {
            // If hit -> there is a wall close
            return hit.point;
        }
        
        // The path is free 
        return position;
    }
}
