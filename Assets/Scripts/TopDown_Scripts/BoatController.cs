using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BoatController : MonoBehaviour
{
    [SerializeField] GameObject boatGO;

    [Header("Movement")]
    public float xLimit = 5.5f;

    [Header("Rotation")]
    public float rotationAngle = 30f;
    public float rotationpeed = 6f;

    [Header("Speed")]
    public float[] moveSpeeds = new float[] { 3.5f, 5f, 7.5f };
    public int currentSpeedIndex = 1;
    public float moveSpeed;
    public static float CurrentSpeed { get; private set; }

    public float boatSpeedMultiplier = 1.5f;

    private float input;
    private float keyboardInput;
    private float uiInput;

    private Quaternion baseRotation;

    [Header("Control Lock")]
    [SerializeField] private bool controlsLocked = false;

    // [ DELEGATES ]
    public delegate void OnSpeedChanged(float newSpeed);
    public static OnSpeedChanged onSpeedChanged;

    void Start()
    {
        baseRotation = boatGO.transform.rotation;
        moveSpeed = moveSpeeds[currentSpeedIndex];
        CurrentSpeed = moveSpeed;
        BroadcastSpeed();
    }

    void Update()
    {
        if (controlsLocked)
        {
            keyboardInput = 0f;
            uiInput = 0f;
            input = 0f;

            HandleRotation();
            return;
        }

        ReadKeyboardInput();

        input = uiInput != 0f ? uiInput : keyboardInput;

        HandleMovement();
        HandleRotation();
    }

    public float[] GetMoveSpeeds()
    {
        return moveSpeeds;
    }

    public void LockControls()
    {
        controlsLocked = true;
        keyboardInput = 0f;
        uiInput = 0f;
        input = 0f;
    }

    public void UnlockControls()
    {
        controlsLocked = false;
    }

    public bool AreControlsLocked()
    {
        return controlsLocked;
    }

    private void ReadKeyboardInput()
    {
        keyboardInput = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            keyboardInput = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            keyboardInput = 1f;
    }

    private void HandleMovement()
    {
        Vector3 pos = boatGO.transform.position;
        pos.x += input * moveSpeed * Time.deltaTime * boatSpeedMultiplier;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        boatGO.transform.position = pos;
    }

    private void HandleRotation()
    {
        bool pushingLeftIntoWall = (input < 0f && boatGO.transform.position.x <= -xLimit);
        bool pushingRightIntoWall = (input > 0f && boatGO.transform.position.x >= xLimit);

        float effectiveInput;

        if ((pushingLeftIntoWall && input < 0f) ||
            (pushingRightIntoWall && input > 0f))
        {
            effectiveInput = 0f;
        }
        else
        {
            effectiveInput = input;
        }

        Quaternion rotateOffset = Quaternion.AngleAxis(effectiveInput * rotationAngle, boatGO.transform.up);
        Quaternion targetRotation = rotateOffset * baseRotation;

        boatGO.transform.rotation = Quaternion.Slerp(boatGO.transform.rotation, targetRotation, rotationpeed * Time.deltaTime);
    }

    public void PressLeft()
    {
        if (controlsLocked) return;
        uiInput = -1f;
    }

    public void PressRight()
    {
        if (controlsLocked) return;
        uiInput = 1f;
    }

    public void Release()
    {
        uiInput = 0f;
    }

    public void PressSpeedUp()
    {
        if (controlsLocked) return;

        if (currentSpeedIndex >= moveSpeeds.Length - 1)
        {
            Debug.Log("Already at max speed!");
            return;
        }

        currentSpeedIndex++;
        moveSpeed = moveSpeeds[currentSpeedIndex];
        BroadcastSpeed();
    }

    public void PressSpeedDown()
    {
        if (controlsLocked) return;

        if (currentSpeedIndex <= 0)
        {
            Debug.Log("Already at min speed!");
            return;
        }

        currentSpeedIndex--;
        moveSpeed = moveSpeeds[currentSpeedIndex];
        BroadcastSpeed();
    }

    private void BroadcastSpeed()
    {
        CurrentSpeed = moveSpeed;
        onSpeedChanged?.Invoke(moveSpeed);
    }
}