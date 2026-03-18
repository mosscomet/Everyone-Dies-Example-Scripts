using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float enemyHealth = 100f;
    [SerializeField] private float enemyMaxHealth;
    [SerializeField] private float enemySpeed = 2.5f;
    [SerializeField] private float enemySize = 1f;
    [SerializeField] public float enemyDamage = 10f;
    [SerializeField] private bool elite = false;
    [SerializeField] private float eliteChance = 10f;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private int DropChance = 5;

    public Vector2[] spawnZones = new Vector2[] {
        new Vector2(-10f, -4f),
        new Vector2(54f, 57f)
    };

    private Color originalColor;
    private SpriteRenderer sprite;

    private GameObject player;
    private Rigidbody2D rb;
    private SpawnManager spawnManager;

    private string itemsFolderPath = "Items";
    private string eliteItemsFolderPath = "EliteItems";
    private GameObject[] itemsPrefabs;
    private GameObject[] eliteItemsPrefabs;

    void Start()
    {
        itemsPrefabs = Resources.LoadAll<GameObject>(itemsFolderPath);
        eliteItemsPrefabs = Resources.LoadAll<GameObject>(eliteItemsFolderPath);

        player = GameObject.Find("player_0");
        //playerScript = player.GetComponent<player>();
        enemySpeed /= Mathf.Pow(1.5f, enemySize);
        transform.localScale = new Vector3(enemySize*0.5f, enemySize * 0.5f, enemySize * 0.5f);
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 50 * Mathf.Pow(2.5f, enemySize);
        sprite = GetComponent<SpriteRenderer>();
        
        
        // Scale Enemies Based on Wave Count
        spawnManager = GameObject.Find("EnemyManager").GetComponent<SpawnManager>();
        if (spawnManager.isExponentialScaling == false)
        {
            enemyHealth *= 1 + (float)(0.1 * spawnManager.waveCount);
            enemySpeed *= 1 + (float)(0.05 * spawnManager.waveCount);
            enemyDamage *= 1 + (float)(0.03 * spawnManager.waveCount);
        }
        else if (spawnManager.isExponentialScaling == true)
        {
            float healthMultiplier = Mathf.Pow(1.05f, spawnManager.waveCount);
            enemyHealth *= healthMultiplier;

            float speedMultiplier = Mathf.Pow(1.02f, spawnManager.waveCount);
            enemySpeed *= speedMultiplier;

            float damageMultiplier = Mathf.Pow(1.02f, spawnManager.waveCount);
            enemyDamage *= damageMultiplier;

            Debug.Log("Health Mult:" + healthMultiplier + "  Speed Mult: " + speedMultiplier + "  Damage Mult: " + damageMultiplier);
        }

        if (!isBoss)
        {
            if (spawnManager.waveCount % 5 == 0)
            {
                if (UnityEngine.Random.Range(0, 100) < eliteChance * 4)
                {
                    elite = true;
                }
            }
            else
            {
                if (UnityEngine.Random.Range(0, 100) < eliteChance)
                {
                    elite = true;
                }
            }
        }
        
        if (elite)
        {
            enemyHealth *= 2.5f;
            enemySpeed *= 1.1f;
            sprite.color = new Color(255f / 255f, 0 / 255f, 203f / 255f);
        }

        originalColor = sprite.color;
        enemyMaxHealth = enemyHealth;
    }

    void FixedUpdate()
    {
        Vector3 direction = (player.transform.position-transform.position).normalized;
        rb.MovePosition(transform.position + (direction * enemySpeed * Time.deltaTime));

        if (enemyHealth <= 0f)
        {
            HandleItemDrop();
            Die();
        }

    }

    private void Update()
    {
        float healthRatio = enemyHealth / enemyMaxHealth;
        Color healthColor = Color.Lerp(Color.red, originalColor, healthRatio);
        sprite.color = healthColor;
    }

    private void HandleItemDrop()
    {
        if (isBoss)
        {
            SpawnItem(eliteItemsPrefabs);
            return;
        }
        
        int dropRoll = UnityEngine.Random.Range(0, 100);
        if ((dropRoll < DropChance) && elite)
        {
            SpawnItem(eliteItemsPrefabs);
        }
        else if (dropRoll < DropChance)
        {
            SpawnItem(itemsPrefabs);
        }
    }

    private void SpawnItem(GameObject[] itemPrefabs)
    {
        if (itemPrefabs.Length == 0) return;

        GameObject itemPrefab = itemPrefabs[UnityEngine.Random.Range(0, itemPrefabs.Length)];
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }

    public void takeDamage(float damage)
    {
        enemyHealth -= damage;
    }

    public void Die()
    {
        EnemyCounter.Instance.IncrementKillCount();
        Destroy(gameObject);
    }
}
