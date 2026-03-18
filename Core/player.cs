using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Vector3 moveDelta;
    private Vector3 mousePosition;
    private SpriteRenderer playerSprite;
    private InventoryManager inventory;

    private Shooting shootingScript;
    private HealthBar healthBarScript;

    //private InventoryManager inventoryManager;

    [SerializeField] private GameObject weapon;

    public bool toUpdateStats = false;

    [Header("Player Settings")]
    public float baseSpeed = 1.0f;
    public float baseMaxHealth = 100f;

    [Header("Weapon Settings")]
    public float baseProjectileSpeed = 10;
    public float baseProjectileDamage = 10;
    public float baseHomingStrength = 150f;
    public float baseCritChance = 1f;
    public float baseHitsPerSecond = 4f;
    public int basePierce = 2;
    public int baseExtraProjCount = 0;

    [Header("Current Stats")]
    public float speed;
    public float maxHealth;
    public float projectileSpeed;
    public float projectileDamage;
    public float homingStrength;
    public float critChance;
    public float health;
    public float hitsPerSecond;
    public int pierce;
    public float iFrames = 0.5f;
    public int extraProjCount;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(transform.childCount - 1).gameObject;
        playerSprite = GetComponent<SpriteRenderer>();

        shootingScript = GetComponent<Shooting>();
        InventoryManager inventory = GetComponent<InventoryManager>();

        GameObject healthBarObject = GameObject.Find("HealthBar");
        healthBarScript = healthBarObject.GetComponent<HealthBar>();

        Application.targetFrameRate = 60;

        ResetStats();
        updateWeaponStats();
        health = maxHealth;
    }

    private void Update()
    {
        
        // Make weapon face mouse
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float direction = Mathf.Atan2((mousePosition.y - weapon.transform.position.y),(mousePosition.x - weapon.transform.position.x))*Mathf.Rad2Deg;
        direction -= 45;
        weapon.transform.localEulerAngles = Vector3.forward * direction;
        iFrames -= Time.deltaTime;

        healthBarScript.UpdateHealth();

        if (toUpdateStats)
        {
            updateWeaponStats();
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDelta = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        // swap sprite direction
        if (moveDelta.x > 0)
        {
            playerSprite.flipX = false;
        }
        else if (moveDelta.x < 0)
        {
            playerSprite.flipX = true;
        }

        rb.MovePosition(transform.position + (moveDelta * Time.deltaTime * speed));

        //moveDelta * Time.deltaTime * speed
        float healthRatio = health / maxHealth;
        Color healthColor = Color.Lerp(Color.red, Color.white, healthRatio);
        playerSprite.color = healthColor;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && iFrames < 0)
        {
            var enemyScript = collision.gameObject.GetComponent<Enemy>();
            health -= enemyScript.enemyDamage;
            iFrames = 1f;
        }
    }

    public void updateWeaponStats()
    {
        if (shootingScript != null)
        {
            shootingScript.damage = projectileDamage;
            shootingScript.projectileSpeed = projectileSpeed;
            shootingScript.homingStrength = homingStrength;
            shootingScript.critChance = critChance;
            shootingScript.hitsPerSecond = hitsPerSecond;
            shootingScript.pierce = pierce;
            shootingScript.extraProjCount = extraProjCount;
        }

    }

    public void ResetStats()
    {
        speed = baseSpeed;
        maxHealth = baseMaxHealth;
        hitsPerSecond = baseHitsPerSecond;
        projectileSpeed = baseProjectileSpeed;
        projectileDamage = baseProjectileDamage;
        extraProjCount = baseExtraProjCount;
        critChance = baseCritChance;
        pierce = basePierce;
    }
}
