using Unity.VisualScripting;
using UnityEngine;

public class BallBehabiour : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigidbody;
    protected Vector3 _velocity;
    [SerializeField] protected LayerMask _loseLimitLayer;
    [SerializeField] [Range(-20f, 5f)] protected float _barOffsetToTheStart;
    [SerializeField] protected bool isTestingWithoutInputs = false;
    protected Spawner _spawner;

    private ObjectPool<BallBehabiour> _pool;

    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spawner = FindObjectOfType<Spawner>();

        //GameManager.instance.AddBall(this);
        //EventManager.Subscribe(TypeEvent.ResetLevel, ResetLevelBall);
        if (isTestingWithoutInputs) _rigidbody.velocity = -Vector3.right * FlyweightPointer.BasicBall.speedBall;
    }

    protected virtual void Update()
    {
        if (!GameManager.instance.isGameStarted)
        {
            //Se iguala la posicion de la pelota con la barra con un offset en Y
            
            transform.position = new Vector3(GameManager.instance.gameBarPos.position.x + _barOffsetToTheStart, 0, GameManager.instance.gameBarPos.position.z);
            if (Input.touchCount <= 0) return;
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                Debug.Log("Comienza el juego");
                GameManager.instance.SetIfGameIsStarted(true);
                
                _rigidbody.velocity = -Vector3.right * FlyweightPointer.BasicBall.speedBall;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized * FlyweightPointer.BasicBall.speedBall;
        _velocity = _rigidbody.velocity;
    }

    protected virtual void ResetLevelBall(params object[] parameters)
    {
        GameManager.instance.SetIfGameIsStarted(false);
        _rigidbody.velocity = Vector3.zero;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bounce");
        AudioManager.Instance.PlaySFX("BallSound");
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.GetContact(0).normal);

        var brickCollision = collision.gameObject.GetComponent<Bricks>();

        if (brickCollision is IndestructableBricks indestructable)
        {
            if (!indestructable.canDestroy) indestructable.CantDestroy();
            else indestructable.Damage(FlyweightPointer.BasicBall.damageAmountBall);
        }
        else if (brickCollision is Bricks) brickCollision.Damage(FlyweightPointer.BasicBall.damageAmountBall);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BallLimit") Destroy();
    }

    public void Create(ObjectPool<BallBehabiour> pool)
    {
        _pool = pool;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = -Vector3.right * FlyweightPointer.BasicBall.speedBall;
    }

    public virtual void Destroy()
    {
        GameManager.instance.RemoveBall(this);
        Debug.Log("Ball destroyed");
        if (_pool == null) { Destroy(gameObject); return; }
            _pool.ReturnObject(this);
    }

    public static void TurnOn(BallBehabiour ball)
    {
        ball.gameObject.SetActive(true);
        GameManager.instance.AddBall(ball);
    }

    public static void TurnOff(BallBehabiour ball)
    {
        ball.gameObject.SetActive(false);
    }
}