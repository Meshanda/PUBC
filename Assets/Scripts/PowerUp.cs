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
        if (other.gameObject.tag is "Player")
        {
            if (HealthBoost)
            {
                other.gameObject.GetComponent<Health>().Heal(50);
            }
            else if(ProjectileBoost)
            {
                other.gameObject.GetComponent<PlayerWeaponsManager>().GetActiveWeapon().BulletsPerShot.Value = 3;
            }
        }
    }
}

