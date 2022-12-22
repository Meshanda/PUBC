using Unity.Netcode;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class Damageable : NetworkBehaviour
    {
        [Tooltip("Multiplier to apply to the received damage")]
        public float DamageMultiplier = 1f;

        [Range(0, 1)] [Tooltip("Multiplier to apply to self damage")]
        public float SensibilityToSelfdamage = 0.5f;

        public Health Health { get; private set; }

        void Awake()
        {
            // find the health component either at the same level, or higher in the hierarchy
            Health = GetComponent<Health>();
            if (!Health)
            {
                Health = GetComponentInParent<Health>();
            }
        }

        
        [ServerRpc (RequireOwnership = false)]
        public void InflictDamageServerRpc(float damage, bool isExplosionDamage, ulong networkObjId)
        {
            GameObject damageSource = NetworkManager.ConnectedClients[networkObjId].PlayerObject.gameObject;
            if (Health)
            {
                var totalDamage = damage;

                // skip the crit multiplier if it's from an explosion
                if (!isExplosionDamage)
                {
                    totalDamage *= DamageMultiplier;
                }

                // apply the damages
                Health.TakeDamage(totalDamage, damageSource);
            }
        }
    }
}