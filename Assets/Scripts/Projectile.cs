using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb2 =  null;
    public float speed = 15f;
    public float damage = 0.5f;
    public float delaySeconds = 3f;


    private WaitForSeconds cullDelay = null;
    // Start is called before the first frame update
    void Start()
    {
        cullDelay = new WaitForSeconds(delaySeconds);

        StartCoroutine(DelayedCull());

        rb2.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2d(Collider other) 
    { 
        if(GetComponent<Collider>().gameObject.layer == 8)
        {
            IDamagable enemyAttributes = GetComponent<Collider>().GetComponent<IDamagable>();
            if(enemyAttributes != null)
            {
                enemyAttributes.applyDamage(damage);

            }

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private IEnumerator DelayedCull()
    {
        yield return cullDelay;

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
