using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float smooth;
    Camera cam;
    public CharacterController playerController;
    CameraBobble bobbleScript;

    Vector3 moveDirection;
    private Vector3 velocity;
    public float h;
    public float moveSpeed;
    public float aimSensi;
    public float xRotaionClamp;
    public float newGravity=-9.81f;
    private float xRotation;
    
    public bool isCrouched;
    public bool cantMove;
    void Start()
    {
        cam = Camera.main;
        bobbleScript = cam.GetComponent<CameraBobble>();
        Cursor.lockState = CursorLockMode.Locked;
        
        h = playerController.height;
        cantMove = false;
    }

    void FixedUpdate()
    {
        if (!cantMove) NewMovement();
    }
    private void Update()
    {
        if (!cantMove)
        {
            Rotation();
            //Crouch();
        }
        //Cursor.visible = true;
    }

    private void NewMovement()
    {
        if (playerController.isGrounded && velocity.y < 0) velocity.y = -2f;
        float x = Input.GetAxis("Horizontal");
        if (Mathf.Abs(x) < 0.1f) x = 0;
        float z = Input.GetAxis("Vertical");
        if (Mathf.Abs(z) < 0.1f) z = 0;
        Vector3 move = transform.right * x + transform.forward * z;
        move = move.normalized;
        if(move.magnitude > 0)
/*            bobbleScript.bobble();
        else
            bobbleScript.returnToEyeHeight();*/
        playerController.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += newGravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }

    private void Rotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * aimSensi * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * aimSensi * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xRotaionClamp, xRotaionClamp);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouched)
            {
                h = 1f;
                isCrouched = true;
                //print("crouched");
            }
            else
            {
                h = 2f;
                isCrouched = false;
            }
        }
        float lastHeight = playerController.height;
        playerController.height = Mathf.Lerp(playerController.height, h, smooth * Time.deltaTime);
        //playerController.center += new Vector3(0, (playerController.height - lastHeight) / 2, 0);
        //transform.position += new Vector3(0, (playerController.height - lastHeight) / 2, 0);
        Vector3 standingHeight = new Vector3(transform.position.x, 2f, transform.position.z);
        Vector3 crouchedHeight = new Vector3(transform.position.x, 1.5f, transform.position.z);
        if (!isCrouched) transform.position = Vector3.Lerp(transform.position, standingHeight, Time.deltaTime *smooth);
        else transform.position = Vector3.Lerp(transform.position, crouchedHeight, Time.deltaTime * smooth);
    }
}
