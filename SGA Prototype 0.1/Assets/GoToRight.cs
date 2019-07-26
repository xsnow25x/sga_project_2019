using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRight : MonoBehaviour
{
    public float CameraSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + new Vector2(1, 0), CameraSpeed * Time.deltaTime);
    }
}
