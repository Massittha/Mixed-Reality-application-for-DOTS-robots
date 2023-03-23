// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
//ROS Mod
using Unity.Robotics.ROSTCPConnector;
using Angular = RosMessageTypes.Geometry.Vector3Msg;

namespace Microsoft.MixedReality.Toolkit.Experimental.Joystick
{
    /// <summary>
    /// Example script to demonstrate joystick control in sample scene
    /// </summary>
    public class CmdVelRotate : MonoBehaviour
    {
        [Experimental]
        [SerializeField, FormerlySerializedAs("objectToManipulate")]
        [Tooltip("The large or small game object that receives manipulation by the joystick.")]
        private GameObject targetObject = null;
        public GameObject TargetObject
        {
            get => targetObject;
            set => targetObject = value;
        }

        [SerializeField]
        [Tooltip("A TextMeshPro object that displays joystick values.")]
        private TextMeshPro debugText = null;

        [SerializeField]
        [Tooltip("The joystick mesh that gets rotated when this control is interacted with.")]
        private GameObject joystickVisual = null;

        [SerializeField]
        [Tooltip("The mesh + collider object that gets dragged and controls the joystick visual rotation.")]
        private GameObject grabberVisual = null;

        [SerializeField]
        [Tooltip("Toggles on / off the GrabberVisual's mesh renderer because it can be dragged away from the joystick visual, it kind of breaks the illusion of pushing / pulling a lever.")]
        private bool showGrabberVisual = true;

        [Tooltip("The speed at which the JoystickVisual and GrabberVisual move / rotate back to a neutral position.")]
        [Range(1, 20)]
        public float ReboundSpeed = 5;

        [Tooltip("How sensitive the joystick reacts to dragging left / right. Customize this value to get the right feel for your scenario.")]
        [Range(0.01f, 10)]
        public float SensitivityLeftRight = 3;

        [Tooltip("How sensitive the joystick reacts to pushing / pulling. Customize this value to get the right feel for your scenario.")]
        [Range(0.01f, 10)]
        public float SensitivityForwardBack = 6;

        [SerializeField]
        [Tooltip("The property that the joystick manipulates.")]
        private JoystickMode mode = JoystickMode.Move;

        public JoystickMode Mode
        {
            get => mode;
            set => mode = value;
        }

        [Tooltip("The distance multiplier for joystick input. Customize this value to get the right feel for your scenario.")]
        [Range(0.0003f, 0.03f)]
        public float MoveSpeed = 0.01f;

        [Tooltip("The rotation multiplier for joystick input. Customize this value to get the right feel for your scenario.")]
        [Range(0.01f, 1f)]
        public float RotationSpeed = 0.05f;

        [Tooltip("The scale multiplier for joystick input. Customize this value to get the right feel for your scenario.")]
        [Range(0.00003f, 0.003f)]
        public float ScaleSpeed = 0.001f;

        private Vector3 startPosition;
        private Vector3 joystickGrabberPosition;
        private Vector3 joystickVisualRotation;
        private const int joystickVisualMaxRotation = 80;
        private bool isDragging = false;
        
        //ROS Mod
        ROSConnection ros;
        public static Angular angular;


        private void Start()
        {
            startPosition = grabberVisual.transform.localPosition;
            if (grabberVisual != null)
            {
                grabberVisual.GetComponent<MeshRenderer>().enabled = showGrabberVisual;
            }

            // ROS Mod
            // start the ROS connection

        }

        private void Update()
        {
            if (!isDragging)
            {
                // when dragging stops, move joystick back to idle
                if (grabberVisual != null)
                {
                    grabberVisual.transform.localPosition = Vector3.Lerp(grabberVisual.transform.localPosition, startPosition, Time.deltaTime * ReboundSpeed);
                }


            }
            CalculateJoystickRotation();
            ApplyJoystickValues();
        }

        private void CalculateJoystickRotation()
        {
            joystickGrabberPosition = grabberVisual.transform.localPosition - startPosition;
            // Left Right = Horizontal
            joystickVisualRotation.z = Mathf.Clamp(-joystickGrabberPosition.x * SensitivityLeftRight, -joystickVisualMaxRotation, joystickVisualMaxRotation);
            // Forward Back = Vertical
            joystickVisualRotation.x = Mathf.Clamp(joystickGrabberPosition.z * SensitivityForwardBack, -joystickVisualMaxRotation, joystickVisualMaxRotation);
            
            
            // TODO: calculate joystickVisualRotation.y to always face the proper direction (for when the joystick container gets moved around the scene)
            if (joystickVisual != null)
            {
                joystickVisual.transform.localRotation = Quaternion.Euler(joystickVisualRotation);
            }
        }

        private void ApplyJoystickValues()
        {
            if (TargetObject != null)
            {
                //ROS Mod
               

                if (Mode == JoystickMode.Move)
                {
      

                }
                else if (Mode == JoystickMode.Rotate)
                {
                    Vector3 newRotation = TargetObject.transform.rotation.eulerAngles;
                    // only take the horizontal axis from the joystick
                    newRotation.y += (joystickGrabberPosition.x * RotationSpeed);
                    newRotation.x = 0;
                    newRotation.z = 0;

                    angular = new Angular(0.0f, 0.0f, -joystickGrabberPosition.x * 0.8f);
                    //ROS Mod
                    if (debugText != null)
                    {
                        if (TargetObject.transform.localRotation.eulerAngles.y >= 180)
                        {
                            debugText.text = (360 - TargetObject.transform.localRotation.eulerAngles.y).ToString();
                        }
                        else if (TargetObject.transform.localRotation.eulerAngles.y < 180)
                        {
                            debugText.text = (-TargetObject.transform.localRotation.eulerAngles.y).ToString();
                        }
                        else if (TargetObject.transform.localRotation.eulerAngles.y == 0)
                        {
                            debugText.text = (TargetObject.transform.localRotation.eulerAngles.y).ToString();
                        }
                    }
                }
                else if (Mode == JoystickMode.Scale)
                {
                }
            }
        }
        /// <summary>
        /// The ObjectManipulator script uses this to determine when the joystick is grabbed.
        /// </summary>
        public void StartDrag()
        {
            isDragging = true;
        }
        /// <summary>
        /// The ObjectManipulator script uses this to determine when the joystick is released.
        /// </summary>
        public void StopDrag()
        {
            isDragging = false;
        }
        /// <summary>
        /// Set the joystick mode from a UI button.
        /// </summary>
        public void JoystickModeRotate()
        {
            Mode = JoystickMode.Rotate;
        }
    }
}
