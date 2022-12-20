using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerNetworkData))]
public class PlayerNetworkShoot : NetworkBehaviour
{
    [SerializeField] private Transform _bulletPrefab;

    private PlayerShootData _playerShootData;

    private float _bulletXOffset = 0.3f;

    private void Start()
    {
        _playerShootData = GetComponent<PlayerNetworkData>().PlayerShootData;
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootServerRpc(_playerShootData.ToStruct());
        }
    }
    
    [ServerRpc]
    private void ShootServerRpc(PlayerShootData.ShootRPC shootData)
    {
        float startPosX = transform.position.x;
       
        if (shootData.NbShoot % 2 == 0)
        {
            startPosX -= (_bulletXOffset / 2);
        }

        startPosX -= (shootData.NbShoot - 1) / 2 * _bulletXOffset;
        
        for (int i = 0; i < shootData.NbShoot; i++)
        {
            Vector3 position = Vector3.forward; // decalage de 0.3
            position += new Vector3(startPosX, 0, 0);
            position += Vector3.right * (i * _bulletXOffset);

            Transform spawnedObject = Instantiate(_bulletPrefab, position, Quaternion.identity);
            spawnedObject.GetComponent<NetworkObject>().Spawn(true);

            Bullet bullet = spawnedObject.GetComponent<Bullet>();

            // Set Bullet specs ...
            bullet.Damage = shootData.Damage;
            bullet.Life = shootData.NbEnemyTouch;
            bullet.Speed = shootData.Speed;

            BulletManager.Instance.AddBullet(bullet);
        }
    }
    
}
