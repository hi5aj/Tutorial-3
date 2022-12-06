using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    public int cogs { get { return currentCogs; } }

    int currentHealth;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    bool isInvincible;
    float invincibleTimer;
    bool isDead;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    AudioSource audioSource;
    public AudioClip damagedClip;
    public AudioClip throwClip;
    public AudioClip winSound;
    public AudioClip loseSound;

    public GameObject damaged;
    public GameObject loseText;
    public GameObject winText;
    public Text cogsText;
    int currentCogs;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 10;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        loseText.SetActive(false);
        winText.SetActive(false);
        currentCogs = 4;
        cogsText.text = "Cogs: " + currentCogs;
        isDead = false;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        cogsText.text = "Cogs: " + currentCogs;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && currentCogs > 0 && isDead != true)
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (EnemyController.win == true)
                {
                    SceneManager.LoadScene(sceneName: "Level 2");
                }
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && isDead != true)
        {
            Attack();
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }

        if (EnemyController.win == true)
        {
            Win();
        }

        /*if (Input.GetButtonDown(KeyCode.C))
        {
            Launch();
        }*/
    }
    void FixedUpdate()
    {
        if (isDead != true)
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(damagedClip);
            damaged.SetActive(true);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwClip);
        currentCogs -= 1;


    }
    void GameOver()
    {
        PlaySound(loseSound);
        isDead = true;
        loseText.SetActive(true);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Win()
    {
        PlaySound(winSound);
        winText.SetActive(true);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void BadEnd()
    {

    }

    public void Reload()
    {
        currentCogs = 4;
    }
    void Attack()
    {
        // Play attack animation
        animator.SetTrigger("Launch");

        // Detect enemies in range of attack
        Collider2D[] hitEnemeies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        // Damage enemeis
        foreach(Collider2D enemy in hitEnemeies)
        {
            Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<EnemyController>().Killed();
        }

    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
