using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LaserManager : NetworkBehaviour
{
    [SerializeField] private Transform _laserPrefab;
    [SerializeField] private Transform _laserSpawnPoint;

    [SerializeField] private float _timeAttack = 5.0f;
    [SerializeField] private int _nbLaser = 1;
    [SerializeField] private float _laserSpeed;

    [SerializeField] private bool _moveLaser;

    [SerializeField] private Transform _topLevelParentTransform;

    private List<Transform> _spawnedLasers = new List<Transform>();

    private bool _hasLaserSpawned;

    private float _timeSpend = 0.0f;

    private Vector3 _posStartOfLaser;

    private void Update()
    {
        if (_hasLaserSpawned)
        {
            //_topLevelParentTransform.position = _posStartOfLaser;
            if(_moveLaser)
                MoveLaserClientRpc(_laserSpeed);

            _timeSpend += Time.deltaTime;   

            if (_timeSpend >= _timeAttack)
            {
                // End the attack
                _timeSpend = 0.0f;
                EndAttackClientRpc();
            }
        }
    }

    [ClientRpc]
    private void MoveLaserClientRpc(float speed)
    {
        _laserSpeed = speed;
        
        foreach (Transform spawnedLaser in _spawnedLasers)
        {
            LineRenderer laserLineRenderer = spawnedLaser.GetChild(0).GetComponent<LineRenderer>();

            Vector3 currentDirection = laserLineRenderer.GetPosition(1) - laserLineRenderer.GetPosition(0);
            // float angle = CalculateAngle(Vector3.forward, currentDirection);

            float angle = Vector3.SignedAngle(Vector3.forward, currentDirection, Vector3.up);
            if (angle < 0)
            {
                angle = 360 - angle * -1;
            }

            angle += _laserSpeed;
            angle = VerifyAngle(angle);

            Vector3 newDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));

            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            
            RaycastHit hit;
            Physics.Raycast(transform.position, newDirection, out hit, Mathf.Infinity, layerMask); 

            laserLineRenderer.SetPosition(1, _laserSpawnPoint.position + (newDirection * hit.distance));
        }
    }

    [ClientRpc]
    private void EndAttackClientRpc()
    {
        _hasLaserSpawned = false;
        foreach (Transform laser in _spawnedLasers)
        {
            Destroy(laser.gameObject);
        }
        _spawnedLasers.Clear();
    }

    private float VerifyAngle(float angle)
    {
        return angle >= 360.0f ? 0.0f : angle;
    }

    private static float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    public void StartAbility()
    {
        //_posStartOfLaser = _topLevelParentTransform.position;
        LaserClientRpc(_nbLaser);
    }

    [ClientRpc]
    private void LaserClientRpc(int nbLaser)
    {
        _nbLaser = nbLaser;
        
        float augmentAngle = 360.0f / _nbLaser;
        float angle = 0.0f;

        for (int i = 0; i < _nbLaser; i++)
        {
            Transform spawnedTransform = Instantiate(_laserPrefab, _laserSpawnPoint);
            _spawnedLasers.Add(spawnedTransform);

            // spawnedTransform.GetComponent<NetworkObject>().Spawn(true);

            LineRenderer laserLineRenderer = spawnedTransform.GetChild(0).GetComponent<LineRenderer>();

            angle = i * augmentAngle;
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));

            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask);

            Vector3 position = _laserSpawnPoint.position;
            laserLineRenderer.SetPosition(0, position);
            laserLineRenderer.SetPosition(1, position + (direction * hit.distance));
        }
        
        _hasLaserSpawned = true;
    }
}