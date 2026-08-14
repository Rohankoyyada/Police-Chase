using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PoliceCarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider fLWheelCollider;
    public WheelCollider fRWheelCollider;
    public WheelCollider rLWheelCollider;
    public WheelCollider rRWheelCollider;

    [Header("Wheel Transform")]
    public Transform fLWheelTransform;
    public Transform fRWheelTransform;
    public Transform rLWheelTransform;
    public Transform rRWheelTransform;

    [Header("Car Engine")]
    public float accelerationForce = 300f;
    public float breakingForce = 3000f;
    private float presentAccelerationForce=0f;
    

    //Car Steering
    public float wheelTorque = 35f;
    private float presentTorque=0f;

    //Player Input - New Input System
    public InputActionAsset playerInput;
    private float moveInput = 0f;
    private float steerInput = 0f;

    private InputAction forwardAction;
    private InputAction backwardAction;
    private InputAction leftAction;
    private InputAction rightAction;
   

    public BrakeLightController brakeLightController;

    [Header("Tyre Properties")]
    public TrailRenderer[] tyre_Marks;  
    bool drift = false;


    private void Awake()
    {
        //Assigning the New input action map Car to the player_car
        var carMap = playerInput.FindActionMap("Car");

        //Accessing the actions such as forward action etc which are present in the player_car
        forwardAction = carMap.FindAction("Up");
        backwardAction = carMap.FindAction("Down");
        leftAction = carMap.FindAction("Left");
        rightAction = carMap.FindAction("Right");
       // brakeAction = carMap.FindAction("Brake");
    }

    private void OnEnable()
    {
        //Enabling the Car Input action
        playerInput.FindActionMap("Car").Enable();
    }

    private void OnDisable()
    {
        //Disabling the Car Input action
        playerInput.FindActionMap("Car").Disable();
    }

    
    void FixedUpdate()
    {
        // Read values (0 or 1)
        //Calculating the move input value and steer input value based on Actions 
        //Ternary Operator that is (condition:if true:if false )
        moveInput = (forwardAction.IsPressed() ? 1 : 0) +
                    (backwardAction.IsPressed() ? -1 : 0);

        steerInput = (rightAction.IsPressed() ? 1 : 0) +
                     (leftAction.IsPressed() ? -1 : 0);

        Move();
        CarSteering();

        UpdateWheelVisuals();
        HandleReverseLights();
        CheckDrift();
        
    }

    //Moving front and back with the move function
    private void Move()
    {
        presentAccelerationForce = accelerationForce * moveInput;

        //Based on move_Input value the ,if it is true the wheelcolliders added to that torque figure
        fLWheelCollider.motorTorque = presentAccelerationForce;
        fRWheelCollider.motorTorque = presentAccelerationForce;
        rLWheelCollider.motorTorque = presentAccelerationForce;
        rRWheelCollider.motorTorque = presentAccelerationForce;
    }

    //Steering input for the car front wheels
    private void CarSteering()
    {
        presentTorque = wheelTorque * steerInput;

        //Based on the steering input , the steering angle is applied to the front wheels
        fLWheelCollider.steerAngle = presentTorque;
        fRWheelCollider.steerAngle = presentTorque;
    }


    
    // NEW FUNCTION: ROTATES WHEEL MESHES
    
    private void UpdateWheelVisuals()
    {
        UpdateSingleWheel(fLWheelCollider, fLWheelTransform);
        UpdateSingleWheel(fRWheelCollider, fRWheelTransform);
        UpdateSingleWheel(rLWheelCollider, rLWheelTransform);
        UpdateSingleWheel(rRWheelCollider, rRWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider col, Transform trans)
    {
        Vector3 pos;//To store the wheel mesh position
        Quaternion rot;//To store the wheel mesh rotation

        //Getting the wheel collider position and rotation and storing in the pos and rot variables
        col.GetWorldPose(out pos, out rot);

        //Updating the wheel transform position
        trans.position = pos;

        // Keep original local rotation as offset
        //We are adding this because the wheels are rotated 90 degrees
        Quaternion meshOffset = Quaternion.Euler(0, 90, 0);

        // Apply collider rotation + mesh’s original rotation
        //Updating the wheel transform rotation
        trans.rotation = rot * meshOffset;
    }

    private void HandleReverseLights()
    {
        bool isReversing = backwardAction.IsPressed();

        // Brake light
        brakeLightController.SetBrakeLight(isReversing);

        // Enable drift only if reversing AND car is moving
        drift = isReversing && GetCarSpeed() > 3f;

        //Tyre Smoke
       
        
    }

    private void CheckDrift()
    {
       
        if(drift)
        {
            StartEmitter();
        }
        else
        {
            StopEmitter();
        }
    }

    private void StartEmitter()
    {
        foreach(TrailRenderer T in tyre_Marks)
        {
            T.emitting = true;
          

        }
    }

    private void StopEmitter()
    {
        foreach (TrailRenderer T in tyre_Marks)
        {
            T.emitting = false;
           
        }
    }

    private float GetCarSpeed()
    {
        // Rigidbody speed in km/h
        return GetComponent<Rigidbody>().linearVelocity.magnitude * 3.6f;
    }


}
