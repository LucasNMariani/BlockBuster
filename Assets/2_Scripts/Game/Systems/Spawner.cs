using UnityEngine;
//using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    //Hacerlo generico si es posible

    public BallBehabiour _objectToSpawn;
    public FireBallBehabiour _fireBallReference;
    ObjectPool<BallBehabiour> _pool;
    ObjectPool<FireBallBehabiour> _firePool;
    [SerializeField] int _maxValueOfObjectsInScene = 5;

    private void Start()
    {
        _pool = new ObjectPool<BallBehabiour>(Factory, BallBehabiour.TurnOn, BallBehabiour.TurnOff, _maxValueOfObjectsInScene);
        _firePool = new ObjectPool<FireBallBehabiour>(FactoryFire, BallBehabiour.TurnOn, BallBehabiour.TurnOff, _maxValueOfObjectsInScene);
        Spawn();
    }

    //Prender los objetos
    public void Spawn()
    {
        var o = _pool.GetObject();
        o.Create(_pool);
        o.transform.position = new Vector3(GameManager.instance.gameBarPos.position.x + -20,0, GameManager.instance.gameBarPos.position.z);
        o.transform.forward = transform.forward;
    }

    public void SpawnFire()
    {
        var o = _firePool.GetObject();
        o.Create(_firePool);
        o.transform.position = new Vector3(GameManager.instance.gameBarPos.position.x + -20, 0, GameManager.instance.gameBarPos.position.z);
        o.transform.forward = transform.forward;
    }

    //Instanciar al comienzo
    public BallBehabiour Factory()
    {
        //GameManager.instance.AddBall(_objectToSpawn);
        return Instantiate(_objectToSpawn);
    }

    public FireBallBehabiour FactoryFire()
    {
        //GameManager.instance.AddBall(_fireBallReference);
        return Instantiate(_fireBallReference);
    }
}
