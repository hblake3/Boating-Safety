using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BoatController : MonoBehaviour
{
    [SerializeField] GameObject boatGO;

    [Header("Movement")]
    public float xLimit = 5.5f; // the x-position boundaries the boat can move within

    [Header("Rotation")]
    public float rotationAngle = 30f;
    public float rotationpeed = 6f;       // how fast it rotates

    [Header("Speed")]
    public float[] moveSpeeds = new float[] { 2.5f, 5f, 7.5f };
    public int currentSpeedIndex = 1;
    public float moveSpeed;
    public static float CurrentSpeed { get; private set; }


    // input variables for keyboard and UI (buttons)
    // -1 left, 1 right, 0 none
    private float input;
    private float keyboardInput;
    private float uiInput;

    // to store the starting rotation of the boat
    private Quaternion baseRotation;

    // [ DELEGATES ]
    public delegate void OnSpeedChanged(float newSpeed);
    public static OnSpeedChanged onSpeedChanged;

    void Start()
    {
        baseRotation = boatGO.transform.rotation; // set the default boat rotation at game start
        moveSpeed = moveSpeeds[currentSpeedIndex]; // set the default starting speed at game start
        CurrentSpeed = moveSpeed;
        BroadcastSpeed(); // ensure the speed UI is updated to the default starting speed upon game start
    }

    void Update()
    {
        ReadKeyboardInput();

        // UI input overrides keyboard if present
        input = uiInput != 0f ? uiInput : keyboardInput;

        HandleMovement();
        HandleRotation();
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
        pos.x += input * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        boatGO.transform.position = pos;
    }

    private void HandleRotation()
    {
        // If we're at the boundaries and still trying to move, cancel turning and return to base rotation
        bool pushingLeftIntoWall = (input < 0f && boatGO.transform.position.x <= -xLimit);
        bool pushingRightIntoWall = (input > 0f && boatGO.transform.position.x >= xLimit);

        float effectiveInput;

        // If the boat is at the left boundary and the player is still pressing left
        // or if the boat is at the right boundary and the player is still pressing right
        // then cancel the input so the boat does not keep turning

        if ((pushingLeftIntoWall && input < 0f) ||
            (pushingRightIntoWall && input > 0f))
        {
            effectiveInput = 0f;
        }
        else
        {
            // Otherwise, allow the input to affect rotation normally
            effectiveInput = input;
        }

        // Rotate the boat axis
        Quaternion rotateOffset = Quaternion.AngleAxis(effectiveInput * rotationAngle, boatGO.transform.up);
        Quaternion targetRotation = rotateOffset * baseRotation;

        boatGO.transform.rotation = Quaternion.Slerp(boatGO.transform.rotation, targetRotation, rotationpeed * Time.deltaTime);
    }

    public void PressLeft()
    {
        Debug.Log("Left Pressed");
        uiInput = -1f;
    }

    public void PressRight()
    {
        uiInput = 1f;
    }

    public void Release()
    {
        uiInput = 0f;
    }

    public void PressSpeedUp()
    {
        if(currentSpeedIndex >= moveSpeeds.Length - 1)
        {
            Debug.Log("Already at max speed!"); // ** TEST CAN GO HERE **
            return; 
        }
        currentSpeedIndex++;
        moveSpeed = moveSpeeds[currentSpeedIndex];
        BroadcastSpeed();
    }

    public void PressSpeedDown()
    {
        if (currentSpeedIndex <= 0)
        {
            Debug.Log("Already at min speed!"); // ** TEST CAN GO HERE **
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
