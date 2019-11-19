using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeathBehaviour
{
    void Die();
}

public interface IMovementBehaviour
{
    float Speed { get; }

    void SetSpeed(float speed);
    void SetThreshold(float trheshold);
}
