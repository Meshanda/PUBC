using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.Variables;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class XPItemScript : NetworkBehaviour
{
    [SerializeField] private BaseXPItem _xpItem;
    [SerializeField] private float _baseFlySpeed;
    [SerializeField] private GameObjectListVariable _playerList;
    [SerializeField] private FloatVariable _xpTracker;

    private GameObject _playerTarget;
    // Update is called once per frame
    void Update()
    {
        if (_playerTarget)
        {
            Vector3 targetPosition = _playerTarget.transform.position;
            targetPosition.y = transform.position.y;
            
            transform.LookAt(targetPosition);
            transform.position += transform.forward * (_baseFlySpeed * Time.deltaTime);
        }
    }

    public void ChoseTargettedPlayer()
    {
        float currentSmallestDistance = float.MaxValue;
        int smallestDistanceIndex = 0;
        for (int i = 0; i < _playerList.GetGos().Count; i++)
        {
            if (currentSmallestDistance  > Vector3.Distance(transform.position, _playerList.GetGos()[i].transform.position))
            {
                    currentSmallestDistance = Vector3.Distance(transform.position, _playerList.GetGos()[i].transform.position);
                    smallestDistanceIndex = i;
            }
        }
        _playerTarget =  _playerList.GetGos()[smallestDistanceIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _xpTracker.value += _xpItem.xpValue;
            if (NetworkManager.Singleton.IsServer)
            {
                CatchXP();
            }
            else
            {
                CatchXPServerRPC();
            }
        }
    }

    private void CatchXP()
    {
        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CatchXPServerRPC()
    {
        Destroy(gameObject);
    }
}
