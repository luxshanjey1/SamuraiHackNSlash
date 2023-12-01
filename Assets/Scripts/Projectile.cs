using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb2 =  null;
    public CircleCollider2D circle;
    private bool hit;
    public float speed = 15f;
    public float damage = 1f;
    public float delaySeconds = 3f;


    private WaitForSeconds cullDelay = null;
    // Start is called before the first frame update

    private void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
    }
    void Start()
    {
        cullDelay = new WaitForSeconds(delaySeconds);

        StartCoroutine(DelayedCull());

        rb2.velocity = transform.right * speed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (hit) return;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    { 
        if(other.tag == "Enemy")
        {
            Debug.Log("INSIDE");
            hit = true;
            circle.enabled = false;
            other.GetComponent<Health>().TakeDamage(1);
        }
    }

    private IEnumerator DelayedCull()
    {
        yield return cullDelay;

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
