using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject doorObject; // ������ �����
    [SerializeField]
    private float openHeight = 3.3f; // ������ �������� �����
    [SerializeField]
    private float moveDuration = 1f; // ����������������� �������� �����
    [SerializeField]
    private string playerTag = "Player"; // ��� ������

    private Vector3 closedPosition; // ������� �������� �����
    private Vector3 openPosition; // ������� �������� �����
    private bool isOpen = false; // ����, �����������, ������� �� �����
    private bool isMoving = false; // ����, �����������, �������� �� �����

    private void Start()
    {
        // ��������, �������� �� ������ �����
        if (doorObject == null)
        {
            Debug.LogError("������ ����� �� �������� � DoorTriggerController!");
            return;
        }

        // ������������� ������� �������� � �������� �����
        closedPosition = doorObject.transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ����� ������ � ������� � ����� ������� � �� ���������
        if (other.CompareTag(playerTag) && !isOpen && !isMoving)
        {
            StartCoroutine(MoveDoor(closedPosition, openPosition));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���� ����� ������� �� �������� � ����� ������� � �� ���������
        if (other.CompareTag(playerTag) && isOpen && !isMoving)
        {
            StartCoroutine(MoveDoor(openPosition, closedPosition));
        }
    }

    private IEnumerator MoveDoor(Vector3 startPos, Vector3 endPos)
    {
        isMoving = true;
        float elapsedTime = 0f;

        // ������� �������� �����
        while (elapsedTime < moveDuration)
        {
            doorObject.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������� �������� �������
        doorObject.transform.position = endPos;
        isMoving = false;
        isOpen = !isOpen;

        // ���� ����� ���������, ������� ������ � ������
        if (!isOpen)
        {
            Destroy(gameObject);
        }
    }
}