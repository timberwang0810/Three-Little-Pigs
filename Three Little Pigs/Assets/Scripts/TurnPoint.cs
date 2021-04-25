using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPoint : MonoBehaviour
{
    public Direction turnDirection;
    private HashSet<GameObject> enemies = new HashSet<GameObject>();

    [Range(0, 180.0f)]
    public float leftAngleInDegrees;

    [Range(0, 180.0f)]
    public float rightAngleInDegrees;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!enemies.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<Enemy>().Turn(turnDirection, leftAngleInDegrees, rightAngleInDegrees);
                enemies.Add(collision.gameObject);
            }
        }
    }
}
