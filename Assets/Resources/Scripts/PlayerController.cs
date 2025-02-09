using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("��������� ������")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float upperLookLimit = 80f;
    [SerializeField] private float lowerLookLimit = -80f;
    private Rigidbody rb;
    private float cameraPitch = 0f;
    private Vector3 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ��������� � �������� ������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ��������� �������� ������
        HandleMouseLook();

        // ��������� �������� ������
        HandleMovement();
    }

    private void HandleMouseLook()
    {
        // �������� ���� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ������� ������ �����-����
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, lowerLookLimit, upperLookLimit);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        // ������� ������ �����-������
        transform.Rotate(Vector3.up * mouseX);
    }


    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

}