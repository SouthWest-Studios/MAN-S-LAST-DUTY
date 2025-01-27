using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1Script : MonoBehaviour
{
    [SerializeField] private GameObject doorToOpen; 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float doorSpeed = 2f;
    private bool isDoorOpen = false;
    private bool isMoving = false;
    private bool pendingClose = false;
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
        if (other.CompareTag("Player") && !isDoorOpen && !isMoving)
        {
            if (doorToOpen != null)
            {
                StartCoroutine(OpenDoor());
            }
            
            if (audioSource != null)
            {
                audioSource.Play();
            }
            
            isDoorOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isDoorOpen)
        {
            if (!isMoving)
            {
                if (doorToOpen != null)
                {
                    StartCoroutine(CloseDoor());
                }
                
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                
                isDoorOpen = false;
            }
            else
            {
                pendingClose = true;
            }
        }
    }

    private IEnumerator OpenDoor()
    {
        yield return StartCoroutine(MoveDoorToPosition(initialPosition, targetPosition));
    }

    private IEnumerator CloseDoor()
    {
        yield return StartCoroutine(MoveDoorToPosition(targetPosition, initialPosition));
    }

    private IEnumerator MoveDoorToPosition(Vector3 startPos, Vector3 endPos)
    {
        isMoving = true;
        float elapsedTime = 0f;
        while(elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            if(doorToOpen != null)
            {
                doorToOpen.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
            }
            yield return null;
        }
        isMoving = false;

        if (pendingClose)
        {
            pendingClose = false;
            StartCoroutine(CloseDoor());
            if (audioSource != null)
            {
                audioSource.Play();
            }
            isDoorOpen = false;
        }
    }
}