using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int fixedBots;


    // Start is called before the first frame update
    void Start()
    {
        //fixedBots = 0;
        //fixedAmount.text = "Robots Fixed: " + fixedBots.ToString();

    }
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }

        EnemyHard h = other.collider.GetComponent<EnemyHard>();
        if (h != null)
        {
            h.Fix();
        }

        Destroy(gameObject);
    }
}
