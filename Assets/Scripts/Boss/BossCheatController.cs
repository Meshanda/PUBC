using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossCheatController : NetworkBehaviour
{
    [SerializeField] private Transform _bossPrefab;

    private bool _hasSpawn = false;

    private Transform _boss;
    private BossController _bossController;
    
    private void OnSpawn(InputValue value)
    {
        if (_hasSpawn) return;
        
        Debug.Log("Boss : Spawn Boss");
        BossSpawnServerRpc();
        _hasSpawn = true;
    }
    
    private void OnAbilityOne(InputValue value)
    {
        if (!_hasSpawn) return;
       
        Debug.Log("Boss : Ability One");
        _bossController.LaunchLaser();
    }

    [ServerRpc]
    private void BossSpawnServerRpc()
    {
        _boss = Instantiate(_bossPrefab, new Vector3(0,0,0), Quaternion.identity);
        _boss.GetComponent<NetworkObject>().Spawn(true);
        _bossController = _boss.GetComponent<BossController>();
    }
}
