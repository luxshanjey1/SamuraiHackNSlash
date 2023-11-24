using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public float healthPool = 10f;

    public float speed = 5f;
    public float jumpFor = 6f;

    public float groundedLeeway = 0.1f;

    private Rigidbody2D rb2 = null;
    private float currentHealth = 1f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
