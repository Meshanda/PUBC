using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ScriptableObjects.Variables;

public class TimeManager : NetworkBehaviour
{
    [SerializeField] private FloatVariable _maxRoundTime;
    [SerializeField] private FloatVariable _timeLeftGeneric;
    private NetworkVariable<float> _roundTimeLeft = new NetworkVariable<float>();

    public float GetTimeLeft()
    {
        return _roundTimeLeft.Value;
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
            _roundTimeLeft.Value = _maxRoundTime.value;
    }
    private void Update()
    {
        if (!IsServer) return;
        _roundTimeLeft.Value -= Time.deltaTime;
        _timeLeftGeneric.value = _roundTimeLeft.Value;
        if(_roundTimeLeft.Value <= 0)
        {
            StartCoroutine(GameManager.instance.GameFinished(true));
        }
    }
}
