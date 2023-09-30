using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float MaxTravelTime = 5f;
    [SerializeField] public EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TravelTime());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator TravelTime()
    {
        yield return new WaitForSeconds(MaxTravelTime);
        Destroy(this.gameObject);
    }
    
    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Player") {
            enemyController.Attack();
            Destroy(this.gameObject);
        }
    }
}
