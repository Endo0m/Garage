using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Rigidbody rb; // ��������� Rigidbody �������
    private Collider objectCollider; // ��������� �������

    private void Start()
    {
        // ��������� ����������� ��� ������
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    public void Pickup()
    {
        // ���������� ������ � ���������� ��� ������� �������
        rb.isKinematic = true;
        objectCollider.enabled = false;
    }

    public void Drop()
    {
        // ��������� ������ � ���������� ��� �������� �������
        rb.isKinematic = false;
        objectCollider.enabled = true;

        // ����� �������� �������
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}