using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPoint : MonoBehaviour
{
    public Direction turnDirection;

    [Range(0, 180.0f)]
    public float leftAngleInDegrees;

    [Range(0, 180.0f)]
    public float rightAngleInDegrees;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Wolf>().Turn(turnDirection, leftAngleInDegrees, rightAngleInDegrees);
        }
    }
}
