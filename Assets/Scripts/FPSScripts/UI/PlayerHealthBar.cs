using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

namespace Unity.FPS.UI
{
    public class PlayerHealthBar : NetworkBehaviour
    {
        [Tooltip("Image component dispplaying current health")]
        public Image HealthFillImage;

        Health m_PlayerHealth;

        private NetworkClient _localClient;
        private NetworkObject _localPlayer;
        private Coroutine _localPlayerCoroutine;

        public override void OnNetworkSpawn()
        {
            _localClient = NetworkManager.LocalClient;
        }

        void Update()
        {
            // update health bar value
            if(_localPlayer == null && _localPlayerCoroutine == null)
            {
                _localPlayerCoroutine = StartCoroutine(GetLocalPlayer());
                return;
            }
            else if(_localPlayer == null)
            {
                return;
            }
        }

        private void UpdateHealthBar(float previousValue, float newValue)
        {
            HealthFillImage.fillAmount = newValue / m_PlayerHealth.MaxHealth;
        }

        private IEnumerator GetLocalPlayer()
        {
            yield return new WaitUntil(() => _localClient.PlayerObject != null);
            _localPlayer = _localClient.PlayerObject;
            m_PlayerHealth = _localPlayer.GetComponent<Health>();
            _localPlayerCoroutine = null;
            UpdateHealthBar(m_PlayerHealth.MaxHealth, m_PlayerHealth.MaxHealth);
            m_PlayerHealth.CurrentHealth.OnValueChanged += UpdateHealthBar;
        }
    }
}