using UnityEngine;

public interface IWeapon
{
    int Level { get; }
    float Damage { get; }
    float Weight { get; }
}
