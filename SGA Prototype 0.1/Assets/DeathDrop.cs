using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Reload the level if the player falls down.
public class DeathDrop : MonoBehaviour
{
    public Transform cameraFollowTransform;
    public float deathY = -50;
    public string reloadScene = "SampleScene";

    // Update is called once per frame
    void Update()
    {


        if (transform.position.y < deathY)
        {
            SceneManager.LoadScene(reloadScene);
        }
    }
}
