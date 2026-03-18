using System;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;
    public GameObject homingBulletPrefab;
    public GameObject player;
    public float fireCooldown;
    public float specialCooldown;
    public float ultimateCooldown;

    public Image specialCooldownImage;
    public Image ultimateCooldownImage;

    private RectTransform specialRect;
    private RectTransform ultimateRect;

    [Header("Weapon Settings")]
    public float damage = 10f;
    public float projectileSpeed = 10f;
    public float homingStrength = 30f;
    public float critChance = 1f;
    public float hitsPerSecond = 4f;
    public int pierce = 2;
    public int extraProjCount = 0;

    private void Start()
    {
        player = GameObject.Find("player_0");
        //specialCooldownImage.transform.localScale = new Vector3(1, 1, 1);
        //ultimateCooldownImage.transform.localScale = new Vector3(1, 1, 1);
        specialRect = specialCooldownImage.GetComponent<RectTransform>();
        ultimateRect = ultimateCooldownImage.GetComponent<RectTransform>();
    }
    

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        else if (Input.GetButton("Fire1") && (fireCooldown < 0))
        {
            Shoot();
        }
        fireCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && (specialCooldown < 0))
        {
            activateSpecial();
        }

        if (Input.GetKeyDown(KeyCode.Q) && ultimateCooldown < 0)
        {
            activateUltimate();
        }

        if (specialCooldown > 0)
        {
            specialCooldown -= Time.deltaTime;
            float normalizedCooldown = specialCooldown / 10f; 
            specialRect.localScale = new Vector3(1, normalizedCooldown * 0.88f, 1);
        }
        else
        {
            specialCooldown -= Time.deltaTime;
            specialRect.localScale = new Vector3(1, 0, 1);
        }

        if (ultimateCooldown > 0)
        {
            ultimateCooldown -= Time.deltaTime;
            float normalizedCooldown = ultimateCooldown / 30f;
            ultimateRect.localScale = new Vector3(1, normalizedCooldown * 0.88f, 1);
        }
        else
        {
            ultimateCooldown -= Time.deltaTime;
            ultimateRect.localScale = new Vector3(1, 0, 1);
        }

    }

    void Shoot()
    {
        // Instantiate the fireball
        float angleOffset = -225f;
        Quaternion offsetRotation = firepoint.rotation * Quaternion.Euler(0f, 0f, angleOffset);
        GameObject fireball = Instantiate(bulletPrefab, firepoint.position, offsetRotation);

        Projectile projectileScript = fireball.GetComponent<Projectile>();
        projectileScript.SetStats(damage,projectileSpeed, 0,critChance, pierce);

        // Get the Rigidbody2D and Collider2D components
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        Collider2D projectileCollider = fireball.GetComponent<Collider2D>();

        // Get the player Collider2D (assuming the player has this component attached)
        Collider2D playerCollider = player.GetComponent<Collider2D>();

        // Ignore collision between the player and the fireball
        Physics2D.IgnoreCollision(playerCollider, projectileCollider);

        // Apply force to the projectile
        //rb.AddForce(offsetRotation * Vector2.up * bulletForce, ForceMode2D.Impulse);
        fireCooldown = 1/hitsPerSecond;
    }

    void activateSpecial()
    {
        if (specialCooldown <= 0)
        {
            for (float i = 0; i < (8 + extraProjCount); i++)
            {
                float angleZ = i * (360f / (8f + extraProjCount));
                Quaternion angle = Quaternion.Euler(0, 0, angleZ);
                GameObject fireball = Instantiate(homingBulletPrefab, transform.position, angle);

                HomingProjectile projectileScript = fireball.GetComponent<HomingProjectile>();
                projectileScript.SetStats(damage * 2, projectileSpeed, homingStrength, critChance);
            }
            ResetCooldownUI();
            specialCooldown = 10f * (1f-0.25f*((hitsPerSecond-4f)/8f));
            
        }
    }

    void activateUltimate()
    {
        if (ultimateCooldown <= 0)
        {
            for (float i = 0; i < (64 + extraProjCount * 4); i++)
            {
                float angleZ = i * (360f / (64f + extraProjCount * 4f));
                Quaternion angle = Quaternion.Euler(0, 0, angleZ);
                GameObject fireball = Instantiate(bulletPrefab, firepoint.position, angle);

                Projectile projectileScript = fireball.GetComponent<Projectile>();
                projectileScript.SetStats(damage, projectileSpeed, 0, critChance, (int)pierce / 3);
            }
            ResetCooldownUI();
            ultimateCooldown = 30f * (1f - 0.25f * ((hitsPerSecond - 4f) / 8f));
        }
            
    }

    private void ResetCooldownUI()
    {
        specialCooldownImage.transform.localScale = Vector3.one;
        ultimateCooldownImage.transform.localScale = Vector3.one;
    }
}
