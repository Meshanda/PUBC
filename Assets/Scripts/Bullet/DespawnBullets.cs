using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnBullets : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            bullet.Life = 0;
        }
    }
}
