using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletManager : GenericSingleton<BulletManager>
{
   private List<Bullet> _bullets = new List<Bullet>();
   
   private void Update()
   {
      // Update bullet position
      foreach (Bullet bullet in _bullets)
      {
         if(bullet.IsDead()) continue;

         Transform bulletTransform = bullet.transform;
         Vector3 newPosition = bulletTransform.position + bulletTransform.forward * bullet.Speed * Time.deltaTime;
         bulletTransform.position = newPosition;
      }
      
      
      // Clean Bullet 
      List<Bullet> deadBullets = _bullets.FindAll((bullet) => bullet.IsDead());
      _bullets.RemoveAll((bullet) => bullet.IsDead());
      deadBullets.ForEach(bullet => bullet.GetComponent<NetworkObject>().Despawn());
   }

   public void AddBullet(Bullet bullet)
   {
      _bullets.Add(bullet);
   }
}
