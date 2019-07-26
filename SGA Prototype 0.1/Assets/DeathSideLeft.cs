using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSideLeft : MonoBehaviour
{
    public Transform cameraFollowTransform;
    public float deathX = 11;
    public string reloadScene = "sga prototype 2D";

    // Update is called once per frame
    void Update()
    {


        if (transform.position.x < cameraFollowTransform.position.x - deathX)
        {
            SceneManager.LoadScene(reloadScene);
        }
    }
}
