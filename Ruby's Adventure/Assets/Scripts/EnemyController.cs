using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{

    AudioSource audioSource;
    public float speed = 1.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    public bool hard;
    public static bool win;
    public static bool badEnd;
    RubyController text;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    public ParticleSystem smokeEffect;

    public AudioClip deathSound;


    public static int fixedBots;
    public Text fixedAmount;

    public Text killedAmount;
    public static int killedBots;


    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        fixedBots = 0;
        killedBots = 0;
        fixedAmount.text = "Robots Fixed: " + fixedBots.ToString() + "/5";
        win = false;
        badEnd = false;

        if (hard == true)
        {
            speed = 3.0f;
        }

    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + Time.deltaTime * speed * direction;
        }
        rigidbody2D.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            if (hard == false)
            {
                player.ChangeHealth(-1);
            }
            else
            {
                player.ChangeHealth(-2);
            }
        }
    }
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        smokeEffect.Stop();
        animator.SetTrigger("Fixed");
        fixedBots += 1;
        fixedAmount.text = "Robots Fixed: " + fixedBots.ToString() + "/5";
        if (fixedBots == 5 )
        {
            win = true;
        }
        if (fixedBots + killedBots == 5)
        {
            win = true;
        }
    }

    public void Killed()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        PlaySound(deathSound);
        animator.SetTrigger("Dead");
        rigidbody2D.simulated = false;
        Destroy(gameObject, 0.5f);
        killedBots += 1;
        killedAmount.text = "Kills: " + killedBots.ToString();
        if (killedBots == 5 && sceneName == "MainScene")
        {
            win = true;
         }
        if (killedBots == 5 && sceneName == "Level 2")
        {
            SceneManager.LoadScene(sceneName: "BadEnd");
        }
        if (fixedBots + killedBots == 5)
        {
            win = true;
        }

    }

    public void Win()
    {
        
    }
}
