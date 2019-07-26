using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroy self on hit and do damage.
public class Shot : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        if (collision.GetComponent<HitPoints>() != null)
        {
            collision.GetComponent<HitPoints>().currentHP--;
        }
    }
}
