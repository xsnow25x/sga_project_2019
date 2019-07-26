using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player weapon.
public class PlayerGun : MonoBehaviour
{
    public Shot shotPrefab;
    public float shotSpeed = 3;
    public float reloadTime = 2;

    public float cooldown = 0;

    // Update is called once per frame
    void Update()
    {
        cooldown = Mathf.Max(0, cooldown - Time.deltaTime);
        
        if (cooldown == 0 && Input.GetAxis("Fire1") > 0)
        {
            Shot shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
            shot.gameObject.layer = gameObject.layer;
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(shotSpeed, 0);
            cooldown = reloadTime;
        }
    }
}
