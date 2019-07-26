using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keep track of hit points and destroy self if HP is 0.
public class HitPoints : MonoBehaviour
{
    public int maxHP = 3;
    public int currentHP = 0;
    public string deathScene;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            if (deathScene.Length > 0)
            {
                SceneManager.LoadScene(deathScene);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
