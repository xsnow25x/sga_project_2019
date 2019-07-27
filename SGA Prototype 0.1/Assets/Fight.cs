using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fight : MonoBehaviour
{
    public float Temps;
    public GameObject Body;
    public GameObject Pied;
    private bool Dead = false;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GameOver", 0.5f, 5.0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Touche");
        if (collision.gameObject.tag == "Ennemie")
        {
            collision.gameObject.GetComponent<Animator>().Play("Dead");
            Destroy(collision.gameObject,1.0f);
        }
        if (collision.gameObject.tag == "Poison")
        {
            Destroy(Body, 0.0f);
            Destroy(Pied, 0.0f);
            Dead = true;
        }
    }

    void GameOver()
    {
        if (Dead == true)
        {
        SceneManager.LoadScene("sga prototype 2D");
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
