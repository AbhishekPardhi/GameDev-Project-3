using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Camera cam;
    Vector3 moveDirection;
    CharacterController playerController;
    public float currentSpeed;
    [SerializeField] private float _gravity = 2f;
    public float moveSpeed;
    public float aimSensi;
    public float xRotaionClamp;
    public float newGravity=-9.81f;
    private float xRotation;
    private Vector3 velocity;
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //OldMovement();
        NewMovement();
        Rotation();
    }

    private void NewMovement()
    {
        if (playerController.isGrounded && velocity.y < 0) velocity.y = -2f;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        move = move.normalized;
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

    private void OldMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(moveX, 0, moveY);
        Vector3 transfromDirection = transform.TransformDirection(inputDirection) * moveSpeed;
        Vector3 flatMovement = moveSpeed * Time.deltaTime * transfromDirection;
        moveDirection = new Vector3(flatMovement.x, moveDirection.y, flatMovement.z);
        if (playerController.isGrounded) moveDirection.y = 0f;
        else moveDirection.y -= _gravity * Time.deltaTime;
        playerController.Move(moveDirection);
        currentSpeed = playerController.velocity.magnitude;
    }
}
