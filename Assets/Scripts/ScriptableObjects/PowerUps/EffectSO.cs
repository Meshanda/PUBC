using UnityEngine;

public abstract class EffectSO<T> : ScriptableObject
{
    public PlayerShootData.ShootEnum Type;
    public T Value;
}
