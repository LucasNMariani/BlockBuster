using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragController : MonoBehaviour
{

    [SerializeField] private Vector3 _inicialPos;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxZ;
    [SerializeField] private float _speed = 50;
    [SerializeField] private float _multiplier = 1;
    public float Multiplier { get { return _multiplier; } set { _multiplier = value; } }
    private bool _isTouching;
    private Vector3 _touchStartPosition;
    private Vector3 _objectStartPosition;
    [SerializeField] private Animator _animator;

    [SerializeField] VirtualJoystick _virtualJoystick;
    Action MovementController;

    private void Awake()
    {
        _inicialPos = transform.position;
    }

    private void Start()
    {
        //EventManager.Subscribe(TypeEvent.ResetLevel, ResetGameBar);
        _animator.GetComponent<Animator>();
        MovementController = delegate { };
        switch (GameManager.instance.controls)
        {
            case Controls.Multitouch: MovementController += TouchInputMovement; break;
            case Controls.Buttons: MovementController += ButtonSystemControl; break;
            case Controls.VirtualJoystick:
                MovementController += JoystickControl;
                if (_virtualJoystick == null) _virtualJoystick = FindObjectOfType<VirtualJoystick>();
                break;
        }
    }

    void Update()
    {
        if (GameManager.instance.IsInPause()) return;
        MovementController();
    }

    #region Touch Control


    void TouchInputMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _touchStartPosition = touch.position;
                _objectStartPosition = transform.position;
                _isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (_isTouching)
                {
                    float touchDelta = touch.position.x - _touchStartPosition.x;
                    float objectZPosition = _objectStartPosition.z + touchDelta / Screen.width * (_speed * Multiplier);
                    objectZPosition = Mathf.Clamp(objectZPosition, _minZ, _maxZ);
                    _animator.SetFloat("DeltaPos", touchDelta);
                    transform.position = new Vector3(transform.position.x, transform.position.y, objectZPosition);
                    //Debug.Log(objectXPosition);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _isTouching = false;
                _animator.SetFloat("DeltaPos", 0);
            }
        }
    }

    #endregion

    #region Acelerometro Control (unused)

    void AccelerometerInputControl()
    {
        float accelerationX = Input.acceleration.x;
        float objectXPos = transform.position.x + accelerationX * (_speed * Multiplier);

        objectXPos = Mathf.Clamp(objectXPos, _minZ, _maxZ);

        transform.position = new Vector3(objectXPos, transform.position.y, transform.position.z);
    }

    #endregion

    #region Buttons Control

    bool _movingLeft, _movingRight;
    void ButtonSystemControl()
    {

        float direction = 0f;

        if (_movingLeft) direction = -1f;
        else if (_movingRight) direction = 1f;

        float objectZPos = transform.position.z + direction * (_speed * Multiplier) * Time.deltaTime;
        objectZPos = Mathf.Clamp(objectZPos, _minZ, _maxZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, objectZPos);

        _animator.SetFloat("DeltaPos", direction);
    }

    public void OnLeftButtonDown()
    {
        _movingLeft = true;
    }

    public void OnLeftButtonUp()
    {
        _movingLeft = false;
    }

    public void OnRightButtonDown()
    {
        _movingRight = true;
    }

    public void OnRightButtonUp()
    {
        _movingRight = false;
    }

    #endregion

    #region Virtual Joystick Control



    void JoystickControl()
    {
        if (_virtualJoystick == null) return;

        float horizontalInput = _virtualJoystick.GetInputVector();



        Vector3 moveDir = new Vector3(0f, 0f, horizontalInput);
        transform.Translate(moveDir * _speed * Time.deltaTime);

        float clampedZ = Mathf.Clamp(transform.position.z, _minZ, _maxZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);

        _animator.SetFloat("DeltaPos", horizontalInput);
    }

    #endregion

    public void ResetGameBar(params object[] parameters)
    {
        transform.position = _inicialPos;
    }

    public void ChangeSpeedValue(float speedMultiplier)
    {
        _speed = _speed * speedMultiplier;
    }

}
