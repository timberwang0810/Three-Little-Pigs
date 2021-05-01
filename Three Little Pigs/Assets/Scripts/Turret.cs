using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool building = true; //for placing
    private bool loading = true; //for load anim
    private SpriteRenderer renderer;
    private Animator animator;

    public Material material;
    public int cost;
    public float shootInterval;
    private float shootTimer = 0f;

    public GameObject projectileObject;
    public float projectileSpeed;

    public GameObject enemyInRange;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.green;
        animator = GetComponent<Animator>();
        animator.enabled = false;
        shootTimer = shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing || loading) return;

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            if (enemyInRange != null)
            {
                bool living = enemyInRange.GetComponent<CapsuleCollider2D>().enabled;
                if (!living)
                {
                    enemyInRange = null;
                    return;
                }
                animator.SetTrigger("shoot");
                shootTimer = 0f;
            }
        }
    }

    private void Shoot()
    {
        if (enemyInRange == null) return;
        Vector3 dir = enemyInRange.transform.position - transform.position;
        GameObject projectile = Instantiate(projectileObject, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = dir.normalized * projectileSpeed;
        SoundManager.S.MakeFireTurretSound(material);
        Destroy(projectile, 5f);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UnplaceableArea" && building) renderer.color = Color.red;

        if (collision.gameObject.tag == "Turret" && building) renderer.color = Color.red;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Turret" && building) renderer.color = Color.green;

        if (collision.gameObject.tag == "UnplaceableArea" && building) renderer.color = Color.green;
    }

    // return true on success
    public bool BuildTurret()
    {
        if (renderer.color == Color.green)
        {
            SoundManager.S.OnBuildTurretSound();
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            building = false;
            renderer.color = Color.white;
            animator.enabled = true;
            animator.SetBool("load", true);
            return true;
        }
        return false;
    }

    public void SetBuildDone()
    {
        animator.SetBool("load", false);
        loading = false;
    }
}
