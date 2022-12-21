using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace Unity.FPS.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [Tooltip("Image component dispplaying current health")]
        public Image HealthFillImage;

        Health m_PlayerHealth;

        public static PlayerHealthBar instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public void SetupHealthBar(GameObject localPlayer)
        {
            PlayerCharacterController playerCharacterController = localPlayer.GetComponent<PlayerCharacterController>();

            m_PlayerHealth = playerCharacterController.GetComponent<Health>();
        }

        void Update()
        {
            // update health bar value
            HealthFillImage.fillAmount = m_PlayerHealth.CurrentHealth.Value / m_PlayerHealth.MaxHealth;
        }
    }
}