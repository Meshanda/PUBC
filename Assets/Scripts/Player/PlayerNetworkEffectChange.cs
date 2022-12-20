using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerNetworkData))]
public class PlayerNetworkEffectChange : NetworkBehaviour
{
    [SerializeField] private IntEffect _nbShootUp;
    [SerializeField] private IntEffect _nbShootDown;
    
    [SerializeField] private FloatEffect _bulletSpeedUp;
    [SerializeField] private FloatEffect _bulletSpeedDown;

    private PlayerShootData _playerShootData;
    
    private void Start()
    {
        _playerShootData = GetComponent<PlayerNetworkData>().PlayerShootData;
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.O))
        {
            _playerShootData.ApplyEffect(_nbShootUp);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _playerShootData.ApplyEffect(_nbShootDown);
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            _playerShootData.ApplyEffect(_bulletSpeedUp);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            _playerShootData.ApplyEffect(_bulletSpeedDown);
        }
    }
}
