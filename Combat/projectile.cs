using UnityEngine;
public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float projectileSpeed = 10f;

    public float critChance = 1;
    public int pierce = 2;
    public bool isCrit = false;

    private Rigidbody2D rb;
    private SpriteRenderer projSprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isCrit = Random.Range(0, 100) < critChance;
        projSprite = GetComponent<SpriteRenderer>();
        if (isCrit)
        {
            projSprite.color = new Color(0, 178, 255, 255);
        }
    }

    public void SetStats(float newDamage, float newSpeed, float homingStrength, float newCritChance, int newPierce)
    {
        damage = newDamage;
        projectileSpeed = newSpeed;
        critChance = newCritChance;
        isCrit = Random.Range(0, 100) < critChance;
        pierce = newPierce;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = -transform.up * projectileSpeed;   
    }

    // This method will be called when the projectile collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Projectiles should not delete eachother
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy")) // Projectiles hit enemy
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>(); // Get the enemy's script component and apply damage
            if (enemy != null)
            {
                float critMultiplier = isCrit ? 2 : 1;

                enemy.takeDamage(critMultiplier * damage);
                //Debug.Log(critMultiplier == 2 ? "CRIT" : "NO");
            }
            pierce -= 1;
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            if (pierce <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
