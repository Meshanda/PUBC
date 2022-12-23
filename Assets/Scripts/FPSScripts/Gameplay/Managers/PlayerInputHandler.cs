using Unity.FPS.Game;
using UnityEngine;
using Unity.Netcode;

namespace Unity.FPS.Gameplay
{
    public class PlayerInputHandler : NetworkBehaviour
    {
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        public override void OnNetworkSpawn()
        {
            if (!IsOwner) enabled = false;
        }


    }
}