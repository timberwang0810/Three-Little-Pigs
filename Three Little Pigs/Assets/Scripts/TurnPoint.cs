using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPoint : MonoBehaviour
{
    public Direction turnDirection;
    private HashSet<GameObject> turningEntities = new HashSet<GameObject>();

    [Range(0, 180.0f)]
    public float leftAngleInDegrees;

    [Range(0, 180.0f)]
    public float rightAngleInDegrees;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Pig"))
        {
            if (!turningEntities.Contains(collision.gameObject))
            {
                if (collision.gameObject.CompareTag("Enemy")) collision.gameObject.GetComponent<Enemy>().Turn(turnDirection, leftAngleInDegrees, rightAngleInDegrees);
                else collision.gameObject.GetComponent<Pig>().Turn(turnDirection, leftAngleInDegrees, rightAngleInDegrees);
                turningEntities.Add(collision.gameObject);
            }
        }
    }
}
