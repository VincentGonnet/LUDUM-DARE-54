using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    [SerializeField] public EnemiesData enemyData;

    public float health;
    public float attackDamage;
    public float attackSpeed;
    public bool isRanged;
    public bool isMelee;
    public int wanderDistance = 8;
    public float detectionDistance = 10f;
    public float attackMaxDistance = 8f;
    public float attackMinDistance = 5f;

    public int currentZone;

    [SerializeField] public GameObject player;
    [SerializeField] public GameObject projectile;
    private GameObject projectileInstance;
    Vector3 direction;

    private NavMeshAgent nav {
        get {
            return GetComponent<NavMeshAgent>();
        }
    }


    private Animator animator;
    private Rigidbody2D rb;


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

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetBool("walking", false);
        var clip = animatornimator.GetAnimationClip(0, "Mushroom_Atk");

        InvokeRepeating("canAttack", 1f, attackSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        else if ((nav?.remainingDistance ?? 10) < 1f) {

            Vector3 nextDestination = transform.position;
            nextDestination += wanderDistance * new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(nextDestination, out hit, 5f, NavMesh.AllAreas)) nav.SetDestination(hit.position);
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (nav != null) {
            animator.SetBool("walking", true);

            float playerEnemyDistance = Vector3.Distance(this.transform.position, player.transform.position);
            if (playerEnemyDistance >= attackMaxDistance && playerEnemyDistance < detectionDistance) nav.SetDestination(player.transform.position);
            else if (playerEnemyDistance < attackMinDistance) nav.SetDestination(player.transform.position - this.transform.position);
        }
    }


    void canAttack()
    {
        if (player.GetComponent<PlayerProperties>().currentZone == currentZone)
        {
            float playerEnemyDistance = Vector3.Distance(this.transform.position, player.transform.position);
            if (isMelee && playerEnemyDistance < attackMinDistance)
            {
                StartCoroutine(Attack());
                animator.SetTrigger("atk");
            }
            else if (isRanged && attackMinDistance < playerEnemyDistance && playerEnemyDistance < attackMaxDistance)
            {
                StartCoroutine(RangedAttack());
                animator.SetTrigger("atk");
            }
        }

    }

    //public void Attack(){}

    //public void RangedAttack(){}

    public IEnumerator Attack(){
        player.GetComponent<PlayerProperties>().SetHealth(player.GetComponent<PlayerProperties>().health - attackDamage);
        yield return new WaitForSeconds(clip.length -0.2f);
    }

    public IEnumerator RangedAttack(){
        direction = (player.transform.position - this.transform.position).normalized*10f;
        projectileInstance = Instantiate(projectile, this.transform.position, Quaternion.LookRotation(direction));
        projectileInstance.GetComponent<Rigidbody2D>().velocity = direction;
        projectileInstance.GetComponent<Projectile>().enemyController = this;

        yield return new WaitForSeconds(clip.length -0.2f);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
        Gizmos.DrawWireSphere(transform.position, attackMaxDistance);
        Gizmos.DrawWireSphere(transform.position, attackMinDistance);
    }

}