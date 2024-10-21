using UnityEngine;
using TMPro;  // Для работы с TextMeshPro

public class PickupController : MonoBehaviour
{
    [SerializeField]
    private Transform holdPoint;                // Точка, где будет отображаться предмет
    [SerializeField]
    private LayerMask pickupLayer;              // Слой для подбираемых объектов
    [SerializeField]
    private float rayLength = 3f;               // Длина луча для поиска объекта
    [SerializeField]
    private float sphereRadius = 0.3f;          // Радиус сферы на конце луча
    [SerializeField]
    private TextMeshProUGUI pickupMessage;      // UI сообщение для отображения возможности подбора

    private Pickupable heldObject;
    private Pickupable potentialPickupObject;  // Объект, на который наведен луч
    private bool canPickup = false;            // Флаг, указывающий на возможность подбора

    private void Start()
    {
        pickupMessage.gameObject.SetActive(false);  // Отключаем сообщение при старте
        InputManager.OnPickupPressed += TogglePickup;
    }

    private void OnDestroy()
    {
        InputManager.OnPickupPressed -= TogglePickup;
    }

    private void Update()
    {
        CheckForPickupableObject();  // Проверяем объект для подбора каждый кадр

        if (heldObject != null)
        {
            // Плавное перемещение объекта к точке удержания
            Vector3 targetPosition = holdPoint.position;
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPosition, Time.deltaTime * 10f);
        }
    }

    private void CheckForPickupableObject()
    {
        // Используем Ray для проверки объектов перед игроком
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Проверяем первое пересечение луча с любым объектом (неважно, что это)
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // Если объект находится на слое подбора
            if (((1 << hit.collider.gameObject.layer) & pickupLayer) != 0)
            {
                potentialPickupObject = hit.collider.GetComponent<Pickupable>();
                if (potentialPickupObject != null)
                {
                    canPickup = true;
                    ShowPickupMessage(true);  // Показываем сообщение
                }
            }
            else
            {
                // Если объект не подбираемый, не продолжаем проверку
                potentialPickupObject = null;
                canPickup = false;
                ShowPickupMessage(false);  // Прячем сообщение
            }
        }
        else
        {
            // Если ничего не нашли
            potentialPickupObject = null;
            canPickup = false;
            ShowPickupMessage(false);  // Прячем сообщение
        }
    }

    private void TogglePickup()
    {
        if (heldObject == null && canPickup)
        {
            TryPickup();
        }
        else if (heldObject != null)
        {
            Drop();
        }
    }

    private void TryPickup()
    {
        if (potentialPickupObject != null)
        {
            heldObject = potentialPickupObject;
            heldObject.Pickup();
            ShowPickupMessage(false);  // Скрываем сообщение при поднятии
        }
    }

    private void Drop()
    {
        if (heldObject != null)
        {
            heldObject.Drop();
            heldObject = null;
        }
    }

    // Функция для отображения и скрытия сообщения
    private void ShowPickupMessage(bool show)
    {
        pickupMessage.gameObject.SetActive(show);
        if (show)
        {
            pickupMessage.text = $"Press E to pick up {potentialPickupObject.name}";
        }
        else if (heldObject != null)
        {
            pickupMessage.text = $"Holding: {heldObject.name}";
        }
        else
        {
            pickupMessage.text = "";
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.forward * rayLength;

        // Рисуем сам луч
        Gizmos.DrawLine(transform.position, transform.position + direction);

        // Рисуем сферу на конце луча
        Gizmos.DrawWireSphere(transform.position + direction, sphereRadius);
    }
}
