using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlyweightPointer
{
    public readonly static Flyweight BrickMaxLife = new Flyweight
    {
        maxLifeBrick = 1f,
    };
    public readonly static Flyweight BasicBall = new Flyweight
    {
        speedBall = 50f,
        damageAmountBall = 1f,
    };
}