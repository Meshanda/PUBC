using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public class Health : NetworkBehaviour
    {
        [Tooltip("Maximum amount of health")] public float MaxHealth = 10f;

        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        public float CriticalHealthRatio = 0.3f;

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction<UInt64,ulong> OnDie;

        public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();
        public bool Invincible { get; set; }
        public bool CanPickup() => CurrentHealth.Value < MaxHealth;

        public float GetRatio() => CurrentHealth.Value / MaxHealth;
        public bool IsCritical() => GetRatio() <= CriticalHealthRatio;

        bool m_IsDead;

        void Start()
        {
            CurrentHealth.Value = MaxHealth;
        }

        public void Heal(float healAmount)
        {
            float healthBefore = CurrentHealth.Value;
            CurrentHealth.Value += healAmount;
            CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value, 0f, MaxHealth);

            // call OnHeal action
            float trueHealAmount = CurrentHealth.Value - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }
        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            TakeDamageServerRpc(damage, damageSource.GetComponent<NetworkObject>().OwnerClientId);
        }

        [ServerRpc]
        public void TakeDamageServerRpc(float damage, ulong damageSourceClient)
        {
            if (Invincible)
                return;

            float healthBefore = CurrentHealth.Value;
            CurrentHealth.Value -= damage;
            CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value, 0f, MaxHealth);

            // call OnDamage action
            float trueDamageAmount = healthBefore - CurrentHealth.Value;
            if (trueDamageAmount > 0f)
            {
                OnDamaged?.Invoke(trueDamageAmount, NetworkManager.ConnectedClients[damageSourceClient].PlayerObject.gameObject);
            }

            HandleDeath(NetworkManager.ConnectedClients[damageSourceClient].PlayerObject.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && GetComponent<NetworkObject>().IsOwner)
            {
                TakeDamage(MaxHealth, gameObject);
            }
        }
        public void Kill()
        {
            CurrentHealth.Value = 0f;

            // call OnDamage action
            OnDamaged?.Invoke(MaxHealth, null);

            HandleDeath(gameObject);
        }

        void HandleDeath(GameObject killer = null)
        {
            if (m_IsDead)
                return;

            // call OnDie action
            if (CurrentHealth.Value <= 0f)
            {
                m_IsDead = true;
                OnDie?.Invoke((UInt64)killer.GetComponent<NetworkObject>()?.OwnerClientId,OwnerClientId);
            }
        }
    }
}