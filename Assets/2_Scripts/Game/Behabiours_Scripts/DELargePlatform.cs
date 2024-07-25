using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELargePlatform : PowerUp
{
          Animator _anim;

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponentInParent<DragController>();
            if (entity != null)
            {
                _anim = entity.GetComponent<Animator>();
                //_anim = GameObject.Find("Player").GetComponent<Animator>();
                Debug.Log("El powerUp toco al player");
                AudioManager.Instance.PlaySFX("DebuffSound");
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
            _anim.SetBool("DePowerUpOn", true);
            Invoke("ResetPowerUp", _resetTime);
            gameObject.SetActive(false);
        }

        protected override void ResetPowerUp()
        {
            _anim.SetBool("DePowerUpOn", false);
            Destroy(gameObject);
        }
    }
