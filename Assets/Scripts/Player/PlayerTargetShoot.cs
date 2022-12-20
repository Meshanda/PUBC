using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class PlayerTargetShoot : NetworkBehaviour
{
#region Rotation
    [Space(5)]
    [Header("Rotation field")]

    [Range(0.0f, 0.3f)]
    [Tooltip("How fast the character face the direction")]
    [SerializeField] private float _rotateSpeed;
    [FormerlySerializedAs("_killian")] [SerializeField] private GameObject _rotateFrom; 

    private Vector3 _playerLookAt;
    private float _targetRotation;
    private float _rotationVelocity;
#endregion

#region Bullet
    [SerializeField] private Transform _bulletPrefab;

    private PlayerShootData _playerShootData;

    private float _bulletXOffset = 0.5f;

    private bool _onShoot = false;
    private bool _canShoot = true;

    public bool CanShoot
    {
        set => _canShoot = value;
    }
    
    private float _bulletTimer;

#endregion

    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _bulletOrigine;

    private void Awake()
    {
        _playerShootData = GetComponentInChildren<PlayerNetworkData>().PlayerShootData;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;

        Rotate();
        
        if(_bulletTimer < 0.0f)
        {
            if (_onShoot && _canShoot)
            {
                ShootServerRpc(_playerShootData.ToStruct());
                _canShoot = false;
            }
        }
        else
        {
            _bulletTimer -= Time.deltaTime;
            _canShoot = true;
        }
    }

    private void Rotate()
    {
        _playerLookAt.Normalize();
        
        _targetRotation = Mathf.Atan2(_playerLookAt.x, _playerLookAt.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotateSpeed);
        
        _rotateFrom.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
    
    protected void OnTargetMouse(InputValue value)
    {
        if (!IsOwner)
            return;
        
        Vector2 mousePosition = value.Get<Vector2>();

        Ray mouseRay = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit rayHit, float.PositiveInfinity, _terrainLayer))
        {
            _playerLookAt = rayHit.point - transform.position;
        }
    }
    
    protected void OnTargetGP(InputValue value)
    {
        if (!IsOwner)
            return;
        
        Vector2 gamePadLA = value.Get<Vector2>();

        if(gamePadLA == Vector2.zero)
        {
            _onShoot = false;
            return;
        }
        
        _playerLookAt = new Vector3(gamePadLA.x, 0, gamePadLA.y);
        
        Debug.Log(_playerLookAt);

        _onShoot = true;
    }
    
    
    protected void OnShoot(InputValue value)
    {
        if (!IsOwner)
            return;
        
        _onShoot = value.isPressed;
    }
    
    [ServerRpc]
    private void ShootServerRpc(PlayerShootData.ShootRPC shootData)
    {
        //_bulletTimer = shootData.ShootDelay;
        
        SetTimerClientRpc(shootData.ShootDelay);
        
        for (int i = 0; i < shootData.NbShoot; i++)
        {
            Vector3 position = _bulletOrigine.position; // decalage de 0.3
            position += _rotateFrom.transform.right * (i * _bulletXOffset);
            
            Transform spawnedObject = Instantiate(_bulletPrefab, position, Quaternion.identity);
            spawnedObject.transform.forward = _rotateFrom.transform.forward;
            spawnedObject.GetComponent<NetworkObject>().Spawn(true);
            

            Bullet bullet = spawnedObject.GetComponent<Bullet>();

            // Set Bullet specs ...
            bullet.Damage = shootData.Damage;
            bullet.Life = shootData.NbEnemyTouch;
            bullet.Speed = shootData.Speed;
            bullet.MaxDistance = shootData.MaxDistance;
        }
    }

    [ClientRpc]
    private void SetTimerClientRpc(float _bulletTimerN)
    {
        if (!IsOwner)
            return; 
        
        _bulletTimer = _bulletTimerN;
    }
}
