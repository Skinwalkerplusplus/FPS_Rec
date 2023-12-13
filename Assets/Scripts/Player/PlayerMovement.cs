using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    Vector3 cameraRotation;
    private Vector3 m_playerCameraRotation;

    [SerializeField] float cameraRotationSpeed = 180;

    public float mouseSensitivity = 100f;
    private float moveSpeed = 5f;
    public Transform playerBody;
    private Vector3 currentSpeedDirection;

    private float xRotation = 0f;

    public Transform myCamera;
    Vector3 move;

    CharacterController controler;
    public bool canSprint;

    public float movSpeed = 5;
    [SerializeField] float jumpSpeed = 8;
    [SerializeField] float jumpSpeedInAir = 3;
    [SerializeField] int gravityMultip = 1;
    float gravity = -9.8f;

    //private int m_inversionX => (isXInverted) ? -1 : 1;
    //private int m_inversionY => (isYInverted) ? -1 : 1;

    private void Start()
    {
        controler = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        m_playerCameraRotation = Vector3.zero;
        cameraRotation = Vector2.zero;
        canSprint = true;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        Rotation();
        Movement();
    }

    void Rotation()
    {
        cameraRotation.x = Input.GetAxis("Mouse Y"); /** m_inversionY;*/
        cameraRotation.y = Input.GetAxis("Mouse X"); /** m_inversionX;*/

        Vector3 cameraRotationVector = new Vector3(cameraRotation.x, 0, 0) * (cameraRotationSpeed * Time.deltaTime);
        myCamera.Rotate(-cameraRotationVector);

        float angleTransformation = (myCamera.eulerAngles.x > 180)
            ? myCamera.localEulerAngles.x - 360
            : myCamera.localEulerAngles.x;

        cameraRotationVector = new Vector3(Mathf.Clamp(angleTransformation, -90, 90), 0, 0);

        myCamera.localEulerAngles = cameraRotationVector;

        m_playerCameraRotation.y = cameraRotation.y;
        transform.Rotate(m_playerCameraRotation * (cameraRotationSpeed * Time.deltaTime));
    }

    void Movement()
    {

        if (controler.isGrounded)
        {
            move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
            move *= movSpeed;
            if (canSprint)
            {
                if (Input.GetButtonDown("Run"))
                {
                    movSpeed = 8f;
                    Debug.Log("Down");
                }

                if (Input.GetButtonUp("Run"))
                {
                    movSpeed = 5f;
                    Debug.Log("Up");
                }
            }
            

            if (Input.GetButton("Jump"))
            {
                currentSpeedDirection = move;
                move.y = jumpSpeed;
            }
        }

        else
        {
            move.x = currentSpeedDirection.x + ((transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")).x) * jumpSpeedInAir;
            move.z = currentSpeedDirection.z + ((transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")).z) * jumpSpeedInAir;
        }

        move.y += gravity * gravityMultip * Time.deltaTime;
        controler.Move(move * Time.deltaTime);
    }
}
