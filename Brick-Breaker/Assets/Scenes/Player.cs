using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    int speed = 10;
    int ballSpeed = 5;

    int ySpeed = 0;
    int xSpeed = 0;

    bool dead = false;

    public GameObject left;
    public GameObject right;
    public GameObject top;
    public GameObject bottom;
    public GameObject playerCenter;
    public GameObject playerLeft;
    public GameObject playerRight;
    public GameObject ball;
    public GameObject replayButton;

    bool fire = false;

    void Start()
    {
        playerCenter = GameObject.Find("PlayerCenter");
        playerRight = GameObject.Find("PlayerRight");
        playerLeft = GameObject.Find("PlayerLeft");
        top = GameObject.Find("Top");
        bottom = GameObject.Find("Bottom");
        left = GameObject.Find("Left");
        right = GameObject.Find("Right");
        ball = GameObject.Find("Ball");
        replayButton = GameObject.Find("ReplayButton");

        replayButton.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.F))
        {
            if (playerRight.GetComponent<SpriteRenderer>().bounds.max.x >= right.GetComponent<SpriteRenderer>().bounds.min.x)
                return;

            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.S))
        {
            if (playerLeft.GetComponent<SpriteRenderer>().bounds.min.x <= left.GetComponent<SpriteRenderer>().bounds.max.x)
                return;

            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            if (dead)
            {
                Replay();
            }
            else
            {
                if (!fire)
                {
                    fire = true;
                    ySpeed = ballSpeed;
                    xSpeed = ballSpeed;
                }
            }
        }

        ball.transform.Translate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);
    }

    public void BallCollideBorder(Collision2D collision)
    {
        if (collision.gameObject == right)
            xSpeed = -ballSpeed;

        if (collision.gameObject == left)
            xSpeed = ballSpeed;

        if (collision.gameObject == top)
            ySpeed = -ballSpeed;

        if (collision.gameObject == bottom)
            Die();
    }

    public void BallCollidePlayer(Collision2D collision)
    {
        ySpeed = ballSpeed;
    }

    public void Die()
    {
        Time.timeScale = 0;
        replayButton.SetActive(true);
        dead = true;
    }

    public void Replay()
    {
        replayButton.SetActive(false);
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
}
