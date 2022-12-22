using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace Unity.FPS.UI
{
    public class WeaponHUDManager : NetworkBehaviour
    {
        [Tooltip("UI panel containing the layoutGroup for displaying weapon ammo")]
        public RectTransform AmmoPanel;

        [Tooltip("Prefab for displaying weapon ammo")]
        public GameObject AmmoCounterPrefab;

        PlayerWeaponsManager m_PlayerWeaponsManager;
        List<AmmoCounter> m_AmmoCounters = new List<AmmoCounter>();

        private NetworkClient _localClient;
        private NetworkObject _localPlayer;
        GameObject _ammoCounterInstance;
        
        public Coroutine localPlayerCoroutine;

        public override void OnNetworkSpawn()
        {
            _localClient = NetworkManager.LocalClient;
            StartCoroutine(GetLocalPlayer());
            _ammoCounterInstance = Instantiate(AmmoCounterPrefab, AmmoPanel);
        }

        private void SetupWeapon()
        {
            WeaponController activeWeapon = m_PlayerWeaponsManager.GetActiveWeapon();
            if (activeWeapon)
            {
                AddWeapon(activeWeapon, m_PlayerWeaponsManager.ActiveWeaponIndex);
                ChangeWeapon(activeWeapon);
            }

            m_PlayerWeaponsManager.OnAddedWeapon += AddWeapon;
            m_PlayerWeaponsManager.OnRemovedWeapon += RemoveWeapon;
            m_PlayerWeaponsManager.OnSwitchedToWeapon += ChangeWeapon;
        }

        void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            AmmoCounter newAmmoCounter = _ammoCounterInstance.GetComponent<AmmoCounter>();

            newAmmoCounter.Initialize(newWeapon, weaponIndex);

            m_AmmoCounters.Add(newAmmoCounter);
        }

        void RemoveWeapon(WeaponController newWeapon, int weaponIndex)
        {
            int foundCounterIndex = -1;
            for (int i = 0; i < m_AmmoCounters.Count; i++)
            {
                if (m_AmmoCounters[i].WeaponCounterIndex == weaponIndex)
                {
                    foundCounterIndex = i;
                    Destroy(m_AmmoCounters[i].gameObject);
                }
            }

            if (foundCounterIndex >= 0)
            {
                m_AmmoCounters.RemoveAt(foundCounterIndex);
            }
        }

        void ChangeWeapon(WeaponController weapon)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(AmmoPanel);
        }

        public IEnumerator GetLocalPlayer()
        {
            yield return new WaitUntil(() => _localClient.PlayerObject != null);
            _localPlayer = _localClient.PlayerObject;
            m_PlayerWeaponsManager = _localPlayer.GetComponent<PlayerWeaponsManager>();
            localPlayerCoroutine = null;
            SetupWeapon();
        }
    }
}