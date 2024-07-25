using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected float _resetTime = 10, _speed;

    protected abstract void PowerUpOn();
    protected abstract void ResetPowerUp();
}
