using System.Collections;
using UnityEngine;

public class FireBallBehabiour : BallBehabiour
{
    [SerializeField, Range(5,15)] float _timerToDestroy;
    private ObjectPool<FireBallBehabiour> _pool;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(DestroyFireBallTimer());
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bounce");
        AudioManager.Instance.PlaySFX("BallSound");

        var brickCollision = collision.gameObject.GetComponent<Bricks>();
        if (brickCollision is IndestructableBricks indestructable)
        {
            if (indestructable != null)
            {
                if (!indestructable.canDestroy) indestructable.CantDestroy();
                else indestructable.Damage(FlyweightPointer.BasicBall.damageAmountBall);
            }
        }
        else if(brickCollision is Bricks)
        {
            brickCollision.Damage(brickCollision.MaxLife);
            _rigidbody.velocity = _velocity * FlyweightPointer.BasicBall.speedBall;
            return;
        }

        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.GetContact(0).normal);
    }

    IEnumerator DestroyFireBallTimer()
    {
        yield return new WaitForSeconds(_timerToDestroy);
        Destroy();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public void Create(ObjectPool<FireBallBehabiour> pool)
    {
        _pool = pool;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = -Vector3.right * FlyweightPointer.BasicBall.speedBall;
    }
}
