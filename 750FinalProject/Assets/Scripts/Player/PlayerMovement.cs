using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CircleCollider circleCollider;
    private Vector2 direction;
    private Vector2 movement;
    [SerializeField]
    private float movementSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider>();
        direction = Vector2.zero;
        movement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        StayInBounds();
        transform.position += (Vector3)movement;
        movement = direction * movementSpeed * Time.deltaTime;
    }

    private void StayInBounds()
    {
        Vector2 cameraWorldBounds = GameManager.Instance.cameraWorldBounds;

        if (transform.position.y + circleCollider.radius + movement.y + 0.1 > cameraWorldBounds.y || 
            transform.position.y - circleCollider.radius + movement.y - 0.1 < -cameraWorldBounds.y
        )
        {
            movement.y = 0;
        }
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }
}
