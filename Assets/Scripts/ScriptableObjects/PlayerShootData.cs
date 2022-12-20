using System;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerShootData")]
public class PlayerShootData : ScriptableObject
{
    [Header("Bullet Data")] 
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _nbEnemyTouch = 1;
    [SerializeField] private float _bulletSpeed = 10;
    [SerializeField] private float _maxDistance = 1000;
    [Header("Shoot Data")]
    [SerializeField] private int _nbShoot = 1;
    [SerializeField] private float shootDelay = 2;

    // Bullet data
    public int Damage { get => _damage; set => _damage = value; }
    public int NbEnemyTouch { get => _nbEnemyTouch; set => _nbEnemyTouch = value; } // == bullet life
    public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }    
    public float MaxDistance { get => _maxDistance;  set => _maxDistance = value; }    

    // Shoot data
    public int NbShoot { get => _nbShoot; set => _nbShoot = value; }
    public float ShootDelay { get => shootDelay; set => shootDelay = value; }

    public enum ShootEnum
    {
        Damage, NbEnemyTouch, BulletSpeed, NbShoot, ShootDelay, MaxDistance
    }

    public struct ShootRPC : INetworkSerializable
    {
        public int Damage;
        public int NbEnemyTouch;
        public float Speed;
        public float MaxDistance;
        
        public int NbShoot;
        public float ShootDelay;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Damage);
            serializer.SerializeValue(ref NbEnemyTouch);
            serializer.SerializeValue(ref Speed);
            serializer.SerializeValue(ref MaxDistance);
            
            serializer.SerializeValue(ref NbShoot);
            serializer.SerializeValue(ref ShootDelay);
        }
    }
    
    public void ApplyEffect(IntEffect effect)
    {
        switch (effect.Type)
        {
            case ShootEnum.Damage:
                Damage += effect.Value;
                break;
            case ShootEnum.NbEnemyTouch:
                NbEnemyTouch += effect.Value;
                break;
            case ShootEnum.NbShoot:
                NbShoot += effect.Value;
                break;
            default:
                Debug.Log("Type (int) : " + effect.Type + " is not recognize");
                break;
        }
    }
    
    public void ApplyEffect(FloatEffect effect)
    {
        switch (effect.Type)
        {
            case ShootEnum.BulletSpeed:
                BulletSpeed += effect.Value;
                break;
            case ShootEnum.ShootDelay:
                ShootDelay += effect.Value;
                if (ShootDelay < 0.1f) ShootDelay = 0.1f;
                break;
            case ShootEnum.MaxDistance:
                MaxDistance += effect.Value;
                break;
            default:
                Debug.Log("Type (float) : " + effect.Type + " is not recognize");
                break;
        }
    }

    public ShootRPC ToStruct()
    {
        return new ShootRPC()
        {
            Damage = Damage,
            Speed = BulletSpeed,
            NbEnemyTouch = NbEnemyTouch,
            NbShoot = NbShoot,
            ShootDelay = ShootDelay,
            MaxDistance = MaxDistance
        };
    }
}
