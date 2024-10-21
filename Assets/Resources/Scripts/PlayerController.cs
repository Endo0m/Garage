using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("Настройки камеры")]
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
        // Блокируем и скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Обработка вращения камеры
        HandleMouseLook();

        // Обработка движения игрока
        HandleMovement();
    }

    private void HandleMouseLook()
    {
        // Получаем ввод мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Вращаем камеру вверх-вниз
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, lowerLookLimit, upperLookLimit);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        // Вращаем игрока влево-вправо
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