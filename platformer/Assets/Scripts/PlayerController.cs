using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text health;
    private int healthValue = 3;
    public Text winText;
    public Text loseText;

    private bool facingRight = true;

    private float hozMovement;
    private float vertMovement;

    private bool right;
    private bool left;
    private bool jump;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    [SerializeField]
    GameObject Player;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score.text = "Score: " + scoreValue.ToString();
        health.text = "Lives: " + healthValue.ToString();

        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    void Update()
    {
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 2);
            right = true;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            right = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
            left = true;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            left = false;
        }

        if (right == false && left == false)
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            Destroy(collision.collider.gameObject);
            SetScoreText ();
        }

        if (collision.collider.tag == "Enemey")
        {
            healthValue -= 1;
            Destroy(collision.collider.gameObject);
            SetHealthText ();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void SetScoreText ()
    {
        score.text = "Score: " + scoreValue.ToString();

        if (scoreValue == 4)
        {
            transform.position = new Vector2(-3, 19);
            healthValue = 3;
            SetHealthText ();
        }

        if (scoreValue == 8)
        {
            winText.text = "You win! Made by Regan Wheeless.";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            Destroy(gameObject);
        }
    }

    void SetHealthText ()
    {
        health.text = "Lives: " + healthValue.ToString();

        if (healthValue == 0)
        {
            loseText.text = "You lose! Made by Regan Wheeless.";
            Destroy(gameObject);
        }
    }
}
