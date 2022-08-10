using UnityEngine;
using Lean.Touch;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public Transform cameraTransform;
    public float speed;
    public float jumpHeight;
    public float gravity;

    CharacterController cc;
    Vector3 direction;
    bool doJump;
    float turnSmoothVelocity;
    float turnTime = 0.1f;

    void OnEnable()
    {
        LeanTouch.OnFingerTap += SignalJump;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerTap -= SignalJump;
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        direction = Vector3.zero;
        doJump = false;

        //temporary
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = (Mathf.Abs(joystick.Horizontal) > 0.2) ? joystick.Horizontal : 0;
        direction.z = (Mathf.Abs(joystick.Vertical) > 0.2) ? joystick.Vertical : 0;

        // rotate direction relative to camera angle
        direction = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up) * direction;

        // smoothly rotate model
        if (direction.x != 0 || direction.z != 0)
        {
            float theta = (Mathf.Atan2(direction.z, -direction.x) - Mathf.Atan2(1, 0)) * Mathf.Rad2Deg;
            theta = Mathf.SmoothDampAngle(transform.eulerAngles.y, theta, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0, theta, 0);
        }

        // gravity and jumping
        if (doJump)
            direction.y = jumpHeight;
        else if (cc.isGrounded)
            direction.y = -0.1f;
        else 
            direction.y -= gravity * Time.deltaTime;
        doJump = false;

        cc.Move(direction * speed * Time.deltaTime);
    }

    void SignalJump(Lean.Touch.LeanFinger finger)
    {
        if (!finger.IsOverGui && cc.isGrounded)
            doJump = true;
    }
}
