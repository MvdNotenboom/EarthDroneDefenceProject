using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerControl : MonoBehaviour
{
    [SerializeField] private GameManager s_gameManager;

    //  Controller scripts
    [SerializeField] private HandController s_handControllerR;
    [SerializeField] private HandController s_handControllerL;
    
    //  Right hand controller
    private InputDevice rightController;
    [SerializeField] private Vector2 rightJoystickVector;
    [SerializeField] private float rightGripF;
    [SerializeField] private bool rightGripB;
    [SerializeField] private float rightTriggerF;
    [SerializeField] private bool rightTriggerB;
    [SerializeField] private bool rightPrimaryButton;

    //  Left hand controller
    private InputDevice leftController;
    [SerializeField] private Vector2 leftJoystickVector;
    [SerializeField] private float leftGripF;
    [SerializeField] private bool leftGripB;
    [SerializeField] private float leftTriggerF;
    [SerializeField] private bool leftTriggerB;
    [SerializeField] private bool leftPrimaryButton;

    private void Start()
    {
        s_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        s_handControllerR = GameObject.Find("RightHand Controller").GetComponent<HandController>();
        s_handControllerL = GameObject.Find("LeftHand Controller").GetComponent<HandController>();
        SetupRightController();
        SetupLeftController();

        if (!s_gameManager.getinLargeMenuBool())
        {
            SetupDrones();
        }
    }

    void Update()
    {
        getRightJoystickValue();
        getLeftJoystickValue();

        getRightGripValue();
        getLefttGripValue();

        getRightTriggerValue();
        getLeftTriggerValue();

        getRightPrimaryButton();
        getLeftPrimaryButton();
    }

    public void SetDrone(string value)
    {
        if (rightPrimaryButton && !leftPrimaryButton)
        {
            if (value.Equals("Pavise") && s_gameManager.leftDrone.Equals("Pavise"))
            {
                //Do not have a shield drone "Pavise" enabled in current state of the game.
                //Let the user know about the incompatibility of double shield drones
            }
            else
            {
                s_handControllerR.SetDrone(value);
                s_gameManager.rightDrone = value.ToString();
                s_gameManager.updatedronecount();

            }
        }
        else if (leftPrimaryButton && !rightPrimaryButton)
        {
            if(value.Equals("Pavise") && s_gameManager.rightDrone.Equals("Pavise"))
            {
                //Do not have a shield drone "Pavise" enabled in current state of the game.
                //Let the user know about the incompatibility of double shield drones
            }
            else
            {
                s_handControllerL.SetDrone(value);
                s_gameManager.leftDrone = value.ToString();
                s_gameManager.updatedronecount();

            }
        }
        else if (leftPrimaryButton && rightPrimaryButton)
        {
        }
    }

    public void SetupDrones()
    {
        s_handControllerR.SetDrone(s_gameManager.rightDrone);
        s_handControllerL.SetDrone(s_gameManager.leftDrone);
        
    }
    public void setupTheMainMenu(bool value)
    {
        s_handControllerL.SetinMainMenu(value);
        s_handControllerR.SetinMainMenu(value);

    }


    //  Trigger value from controllers
    #region Trigger value from controllers
    private void getRightTriggerValue()
    {
        rightController.TryGetFeatureValue(CommonUsages.trigger, out float righttriggerValue);
        rightTriggerF = righttriggerValue;
        if (rightTriggerF > 0.8f)
        {
            rightTriggerB = true;
            s_handControllerR.setTrigger(true);
        }
        else
        {
            rightTriggerB = false;
            s_handControllerR.setTrigger(false);
        }
    }
    private void getLeftTriggerValue()
    {
        leftController.TryGetFeatureValue(CommonUsages.trigger, out float lefttriggerValue);
        leftTriggerF = lefttriggerValue;
        if (leftTriggerF > 0.8f)
        {
            leftTriggerB = true;
            s_handControllerL.setTrigger(true);
        }
        else
        {
            leftTriggerB = false;
            s_handControllerL.setTrigger(false);
        }
    }
    #endregion

    //  Grip value from controllers
    #region Grip value from controllers
    private void getRightGripValue()
    {
        rightController.TryGetFeatureValue(CommonUsages.grip, out float rightgripValue);
        rightGripF = rightgripValue;
        if (rightGripF > 0.8f)
        {
            rightGripB = true;
            s_handControllerR.setGrip(true);
        }
        else
        {
            rightGripB = false;
            s_handControllerR.setGrip(false);
        }
    }
    private void getLefttGripValue()
    {
        leftController.TryGetFeatureValue(CommonUsages.grip, out float leftgripValue);
        leftGripF = leftgripValue;
        if (leftGripF > 0.8f)
        {
            leftGripB = true;
            s_handControllerL.setGrip(true);
        }
        else
        {
            leftGripB = false;
            s_handControllerL.setGrip(false);
        }
    }
    #endregion

    //  Primary button value from controllers (X/A)
    #region Grip value from controllers
    private void getRightPrimaryButton()
    {
        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary);
        rightPrimaryButton = rightPrimary;
        s_handControllerR.setPrimary(rightPrimaryButton);
    }
    private void getLeftPrimaryButton()
    {
        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary);
        leftPrimaryButton = leftPrimary;
        s_handControllerL.setPrimary(leftPrimaryButton);
    }
    #endregion

    //  Joystick value from controllers
    #region Joystick value from controllers
    private void getRightJoystickValue()
    {
        rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValueRight);
        rightJoystickVector = primary2DAxisValueRight;
    }
    private void getLeftJoystickValue()
    {
        leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValueLeft);
        leftJoystickVector = primary2DAxisValueLeft;
    }
    #endregion

    //  Setup controllers
    #region Setup controllers
    private void SetupRightController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristic = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristic, devices);
        if (devices.Count > 0)
        {
            rightController = devices[0];
        }
        else
        {
            Debug.Log("ERROR: Right hand controller setup fail.");
        }
    }
    private void SetupLeftController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristic = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristic, devices);
        if (devices.Count > 0)
        {
            leftController = devices[0];
        }
        else
        {
            Debug.Log("ERROR: Left hand controller setup fail.");
        }
    }
    #endregion

}
