using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static event Action OnPickupPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnPickupPressed?.Invoke();
        }
    }
}