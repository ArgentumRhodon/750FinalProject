using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1.0f;
    private Vector2 direction = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)direction * Time.deltaTime * movementSpeed;
    }
}
