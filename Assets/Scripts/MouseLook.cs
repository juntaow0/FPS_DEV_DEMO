using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float _sensitivity = 1f;
    [SerializeField]
    private float m_yaw = 0.022f;
    [SerializeField]
    private float m_pitch = 0.022f;
    private float _pitch = 0f;
    private float _yaw = 0f;
    private PlayerInputClass _pi;
    private Transform _cameras;
    private Vector2 lookInput;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _pi = new PlayerInputClass();
        _cameras = transform.Find("CameraHolder");
        if (_cameras == null)
        {
            Debug.LogError("Camera holder not attached!");
        }
    }

    private void Update()
    {
        lookInput = _pi.Player.Look.ReadValue<Vector2>();
        float mouseX = lookInput.x * m_yaw * _sensitivity;
        float mouseY = lookInput.y * m_pitch * _sensitivity;
        _pitch -= mouseY;
        _yaw += mouseX;
        _pitch = Mathf.Clamp(_pitch, -90, 90);
        Vector3 horizontal = transform.localEulerAngles;
        Vector3 vertical = _cameras.localEulerAngles;
        horizontal.y = _yaw;
        vertical.x = _pitch;
        transform.localEulerAngles = horizontal;
        _cameras.localEulerAngles = vertical;
        //_pitch -= mouseY;
        //_pitch = Mathf.Clamp(_pitch, -90f, 90f);
        //transform.Rotate(Vector3.up * mouseX);
        //_cameras.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void OnEnable()
    {
        _pi.Player.Look.Enable();
    }
    private void OnDisable()
    {
        _pi.Player.Look.Disable();
    }

    public float getSensitivity()
    {
        return _sensitivity;
    }

    public void setSensitivity(float value)
    {
        _sensitivity = value;
    }
}
