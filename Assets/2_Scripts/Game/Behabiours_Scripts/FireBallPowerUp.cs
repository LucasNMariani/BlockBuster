using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallPowerUp : PowerUp
{

    [SerializeField] Spawner _spawner;
    //public ParticleSystem fireEffect;
    

    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponentInParent<DragController>();
        if (entity != null)
        {
            if (_spawner == null) _spawner = FindObjectOfType<Spawner>();
            Debug.Log("El powerUp toco al player");
            PowerUpOn();
        }
    }
    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }

    

    protected override void PowerUpOn()
    {
        if (_spawner != null)
        {
            _spawner.SpawnFire();
            //fireEffect.gameObject.SetActive(true);
            //fireEffect.Play();

        }

        else Debug.LogWarning("NO HAY SPAWNER EN ESCENA, AGREGAR EMPTYOBJECT Y COLOCARLE SCRIPT DE SPAWNER");
        Destroy(gameObject);

    }

    protected override void ResetPowerUp()
    {
        Debug.Log("Reseteando PowerUp");
    }
}
