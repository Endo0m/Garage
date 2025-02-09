using UnityEngine;
using TMPro;  // ��� ������ � TextMeshPro

public class PickupController : MonoBehaviour
{
    [SerializeField]
    private Transform holdPoint;                // �����, ��� ����� ������������ �������
    [SerializeField]
    private LayerMask pickupLayer;              // ���� ��� ����������� ��������
    [SerializeField]
    private float rayLength = 3f;               // ����� ���� ��� ������ �������
    [SerializeField]
    private float sphereRadius = 0.3f;          // ������ ����� �� ����� ����
    [SerializeField]
    private TextMeshProUGUI pickupMessage;      // UI ��������� ��� ����������� ����������� �������

    private Pickupable heldObject;
    private Pickupable potentialPickupObject;  // ������, �� ������� ������� ���
    private bool canPickup = false;            // ����, ����������� �� ����������� �������

    private void Start()
    {
        pickupMessage.gameObject.SetActive(false);  // ��������� ��������� ��� ������
        InputManager.OnPickupPressed += TogglePickup;
    }

    private void OnDestroy()
    {
        InputManager.OnPickupPressed -= TogglePickup;
    }

    private void Update()
    {
        CheckForPickupableObject();  // ��������� ������ ��� ������� ������ ����

        if (heldObject != null)
        {
            // ������� ����������� ������� � ����� ���������
            Vector3 targetPosition = holdPoint.position;
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPosition, Time.deltaTime * 10f);
        }
    }

    private void CheckForPickupableObject()
    {
        // ���������� Ray ��� �������� �������� ����� �������
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // ��������� ������ ����������� ���� � ����� �������� (�������, ��� ���)
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // ���� ������ ��������� �� ���� �������
            if (((1 << hit.collider.gameObject.layer) & pickupLayer) != 0)
            {
                potentialPickupObject = hit.collider.GetComponent<Pickupable>();
                if (potentialPickupObject != null)
                {
                    canPickup = true;
                    ShowPickupMessage(true);  // ���������� ���������
                }
            }
            else
            {
                // ���� ������ �� �����������, �� ���������� ��������
                potentialPickupObject = null;
                canPickup = false;
                ShowPickupMessage(false);  // ������ ���������
            }
        }
        else
        {
            // ���� ������ �� �����
            potentialPickupObject = null;
            canPickup = false;
            ShowPickupMessage(false);  // ������ ���������
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
            ShowPickupMessage(false);  // �������� ��������� ��� ��������
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

    // ������� ��� ����������� � ������� ���������
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

        // ������ ��� ���
        Gizmos.DrawLine(transform.position, transform.position + direction);

        // ������ ����� �� ����� ����
        Gizmos.DrawWireSphere(transform.position + direction, sphereRadius);
    }
}
