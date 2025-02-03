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
    public float initialAttackDuration = 12.0f;
    public float initialShieldDuration = 9.0f;
    public float attackDuration;
    public float shieldDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAlive = true;
        isHit = false;
        transform.position = SetInitialPosition();
        playerAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == true)
        {
            float HorizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector2.right * speed *  HorizontalInput * Time.deltaTime);
            OutOfBounds();
            if (Input.GetKeyDown(KeyCode.Space) && shootAllow)
            {
                StartCoroutine(Shooting());
            }
            if (playerAnimation.GetBool("inAttackState"))
            {
                attackDuration -= Time.deltaTime;
                fireRate = fireRateAttackBuff;
                if (attackDuration < 0f)
                {
                    playerAnimation.SetBool("inAttackState", false);
                    fireRate *= 3;
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
        }
        if (lives == 0)
        {
            isAlive = false;
        }
    }

    IEnumerator Shooting()
    {
        shootAllow = false;
        yield return new WaitForSeconds(fireRate);
        SpawningBullets();
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
        else if (other.CompareTag("Attack"))
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
        else if (other.CompareTag("Shield"))
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
