using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : MonoBehaviour
{
    public float StartVelocity;
    private float currentVelocity;
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = Random.insideUnitCircle.normalized;
        currentVelocity = StartVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = Mathf.Lerp(currentVelocity, 0, Time.deltaTime * 2);
        transform.position += dir * currentVelocity * Time.deltaTime;
    }
}
