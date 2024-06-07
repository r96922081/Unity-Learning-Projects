using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    int speed = 10;
    int ballSpeed = 5;

    int ySpeed = 0;
    int xSpeed = 0;

    bool dead = false;

    GameObject left;
    GameObject right;
    GameObject top;
    GameObject bottom;
    GameObject playerCenter;
    GameObject playerLeft;
    GameObject playerRight;
    GameObject ball;
    GameObject replayButton;
    GameObject firstBrick;

    Collision2D collision = null;


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
        firstBrick = GameObject.Find("Brick");

        replayButton.SetActive(false);

        float xMargin = 0.1f;
        float yMargin = 0.2f;
        float xSize = firstBrick.GetComponent<SpriteRenderer>().bounds.size.x;
        float ySize = firstBrick.GetComponent<SpriteRenderer>().bounds.size.y;
        float left2 = left.GetComponent<SpriteRenderer>().bounds.max.x;
        float top2 = top.GetComponent<SpriteRenderer>().bounds.min.y;

        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                GameObject newBrick = Instantiate(firstBrick);

                float xPos = left2 + xSize / 2 + (xSize + xMargin) * x;
                float yPos = top2 - ySize / 2 - (ySize + yMargin) * y;

                newBrick.transform.position = new Vector3(xPos, yPos, 0);
                newBrick.SetActive(true);
            }
        }


        firstBrick.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.F))
        {
            if (playerRight.GetComponent<SpriteRenderer>().bounds.max.x >= right.GetComponent<SpriteRenderer>().bounds.min.x)
                return;

            transform.Translate(speed * Time.deltaTime, 0, 0);

            if (!fire)
            {
                ball.transform.Translate(speed * Time.deltaTime, 0, 0);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.S))
        {
            if (playerLeft.GetComponent<SpriteRenderer>().bounds.min.x <= left.GetComponent<SpriteRenderer>().bounds.max.x)
                return;

            transform.Translate(-speed * Time.deltaTime, 0, 0);

            if (!fire)
            {
                ball.transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
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

        if (collision != null)
        {
            ySpeed = -ySpeed;
            xSpeed = -xSpeed;
            collision.gameObject.SetActive(false);
            collision = null;
        }

        ball.transform.Translate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);
    }


    public void OnBallCollide(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject == right)
            xSpeed = -ballSpeed;

        if (collision.gameObject == left)
            xSpeed = ballSpeed;

        if (collision.gameObject == top)
            ySpeed = -ballSpeed;

        if (collision.gameObject == playerCenter || collision.gameObject == playerLeft || collision.gameObject == playerRight)
            ySpeed = ballSpeed;

        if (collision.gameObject == bottom)
            Die();

        if (collision.gameObject.tag == "brick")
        {
            this.collision = collision;
            //ySpeed = -ySpeed;
            //xSpeed = -xSpeed;
            //collision.gameObject.SetActive(false);
        }
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
