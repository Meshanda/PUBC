using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Variables/GameObjects", fileName = "New GameObjects")]
public class GameObjectListVariable : ScriptableObject
{
    public List<GameObject> gos;

   public void AddGameObject(GameObject go) 
    {
        if (!NetworkManager.Singleton.IsServer)
            return;
        if (gos == null)
            gos = new List<GameObject>();
        for (int i = gos.Count - 1; i >= 0; i--)
        {
            if (gos[i] == null)
            {
                gos[i] = gos[gos.Count - 1];
                gos.RemoveAt(gos.Count - 1);
            }
        }
        gos.Add(go);
    }

    public List<GameObject> GetGos() { return gos; }
    public void SetGos(List<GameObject> gos) { this.gos = gos; }
}
