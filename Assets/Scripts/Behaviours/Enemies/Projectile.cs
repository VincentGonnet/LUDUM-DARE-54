using System;
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

    void LateUpdate()
    {
        Vector3 vel = GetComponent<Rigidbody2D>().velocity;
        double tan = vel.y / (vel.x == 0 ? 0.001f : vel.x);
        double angle = (Math.Atan(tan) * (180 / Math.PI));
        angle += vel.x > 0 ? 0 : 180;
        this.transform.rotation = Quaternion.Euler(0, 0, (float)(angle));
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

        // If Object is in Layer Wall, destroy projectile
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            Destroy(this.gameObject);
        }
    }
}
