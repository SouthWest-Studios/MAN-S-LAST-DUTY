using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1Script : MonoBehaviour
{
    [SerializeField] private GameObject doorToOpen; // Referencia a la puerta que se moverá
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float doorSpeed = 2f;
    private bool isDoorOpen = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        if(doorToOpen != null)
        {
            initialPosition = doorToOpen.transform.position;
            targetPosition = initialPosition + (Vector3.up * 3.3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDoorOpen)
        {
            if (doorToOpen != null)
            {
                StartCoroutine(MoveDoor());
            }
            
            if (audioSource != null)
            {
                audioSource.Play();
            }
            
            isDoorOpen = true;
        }
    }

    private IEnumerator MoveDoor()
    {
        float elapsedTime = 0f;
        while(elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            if(doorToOpen != null)
            {
                doorToOpen.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            }
            yield return null;
        }
    }
}