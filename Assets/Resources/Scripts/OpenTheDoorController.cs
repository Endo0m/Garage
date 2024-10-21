using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject doorObject; // Объект двери
    [SerializeField]
    private float openHeight = 3.3f; // Высота открытия двери
    [SerializeField]
    private float moveDuration = 1f; // Продолжительность движения двери
    [SerializeField]
    private string playerTag = "Player"; // Тег игрока

    private Vector3 closedPosition; // Позиция закрытой двери
    private Vector3 openPosition; // Позиция открытой двери
    private bool isOpen = false; // Флаг, указывающий, открыта ли дверь
    private bool isMoving = false; // Флаг, указывающий, движется ли дверь

    private void Start()
    {
        // Проверка, назначен ли объект двери
        if (doorObject == null)
        {
            Debug.LogError("Объект двери не назначен в DoorTriggerController!");
            return;
        }

        // Инициализация позиций закрытой и открытой двери
        closedPosition = doorObject.transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если игрок входит в триггер и дверь закрыта и не двигается
        if (other.CompareTag(playerTag) && !isOpen && !isMoving)
        {
            StartCoroutine(MoveDoor(closedPosition, openPosition));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Если игрок выходит из триггера и дверь открыта и не двигается
        if (other.CompareTag(playerTag) && isOpen && !isMoving)
        {
            StartCoroutine(MoveDoor(openPosition, closedPosition));
        }
    }

    private IEnumerator MoveDoor(Vector3 startPos, Vector3 endPos)
    {
        isMoving = true;
        float elapsedTime = 0f;

        // Плавное движение двери
        while (elapsedTime < moveDuration)
        {
            doorObject.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Установка конечной позиции
        doorObject.transform.position = endPos;
        isMoving = false;
        isOpen = !isOpen;

        // Если дверь закрылась, удаляем объект и скрипт
        if (!isOpen)
        {
            Destroy(gameObject);
        }
    }
}