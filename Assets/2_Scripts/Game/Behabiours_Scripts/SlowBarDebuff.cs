using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBarDebuff : PowerUp
{
    DragController _controller;
    [SerializeField, Range(0, 1)] private float _slowAmount = .3f;
    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        _controller = other.GetComponentInParent<DragController>();
        if (_controller != null)
        {
            Debug.Log("El powerUp toco al player");
            AudioManager.Instance.PlaySFX("DebuffSound");
            PowerUpOn();
        }
    }
    protected override void PowerUpOn()
    {
        _controller.Multiplier = _slowAmount;
        Invoke("ResetPowerUp", _resetTime);
        gameObject.SetActive(false);
    }

    protected override void ResetPowerUp()
    {
        _controller.Multiplier = 1;
        Destroy(gameObject);
    }
}
