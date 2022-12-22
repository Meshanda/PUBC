using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using Unity.Netcode;
using UnityEngine;

public class PowerUp : NetworkBehaviour
{
    [SerializeField] private bool HealthBoost;
    [SerializeField] private bool ProjectileBoost;


    //TODO CLEAAAAANNNNNEEEEEE;
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
        {
            return;
        }

        if (other != null && other.gameObject.tag is "Player")
        {
            if (HealthBoost)
            {
                Debug.Log("heal");
                other.gameObject.GetComponent<Health>().Heal(50);
            }
            else if (ProjectileBoost)
            {
                Debug.Log("damage");
                //other.gameObject.GetComponent<Health>().Heal(-50);

                //marche pas ca
                other.gameObject.GetComponent<PlayerWeaponsManager>().GetActiveWeapon().BulletsPerShot.Value = 3;

            }

            DestroyPowerUp();
        }
    }

    void DestroyPowerUp()
    {
        //AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        NetworkObject.Despawn(true);
    }

}

