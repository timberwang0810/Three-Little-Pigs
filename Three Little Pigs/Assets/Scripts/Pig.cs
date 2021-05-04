﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public float speed;
    public Vector2 initialDirection;
    public bool isJumping;
    public float jumpStartDelay;

    private Rigidbody2D rb;
    private Vector2 currDirection;

    private void Awake()
    {
      
        if (!isJumping)
        {
            currDirection = initialDirection;
            currDirection.Normalize();
            Debug.Log("awake to: " + initialDirection);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isJumping) StartCoroutine(JumpStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentDirection(Vector2 newDir)
    {
        currDirection = newDir;
    }

    public void RunAroundForever(Vector3 initDirection, float runSpeed, float timeBetweenTurning)
    {
        speed = runSpeed;
        StartCoroutine(RunAroundCoroutine(initDirection, timeBetweenTurning));
    }
    
    public void Turn(Direction dir, float leftAngle, float rightAngle)
    {
        if (isJumping) return;
        switch (dir)
        {
            case Direction.LEFT:
                RotateByDegrees(leftAngle);
                break;
            case Direction.RIGHT:
                RotateByDegrees(-rightAngle);
                break;
            case Direction.EITHER:
                float r = Random.Range(0, 1.0f);
                if (r < 0.5f) RotateByDegrees(leftAngle);
                else RotateByDegrees(-rightAngle);
                break;
            default:
                return;
        }
    }

    private void RotateByDegrees(float degree)
    {
        Vector2 newDir = currDirection;
        newDir.x = currDirection.x * Mathf.Cos(Mathf.Deg2Rad * degree) - currDirection.y * Mathf.Sin(Mathf.Deg2Rad * degree);
        newDir.y = currDirection.x * Mathf.Sin(Mathf.Deg2Rad * degree) + currDirection.y * Mathf.Cos(Mathf.Deg2Rad * degree);
        currDirection = newDir;
        
        if (currDirection.x == -1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (currDirection.x == 1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private IEnumerator JumpStart()
    {
        currDirection = Vector2.up;
        float origSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(jumpStartDelay);
        speed = origSpeed;
        speed *= 2;
        yield return new WaitForSeconds(0.25f);
        currDirection = Vector2.down;
        yield return new WaitForSeconds(0.25f);
        speed = origSpeed;
        currDirection = initialDirection;
        currDirection.Normalize();
        isJumping = false;
        GetComponent<SpriteRenderer>().flipX = true;
    }

    private IEnumerator RunAroundCoroutine(Vector2 initDirection, float timeBetweenTurning)
    {
        currDirection = initDirection;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenTurning)
            {
                timer = 0;
                Turn(Direction.LEFT, 180, 180);
            }
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = currDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hut"))
        {
            speed = 0;
            collision.gameObject.GetComponent<Hut>().OnPigEntered();
            Destroy(this.gameObject);
        }        
    }
}
