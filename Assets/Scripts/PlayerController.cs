using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private KeyCode primaryMoveForward = KeyCode.W,
        secondaryMoveForward = KeyCode.UpArrow,
        primaryTurnLeft = KeyCode.A,
        secondaryTurnLeft = KeyCode.LeftArrow,
        primaryTurnRight = KeyCode.D,
        secondaryTurnRight = KeyCode.RightArrow,
        shootButton = KeyCode.Space;
    [SerializeField]
    private float thrustSpeed = 1f;
    [SerializeField]
    private float turnSpeed = 1f;
    private bool thrusting;
    private float turnDirection;
    private Rigidbody2D playerRb;
    private AudioSource playerShootingSound;
    // Start is called before the first frame update
    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerShootingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        thrusting = Input.GetKey(primaryMoveForward) || Input.GetKey(secondaryMoveForward);
        if (Input.GetKey(primaryTurnLeft) || Input.GetKey(secondaryTurnLeft))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(primaryTurnRight) || Input.GetKey(secondaryTurnRight))
        {
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(shootButton))
        {
            Shoot();
        }
    }
    // Physic related code better do it here to make it consistend and not depend on game frame rate.
    private void FixedUpdate()
    {
        if (thrusting)
        {
            playerRb.AddForce(this.transform.up * thrustSpeed);
        }

        if (turnDirection != 0.0f)
        {
            playerRb.AddTorque(turnDirection * turnSpeed);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Project(transform.up);
        playerShootingSound.PlayOneShot(playerShootingSound.clip);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = 0f;

            gameObject.SetActive(false);

            GameManager.instance.PlayerDied();
        }
    }
}
