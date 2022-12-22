using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public abstract class ProjectileBase : NetworkBehaviour
    {
        public GameObject Owner
        {
            get
            {
                NetworkObject OwnerReturn = NetworkManager.LocalClient.PlayerObject;
                return OwnerReturn.gameObject;
            }
        }

        public ulong OwnerId { get; set; }
        public Vector3 InitialPosition { get; private set; }
        public Vector3 InitialDirection { get; private set; }
        public Vector3 InheritedMuzzleVelocity { get; set; }
        public float InitialCharge { get; set; }

        public UnityAction OnShoot;

        [ClientRpc]
        public void ShootClientRpc()
        {
            if (!IsOwner)
                return;
            
            InitialPosition = transform.position;
            InitialDirection = transform.forward;

            OnShoot?.Invoke();
        }


        public void TryDestroying(UInt16 life = 0)
        {
            if (IsSpawned || life > 15)
            {
                DestroyObjectServerRpc();
            }
            else
            {
                life++;
                TryDestroying(life);
            }

        }
        
        
        [ServerRpc]
        public void DestroyObjectServerRpc()
        {
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}