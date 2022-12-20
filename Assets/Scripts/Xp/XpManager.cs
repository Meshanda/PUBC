
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XpManager : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.Variables.FloatVariable _lvlSo, _xpSo, _xpThreshSo;
    [SerializeField] private GameEventSO _lvlUp;
    [SerializeField] private NetworkVariable<float> xp, lvl;
    [SerializeField] private NetworkVariable<float> _lvlThreshold;
    [SerializeField] float _refreshRate;
    private float _time;

     private bool _bLaunchedUpdate = false;
    // Start is called before the first frame update
    void Start()
    {
        _lvlSo.value = 0;
        _xpSo.value = 0;
        _xpThreshSo.value = _lvlThreshold.Value;
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
            _time = _refreshRate;
        else
            return;
        //if (NetworkManager.IsClient && !NetworkManager.Singleton.IsServer )
        //{
        //    _xpSo.value += 5;
        //}
        _xpThreshSo.value = _lvlThreshold.Value;

        if (lvl.Value > _lvlSo.value)
            _lvlSo.value = lvl.Value;
        if (_xpSo.value > xp.Value) 
        {
            if(NetworkManager.Singleton.IsServer)
            {
                xp.Value = _xpSo.value;
            }
            else if(!_bLaunchedUpdate)
            {
                _bLaunchedUpdate = true;
                UpdateXpServerRPC(_xpSo.value - xp.Value);
            }
        }

        if (_xpSo.value < xp.Value)
            _xpSo.value = xp.Value;

        if (NetworkManager.Singleton.IsServer && xp.Value > _lvlThreshold.Value) 
        {
            xp.Value = xp.Value - _lvlThreshold.Value;
            lvlUpClientRpc(xp.Value);
            lvl.Value++;
            SetThreshold();
        }

    }
    
    private void SetThreshold() 
    {
        _lvlThreshold.Value *= 1.5f;
    }
    [ClientRpc]
    private void lvlUpClientRpc(float xp)
    {
        _xpSo.value = xp;
        _lvlUp.Raise();
    }

    [ClientRpc]
    private void XpUpdateClientRpc(float xp)
    {
        _xpSo.value = xp;
        _bLaunchedUpdate = false;
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateXpServerRPC(float addXp) 
    {
        xp.Value += addXp;
        _xpSo.value += addXp;
        XpUpdateClientRpc(_xpSo.value);
    }
}
