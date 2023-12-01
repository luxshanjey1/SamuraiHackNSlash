using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            if (gameObject.tag == "Portal1")
            {
                SceneManager.LoadScene(2);
            }
            if (gameObject.tag == "Portal2") 
            {
                SceneManager.LoadScene(0);

            }

        }
    }
}
