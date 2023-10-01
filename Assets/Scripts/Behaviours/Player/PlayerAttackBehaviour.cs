using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{

    [Header("Component References")]
    public GameObject player;

    [Header("Attack Settings")]
    public float attackDistance = 3f;

    //Stored Values
    private Vector3 attackDirection;

    public Collider2D[] attackTargets;
    private Vector2 startPositionForAnimation;


    public void SetupBehaviour()
    {
        // Potentially setup any other components here
        // Is called when the player is setup in the GameManager
    }

    public void UpdateAttackData(Vector3 newAttackDirection)
    {
        attackDirection = newAttackDirection;
    }

    public void Attack()
    {
        attackTargets = Physics2D.OverlapCircleAll(transform.position, attackDistance, LayerMask.GetMask("Enemies"));
        if(attackTargets.Length > 0){
            StartCoroutine(AttackAnimation());
        }
        foreach (var target in attackTargets)
        {
            target.GetComponent<EnemyController>().takeHit();
            UpdateAttackData((transform.position - target.transform.position).normalized);
            target.GetComponent<Rigidbody2D>().AddForce(attackDirection * -1000000f);
        }
            
        
    }


    IEnumerator AttackAnimation(){
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
        player.GetComponent<PlayerController>().SetSpriteDirection(new Vector2(1, 1));
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().SetSpriteDirection(new Vector2(-1, 1));
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().SetSpriteDirection(new Vector2(-1, -1));
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController>().SetSpriteDirection(new Vector2(1, -1));
        player.GetComponent<PlayerMovementBehaviour>().ToggleMovement();
        yield return 0;
    }
}
