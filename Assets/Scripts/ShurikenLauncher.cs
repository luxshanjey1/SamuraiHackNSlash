using UnityEngine;

public class ShurikenLauncher : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject[] shuriken;
    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;

        shuriken[FindShuriken()].transform.position = launchPoint.position;
        shuriken[FindShuriken()].GetComponent<EnemyShuriken>().ActivateShuriken();
    }
    private int FindShuriken()
    {
        for (int i = 0; i < shuriken.Length; i++)
        {
            if (!shuriken[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}