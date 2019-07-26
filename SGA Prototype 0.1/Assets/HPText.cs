using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Show hit points.
public class HPText : MonoBehaviour
{
    public HitPoints thingWithHitPoints;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "HP: " + thingWithHitPoints.currentHP + "/" + thingWithHitPoints.maxHP;
    }
}
