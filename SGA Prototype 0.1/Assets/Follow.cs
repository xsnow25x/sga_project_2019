using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (follow != null)
        {
            transform.position = follow.position + offset;
        }
    }
}
