using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shoot at the player if they are close enough.
public class ShootPlayer : MonoBehaviour
{
    public GameObject player;
    public float shootRadius = 5;
    public Shot shotPrefab;
    public float shotSpeed = -3;
    public float reloadTime = 2;

    public float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = Mathf.Max(0, cooldown - Time.deltaTime);
        if (cooldown == 0 && (transform.position - player.transform.position).magnitude < shootRadius)
        {
            Shot shot = Instantiate(shotPrefab, transform.position, Quaternion.identity);
            shot.gameObject.layer = gameObject.layer;
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(shotSpeed, 0);
            cooldown = reloadTime;
        }
    }
}
