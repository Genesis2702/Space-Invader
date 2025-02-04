using System.Collections;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float xBound = 0.37f;
    private float backgroundBound = 13.0f;
    public GameObject bulletPrefab;
    private int lives = 3;
    public bool isAlive;
    public bool isHit;
    private float fireRate = 0.3f;
    private float fireRateAttackBuff = 0.1f;
    private bool livesShieldBuff;
    private bool shootAllow = true;
    private Animator playerAnimation;
    private float initialAttackDuration = 12.0f;
    private float initialShieldDuration = 9.0f;
    private float attackDuration;
    private float shieldDuration;
    public AudioClip shootingSound;
    public AudioClip explosionSound;
    public AudioClip hitSound;
    private AudioSource playerAudio;
    private GameManager gameManagerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAlive = true;
        isHit = false;
        transform.position = SetInitialPosition();
        playerAnimation = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameManagerScript.UpdateLive(lives);
        if (isAlive == true)
        {
            float HorizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector2.right * speed *  HorizontalInput * Time.deltaTime);
            OutOfBounds();
            if (playerAnimation.GetBool("inAttackState"))
            {
                attackDuration -= Time.deltaTime;
                fireRate = fireRateAttackBuff;
                if (Input.GetKeyDown(KeyCode.Space) && shootAllow)
                {
                    playerAudio.PlayOneShot(shootingSound, 0.6f);
                    StartCoroutine(ShootingAttackBuff());
                }
                if (attackDuration < 0f)
                {
                    playerAnimation.SetBool("inAttackState", false);
                    fireRate *= 3;
                }
            }
            else if (!playerAnimation.GetBool("inAttackState"))
            {
                if (Input.GetKeyDown(KeyCode.Space) && shootAllow)
                {
                    playerAudio.PlayOneShot(shootingSound, 0.6f);
                    StartCoroutine(Shooting());
                }
            }
            if (playerAnimation.GetBool("inShieldState"))
            {
                shieldDuration -= Time.deltaTime;
                if (shieldDuration < 0f)
                {
                    playerAnimation.SetBool("inShieldState", false);
                    livesShieldBuff = playerAnimation.GetBool("inShieldState");
                }
            }
            if (isHit)
            {
                //playerAudio.PlayOneShot(hitSound);
            }
        }
        if (lives == 0)
        {
            isAlive = false;
            playerAudio.PlayOneShot(explosionSound);
            playerAnimation.SetBool("alive", isAlive);
        }
    }

    IEnumerator Shooting()
    {
        shootAllow = false;
        yield return new WaitForSeconds(fireRate);
        SpawningBullets();
        shootAllow = true;
    }

    IEnumerator ShootingAttackBuff()
    {
        shootAllow = false;
        yield return new WaitForSeconds(fireRate);
        SpawningBulletsAttackBuff();
        shootAllow = true;
    }

    private Vector2 SetInitialPosition()
    {
        Vector2 offset = new Vector2(0, -5);
        return new Vector2(0, 0) + offset;
    }

    private void OutOfBounds()
    {
        if (transform.position.x > backgroundBound - xBound)
        {
            transform.position = new Vector3(backgroundBound - xBound, transform.position.y);
        }
        else if (transform.position.x < -backgroundBound  + xBound)
        {
            transform.position = new Vector3(-backgroundBound + xBound, transform.position.y);
        }
    }

    private void SpawningBullets()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.1f), bulletPrefab.gameObject.transform.rotation);
    }

    private void SpawningBulletsAttackBuff()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.1f), bulletPrefab.gameObject.transform.rotation);
        Instantiate(bulletPrefab, transform.position + new Vector3(-0.3f, 0.1f), bulletPrefab.gameObject.transform.rotation);
        Instantiate(bulletPrefab, transform.position + new Vector3(0.3f, 0.1f), bulletPrefab.gameObject.transform.rotation);
    }

    IEnumerator AbsorbingAttackOrb()
    {
        yield return new WaitForSeconds(0.5f);
        playerAnimation.SetBool("attackOrbAbsorbed", false);
    }

    IEnumerator AbsorbingShieldOrb()
    {
        yield return new WaitForSeconds(0.5f);
        playerAnimation.SetBool("shieldOrbAbsorbed", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Bullet") && !livesShieldBuff)
        {
            isHit = true;
            lives--;
        }
        else if (other.CompareTag("Attack") && !playerAnimation.GetBool("inShieldState"))
        {
            if (playerAnimation.GetBool("inAttackState"))
            {
                attackDuration += initialAttackDuration;
            }
            else if (playerAnimation.GetBool("inAttackState") == false)
            {
                playerAnimation.SetBool("attackOrbAbsorbed", true);
                StartCoroutine(AbsorbingAttackOrb());
                attackDuration = initialAttackDuration;
                playerAnimation.SetBool("inAttackState", true);
            }
        }
        else if (other.CompareTag("Shield") && !playerAnimation.GetBool("inAttackState"))
        {
            if (playerAnimation.GetBool("inShieldState"))
            {
                shieldDuration += initialShieldDuration;
            }
            else if (playerAnimation.GetBool("inShieldState") == false)
            {
                playerAnimation.SetBool("shieldOrbAbsorbed", true);
                StartCoroutine(AbsorbingShieldOrb());
                shieldDuration = initialShieldDuration;
                playerAnimation.SetBool("inShieldState", true);
                livesShieldBuff = playerAnimation.GetBool("inShieldState");
            }
        }
    }
}
