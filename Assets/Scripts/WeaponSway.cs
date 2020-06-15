using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [SerializeField]
    private float amount = 0.02f;
    [SerializeField]
    private float ADSAmount = 0.0001f;
    [SerializeField]
    private float maxAmount = 0.06f;
    [SerializeField]
    private float smoothAmount = 2f;

    [SerializeField]
    private float rotationAmount = 2f;
    [SerializeField]
    private float ADSRotationAmount = 0f;
    [SerializeField]
    private float maxRotationAmount = 4f;
    [SerializeField]
    private float smoothRotation = 2f;

    public bool xRotation = false;
    public bool yRotation = true;
    public bool zRotation = true;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private float inputX;
    private float inputY;
    private float currentAmount;
    private float currentRotationAmount;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        currentAmount = amount;
        currentRotationAmount = rotationAmount;
    }

    private void Update()
    {
        calculateSway();
        moveSway();
        rotateSway();
    }

    void calculateSway()
    {
        Vector2 delta = Mouse.current.delta.ReadValue();
        inputX = -delta.x;
        inputY = -delta.y;
    }

    void moveSway()
    {
        float moveX = Mathf.Clamp(inputX * currentAmount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(inputY * currentAmount, -maxAmount, maxAmount);

        Vector3 targetPosition = new Vector3(moveX, moveY, 0)+ initialPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smoothAmount * Time.deltaTime);
    }

    void rotateSway()
    {
        float moveX = Mathf.Clamp(inputX * currentRotationAmount, -maxRotationAmount, maxRotationAmount);
        float moveY = Mathf.Clamp(inputY * currentRotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion targetRoation = Quaternion.Euler(new Vector3(xRotation? -moveY:0f, yRotation? moveX:0f, zRotation? moveX:0f)) * initialRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRoation, smoothRotation * Time.deltaTime);
    }

    public void setADS(bool active)
    {
        if (active)
        {
            currentAmount = ADSAmount;
            currentRotationAmount = ADSRotationAmount;
        }
        else
        {
            currentAmount = amount;
            currentAmount = rotationAmount;
        }
    }
}
