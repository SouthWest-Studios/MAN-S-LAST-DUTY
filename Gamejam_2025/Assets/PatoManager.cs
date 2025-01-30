using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatoManager : MonoBehaviour
{
    public Transform eye1, eye2;
    public Transform target;
    public float rotationSpeed = 5f; // Velocidad de rotación

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        eye1.LookAt(target);
        eye2.LookAt(target);

        if (!IsPlayerLookingAtMe())
        {
            RotateTowardsPlayer();
        }
    }

    bool IsPlayerLookingAtMe()
    {
        Vector3 toPato = (transform.position - target.position).normalized;
        float dotProduct = Vector3.Dot(target.forward, toPato);

        return dotProduct > 0.5f; // Si el valor es alto, el jugador está mirando
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Mantener la rotación solo en Y
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
