using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Rigidbody rb; // Компонент Rigidbody объекта
    private Collider objectCollider; // Коллайдер объекта

    private void Start()
    {
        // Получение компонентов при старте
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    public void Pickup()
    {
        // Отключение физики и коллайдера при подборе объекта
        rb.isKinematic = true;
        objectCollider.enabled = false;
    }

    public void Drop()
    {
        // Включение физики и коллайдера при бросании объекта
        rb.isKinematic = false;
        objectCollider.enabled = true;

        // Сброс скорости объекта
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}