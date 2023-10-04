using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class ZoneTransitionBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject enteringZoneTrigger;
    [SerializeField] private Direction transitionDirection;
    [SerializeField] private float unitsToSlide = 2f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Zone Transition Triggered");
            Vector3 movementDirection = Vector3.zero;
            switch (transitionDirection)
            {
                case Direction.Up:
                    movementDirection = Vector3.up;
                    break;
                case Direction.Down:
                    movementDirection = Vector3.down;
                    break;
                case Direction.Left:
                    movementDirection = Vector3.left;
                    break;
                case Direction.Right:
                    movementDirection = Vector3.right;
                    break;
            }

            other.GetComponent<PlayerMovementBehaviour>().ChangeZone(movementDirection, unitsToSlide, enteringZoneTrigger);
        }
    }
}
