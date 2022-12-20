using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public int Damage { get; set; }
    public int Life { get; set; }
    public float Speed { get; set; }
    
    public float MaxDistance { get; set; }

    private float _distTraveled;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        ManageDistance();

        if (!IsOwner)
            return;

        Transform bulletTransform = transform;
        Vector3 newPosition = bulletTransform.position + bulletTransform.forward * (Speed * Time.deltaTime);
        bulletTransform.position = newPosition;
    }

    private void ManageDistance()
    {
        if (!IsServer)
            return;

        _distTraveled = Vector3.Distance(_startPos, transform.position);
        if (_distTraveled > MaxDistance)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }


    public bool IsDead()
    {
        return Life <= 0;
    }

    public void OnTriggerEnter(Collider other) 
    {
        EnemyScript es = other.GetComponent<EnemyScript>();
        if (es == null)
            return;
        es.Damage(Damage);
        Life -= 1;
        if(IsDead())
            ImpactBulletServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    void ImpactBulletServerRPC() 
    {
       gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
