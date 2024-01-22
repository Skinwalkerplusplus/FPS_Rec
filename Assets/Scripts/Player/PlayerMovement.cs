using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    Vector3 cameraRotation;
    private Vector3 m_playerCameraRotation;
    private Vector3 currentSpeedDirection;

    [SerializeField] float cameraRotationSpeed = 180;
    [SerializeField] float jumpSpeedInAir = 3;


    public float mouseSensitivity;
    private float moveSpeed = 5f;
    public Transform playerBody;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public Transform myCamera;
    Vector3 move;

    CharacterController controllerPlayer;
    public bool canSprint;

    public float movSpeed = 5;
    [SerializeField] float jumpSpeed = 8;
    [SerializeField] int gravityMultip = 1;
    float gravity = -9.8f;

    //private int m_inversionX => (isXInverted) ? -1 : 1;
    //private int m_inversionY => (isYInverted) ? -1 : 1;

    private void Start()
    {
        Time.fixedDeltaTime = 0.01f;
        controllerPlayer = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        m_playerCameraRotation = Vector3.zero;
        cameraRotation = Vector2.zero;
        canSprint = true;
        Debug.Log(Time.fixedDeltaTime);

    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

        //cameraRotation.y = Input.GetAxis("Mouse X"); /** m_inversionY;*/
        //cameraRotation.x = Input.GetAxis("Mouse Y"); /** m_inversionX;*/

        //Vector3 cameraRotationVector = new Vector3(cameraRotation.x, cameraRotation.y, 0) * (cameraRotationSpeed * Time.fixedDeltaTime);
        //myCamera.Rotate(-cameraRotationVector);

        //float angleTransformation = (myCamera.eulerAngles.x > 180)
        //    ? myCamera.localEulerAngles.x - 360
        //    : myCamera.localEulerAngles.x;

        //cameraRotationVector = new Vector3(0, Mathf.Clamp(angleTransformation, -90, 90), 0);

        //myCamera.localEulerAngles = cameraRotationVector;

        //m_playerCameraRotation.y = cameraRotation.y;
        //transform.Rotate(m_playerCameraRotation * (cameraRotationSpeed * Time.fixedDeltaTime));
        //myCamera.localRotation = Quaternion.Lerp(Quaternion.Euler(myCamera.localRotation.x, myCamera.localRotation.y, 0), Quaternion.Euler(myCamera.localEulerAngles.x, myCamera.localEulerAngles.y, 0), Time.fixedDeltaTime * cameraRotationSpeed);
        //myCamera.localRotation = Quaternion.Lerp(Quaternion.Euler(myCamera.localRotation.x, myCamera.localRotation.y, 0), Quaternion.Euler(myCamera.localEulerAngles.x, myCamera.localEulerAngles.y, 0), Time.fixedDeltaTime * cameraRotationSpeed);

        //Quaternion targetRotation = Quaternion.Euler(0f, cameraRotation.y * cameraRotationSpeed, 0f);

        //myCamera.rotation = Quaternion.Lerp(myCamera.rotation, targetRotation, Time.deltaTime * cameraRotationSpeed);

        //yRotation += mouseX;

        //xRotation -= mouseY;
        //yRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //transform.rotation = Quaternion.Euler(0, myCamera.rotation.y, 0);
        //myCamera.rotation = Quaternion.Slerp(Quaternion.Euler(myCamera.rotation.x, myCamera.rotation.y, 0), Quaternion.Euler(xRotation, yRotation, 0), Time.fixedDeltaTime * mouseSensitivity);
    }

    void Movement()
    {

        if (controllerPlayer.isGrounded)
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

        move.y += gravity * gravityMultip * Time.fixedDeltaTime;
        controllerPlayer.Move(move / 50);
    }


}
