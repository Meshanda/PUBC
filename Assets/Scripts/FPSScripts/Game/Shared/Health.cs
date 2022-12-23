using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public class Health : NetworkBehaviour
    {
        [Tooltip("Maximum amount of health")] 
        public float MaxHealth = 10f;

        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        public float CriticalHealthRatio = 0.3f;

        [SerializeField] AudioClip[] sounds;
        AudioSource myAudioSource;

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction<ulong,ulong> OnDie;

        public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();
        public NetworkVariable<bool> Invincible = new NetworkVariable<bool>();
        public bool CanPickup() => CurrentHealth.Value < MaxHealth;

        public float GetRatio() => CurrentHealth.Value / MaxHealth;
        public bool IsCritical() => GetRatio() <= CriticalHealthRatio;

        bool m_IsDead;

        

        void Start()
        {
            if(IsServer || IsHost)
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
            Debug.Log(damageSource.GetComponent<NetworkObject>().OwnerClientId);
            Debug.Log(damageSource.gameObject.name);
            TakeDamageServerRpc(damage, damageSource.GetComponent<NetworkObject>().OwnerClientId);
        }

        [ServerRpc (RequireOwnership = false)]
        public void TakeDamageServerRpc(float damage, ulong damageSourceClient)
        {
            if (Invincible.Value)
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
                TakeDamage(MaxHealth/2, gameObject);
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
                DespawnOnKillServerRpc();
                OnDie?.Invoke(killer.GetComponent<NetworkObject>().OwnerClientId,OwnerClientId);
            }
        }

        [ServerRpc]
        void DespawnOnKillServerRpc()
        {
            GetComponent<NetworkObject>().Despawn();
        }
        
    }
}