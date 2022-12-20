using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObjectListVariable _players;
    [SerializeField] private float _timeBetweenTick;
    [SerializeField] private float _damage;
    private float _time;
    void Start()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        _time -= Time.deltaTime;

        if (other.gameObject.tag != "Player" || _time > 0)
            return;
        _time = _timeBetweenTick;

        PlayerHpSetter hp = other.transform.parent.GetComponent<PlayerHpSetter>();

        if (hp == null)
            return;

        hp.TakeDamage(_damage);
    }
}
