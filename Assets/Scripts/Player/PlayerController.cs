using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera Movement")]
        private float _yaw;
        private float _pitch;
        
        [SerializeField] private float yawRotationalSpeed = 360.0f;
        [SerializeField] private float pitchRotationalSpeed = 180.0f;
        [SerializeField] private float minPitch = -90.0f;
        [SerializeField] private float maxPitch = 90.0f;
        [SerializeField] private Transform pitchControllerTransform;
        [SerializeField] private bool invertedYaw;
        [SerializeField] private bool invertedPitch;

        [Header("Player Movement")]
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private float speedMultiplier = 1.0f;
        private CharacterController _characterController;
        private const KeyCode LeftKeyCode = KeyCode.A;
        private const KeyCode RightKeyCode = KeyCode.D;
        private const KeyCode UpKeyCode = KeyCode.W;
        private const KeyCode DownKeyCode = KeyCode.S;
        private const KeyCode SprintKeyCode = KeyCode.LeftShift;
        private const KeyCode JumpKeyCode = KeyCode.Space;
        private const KeyCode ReloadKeyCode = KeyCode.R;
        
        [Header("Shooting")] 
        [SerializeField] private float timeBetweenShots = 0.3f;
        private float _shootingTimer;
        private Weapon _weapon;

        //Gravity
        private float _verticalSpeed;
        private bool _onGround;

        //Cursor
    #if UNITY_EDITOR
        private const KeyCode DebugLockAngleKeyCode = KeyCode.I;
        private const KeyCode DebugLockKeyCode = KeyCode.O;
        private bool _angleLocked = true;
    #endif
    
        private void Awake()
        {
            _yaw = transform.rotation.eulerAngles.y;
            _pitch = pitchControllerTransform.localRotation.eulerAngles.x;

            _characterController = GetComponent<CharacterController>();
            _weapon = GetComponent<Weapon>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (GameManager.gameManager.hudController.isPause)
                return;
        #if UNITY_EDITOR
                if (Input.GetKeyDown(DebugLockAngleKeyCode))
                _angleLocked = !_angleLocked;
            if (Input.GetKeyDown(DebugLockKeyCode))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }
        #endif

            _shootingTimer += Time.deltaTime;
            
            var mouseAxisY = Input.GetAxis("Mouse Y");
            var mouseAxisX = Input.GetAxis("Mouse X");

            if (invertedPitch == false)
            {
                _pitch -= mouseAxisY * pitchRotationalSpeed * Time.deltaTime;
            }
            else
            {
                _pitch += mouseAxisY * pitchRotationalSpeed * Time.deltaTime;
            }
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            if (invertedYaw == false)
            {
                _yaw += mouseAxisX * yawRotationalSpeed * Time.deltaTime;
            }
            else
            {
                _yaw -= mouseAxisX * yawRotationalSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.Euler(0.0f, _yaw, 0.0f);

            pitchControllerTransform.localRotation = Quaternion.Euler(_pitch, 0.0f, 0.0f);

            //Movement
            var movement = Vector3.zero;

            var yawInRadians = _yaw * Mathf.Deg2Rad;
            var yaw90InRadians = (_yaw + 90.0f) * Mathf.Deg2Rad;
            var forward = new Vector3(Mathf.Sin(yawInRadians), 0.0f, Mathf.Cos(yawInRadians));
            var right = new Vector3(Mathf.Sin(yaw90InRadians), 0.0f, Mathf.Cos(yaw90InRadians));

            if (Input.GetKey(UpKeyCode))
            {
                movement = forward;
            }
            else if (Input.GetKey(DownKeyCode))
            {
                movement = -forward;
            }

            if (Input.GetKey(RightKeyCode))
            {
                movement += right;
            }
            else if (Input.GetKey(LeftKeyCode))
            {
                movement -= right;
            }

            speedMultiplier = Input.GetKey(SprintKeyCode) ? 1.5f : 1f;
            
            //Reload
            if (Input.GetKeyDown(ReloadKeyCode))
            {
                _weapon.ChangeWeaponColor();
            }

            movement.Normalize();
            movement *= (Time.deltaTime * speed * speedMultiplier);

            //Gravity
            _verticalSpeed += Physics.gravity.y * Time.deltaTime;
            movement.y = _verticalSpeed * Time.deltaTime;
            var collisionFlags = _characterController.Move(movement);

            if ((collisionFlags & CollisionFlags.Below) != 0)
            {
                _onGround = true;
                _verticalSpeed = 0.0f;
            }
            else
            {
                _onGround = false;
            }

            if ((collisionFlags & CollisionFlags.Above) != 0 && _verticalSpeed > 0.0f)
            {
                _verticalSpeed = 0.0f;
            }

            if (Input.GetKey(JumpKeyCode) && _onGround)
            {
                _verticalSpeed = 5.0f;
            }

            //Shoot
            if (Input.GetMouseButtonDown(0) && _shootingTimer >= timeBetweenShots)
            {
                _weapon.InstantiateProjectile();
                _shootingTimer = 0f;
            } 
                _weapon.SetVelocityIdle(Mathf.Clamp(_characterController.velocity.magnitude, 0.2f, 1.2f));
        }
    }
}
