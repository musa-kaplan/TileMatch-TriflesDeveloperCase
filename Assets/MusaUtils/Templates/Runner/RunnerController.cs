using MusaUtils.RigidBody;
using UnityEngine;

namespace MusaUtils.Runner
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class RunnerController : MonoBehaviour
    {
        public bool _jumpable;
        [Range(1f, 20f)] public float speed;
        [Range(1f, 20f)] public float sensitivity;
        [Range(1f, 20f)] public float jumpForce;

        private FloatingJoystick _joystick;
        private Rigidbody _body;
        private Vector3 _velocity;
        private bool _jump;

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
            _joystick = FindObjectOfType<FloatingJoystick>();
            QuickBody.GetRigid(_body).FreezeRotation(true, false);
            QuickBody.GetRigid(_body).FreezePosition(false, !_jumpable, false);
            _velocity.y = jumpForce;
        }

        private void Update()
        {
            QuickBody.GetRigid(_body).GoForward(transform.forward, speed);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, 
                Quaternion.Euler(0, _joystick.Horizontal * sensitivity, 0), .15f);

            if(!_jumpable) return;
            if (_joystick.Vertical > .35f)
            {
                Jump();
            }
        }

        private void Jump()
        {
            if (_jump) return;
            _body.AddForceAtPosition(_velocity, transform.position);
            _jump = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _jump = false;
            }
        }
    }
}