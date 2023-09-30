using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public EnemiesData enemyData;

    public float health;
    public float attackDamage;
    public float attackSpeed;
    public bool isRanged;
    public bool isMelee;

    [SerializeField] public GameObject player;
    [SerializeField] public GameObject projectile;
    private GameObject projectileInstance;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EnemyController Start()");
        // Initialize data from ScriptableObject
        this.GetComponent<SpriteRenderer>().sprite = enemyData.sprite;
        this.health = enemyData.health;
        this.attackDamage = enemyData.attackDamage;
        this.attackSpeed = enemyData.attackSpeed;
        this.isRanged = enemyData.isRanged;
        this.isMelee = enemyData.isMelee;

        InvokeRepeating("canAttack", 1f, attackSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    void canAttack()
    {
        float playerEnemyDistance = Vector3.Distance(this.transform.position, player.transform.position);
        if (isMelee && playerEnemyDistance < 6f)
        {
            Attack();
        }
        else if (isRanged && 6f < playerEnemyDistance && playerEnemyDistance < 10f)
        {
            direction = (player.transform.position - this.transform.position).normalized*10f;
            projectileInstance = Instantiate(projectile, this.transform.position, Quaternion.LookRotation(direction));
            projectileInstance.GetComponent<Rigidbody2D>().velocity = direction;
            projectileInstance.GetComponent<Projectile>().enemyController = this;
        }

    }

    public void Attack()
    {
        player.GetComponent<PlayerProperties>().health -= attackDamage;
    }


}
