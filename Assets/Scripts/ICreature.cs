using UnityEngine;

public interface ICreature
{
    int Level { get; }
    float Health { get; }
    float MoveSpeed { get; }
    float MaxSwingSpeed { get; }
    float SwingAcceleration { get; }
    float Weight { get; }
    HingeJoint2D Joint { get; set; }

    void TakeDamage(float incomingDamage);
}