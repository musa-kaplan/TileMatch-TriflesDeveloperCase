using System;
using UnityEngine;

namespace MusaUtils.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class MuMobileCharacter : MonoBehaviour
    {
        [HideInInspector] public bool isPortrait;
        
        [Range(0.01f, 1f)] [SerializeField] private float movementSmoothness = .05f;
        [Range(0.01f, 1f)] [SerializeField] private float rotationSmoothness = .1f;
        [SerializeField] private MobileCharacterData characterData;

        #region Local Variables

        private FloatingJoystick joystick;
        private FloatingJoystick sideJoystick;
        private Rigidbody rigidBody;
        private Vector3 currentPos;
        private Vector3 currentXAxis;
        private Vector3 currentZAxis;
        private Vector3 currentRot;
        private Vector3 currentCameraRot;
        private Camera currentCamera;

        #endregion

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (isPortrait)
            {
                SimpleMovement();
            }
            else
            {
                TwoJoystickMovement();
                Rotation();
            }
        }

        private void Rotation()
        {
            currentRot.y += sideJoystick.Horizontal * rotationSmoothness;

            currentCameraRot.x += -sideJoystick.Vertical * rotationSmoothness;
            currentCameraRot.x = Mathf.Clamp(currentCameraRot.x, -characterData.verticalLookAngle,
                characterData.verticalLookAngle);
            
            currentCamera.transform.localRotation = Quaternion.Slerp(currentCamera.transform.localRotation, 
                Quaternion.Euler(currentCameraRot), .1f);
            
            transform.localRotation = Quaternion.Slerp(transform.localRotation, 
                Quaternion.Euler(currentRot), .1f);
            
        }

        private void SimpleMovement()
        {
            currentPos.x += joystick.Horizontal * characterData.movementSpeed;
            currentPos.z += joystick.Vertical * characterData.movementSpeed;
            currentPos.y = characterData.gravity;
            
            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, currentPos, movementSmoothness);
            currentPos.x = 0;
            currentPos.z = 0;

            currentRot.x = joystick.Horizontal > .1f || 
                           joystick.Horizontal < -.1f ? joystick.Horizontal : currentRot.x;
            
            currentRot.z = joystick.Vertical > .1f || 
                           joystick.Vertical < -.1f ? joystick.Vertical : currentRot.z;
            
            transform.localRotation = Quaternion.Slerp(transform.localRotation, 
                Quaternion.LookRotation(currentRot.normalized), .1f);
        }
        
        private void TwoJoystickMovement()
        {
            currentXAxis += transform.right * (joystick.Horizontal * characterData.movementSpeed);
            currentZAxis += transform.forward * (joystick.Vertical * characterData.movementSpeed);
            currentXAxis.y = currentZAxis.y = characterData.gravity;

            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, (currentXAxis + currentZAxis), movementSmoothness);
            currentXAxis = Vector3.zero;
            currentZAxis = Vector3.zero;
        }

        private void Initialize()
        {
            rigidBody = GetComponent<Rigidbody>();
            
            if (isPortrait)
            { joystick = FindObjectOfType<FloatingJoystick>(); }
            else
            { joystick = Cookies.QuickFind("LeftJoystick").GetComponent<FloatingJoystick>();
                sideJoystick = Cookies.QuickFind("RightJoystick").GetComponent<FloatingJoystick>(); }
            
            for (var i = 0; i < transform.childCount; i++)
            { if (!transform.GetChild(i).TryGetComponent(out Camera c)) continue;
                currentCamera = c;
                break; }

            if (currentCamera == null)
            { var cam = new GameObject("Camera");
                currentCamera = cam.AddComponent<Camera>();
                currentCamera.transform.parent = transform;
                currentCamera.transform.localPosition = transform.localPosition; }
        }
    }
}
