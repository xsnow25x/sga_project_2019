using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player movement controls.
public class PlayerMovement : MonoBehaviour
{
    public float touchRadius = 0.51f;
    public float jumpForce = 10;
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, touchRadius, LayerMask.GetMask("Solid")))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Jump") * jumpForce);
        }
    }
}
