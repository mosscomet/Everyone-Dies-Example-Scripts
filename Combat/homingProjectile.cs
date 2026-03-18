using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
public class HomingProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float turnSpeed = 30f;
    public float projectileSpeed = 10f;
    public float critChance = 1;
    public bool isCrit = false;

    private Rigidbody2D rb;
    private Transform target;

    private SpriteRenderer projSprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        findNearestEnemy();
        isCrit = Random.Range(0,100) < critChance;
        projSprite = GetComponent<SpriteRenderer>();
        if (isCrit)
        {
            projSprite.color = new Color(0, 178, 255, 255);
        }
    }

    public void SetStats(float newDamage, float newSpeed, float homingStrength, float newCritChance)
    {
        damage = newDamage;
        projectileSpeed = newSpeed * 0.8f;
        turnSpeed = homingStrength * (projectileSpeed / 10f);
        critChance = newCritChance;
        isCrit = Random.Range(0, 100) < critChance;
    }

    // This method will be called when the projectile collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Projectiles hit enemy
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>(); // Get the enemy's script component and apply damage
            if (enemy != null)
            {
                float critMultiplier = isCrit ? 2 : 1;

                enemy.takeDamage(critMultiplier * damage);

                //Debug.Log(critMultiplier == 2 ? "CRIT" : "NO");
            }

            Destroy(gameObject);
        }
        if (!collision.gameObject.CompareTag("Projectile")) // Projectiles should not delete eachother
        {
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 90f; // Adjust by adding 90 degrees (rotate left by 90 degrees)
            float currentAngle = rb.rotation;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, angle, turnSpeed * Time.fixedDeltaTime);
            rb.SetRotation(newAngle);
            rb.linearVelocity = -transform.up * projectileSpeed;
        }
        else
        {
            // If no target, move in the same direction with the rotation applied
            findNearestEnemy();
            rb.linearVelocity = -transform.up * projectileSpeed;
        }
    }

    private void findNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;

        foreach ( GameObject enemy in enemies )
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                target = enemy.transform;
            }
        }

    }

}
