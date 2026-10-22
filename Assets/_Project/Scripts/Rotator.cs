using UnityEngine;

public class Rotator : MonoBehaviour
{
    // messaggio che mette un valore di defualt per la rotazione dell'oggetto
    [Tooltip("Velocità di rotazione in gradi al secondo")]
    [SerializeField] private float rotationSpeed = 45f;
    // imposta la rotazione dell'oggetto in modo singolo
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
    // imposta la rotazione dell'oggeto in modo pubblico
    public void SetSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }
}