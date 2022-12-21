using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public abstract class ProjectileBase : NetworkBehaviour
    {
        public GameObject Owner { get; set; }
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
    }
}