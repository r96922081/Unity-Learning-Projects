using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float timeScale;
    float prevTimeScale;
    int speed = 5;
    int hp = 10;
    public int score = 0;
    GameObject[] hpBlocks = new GameObject[10];
    GameObject replayButton;

    void Start()
    {
        replayButton = GameObject.Find("replayButton");
        replayButton.SetActive(false);
        hpBlocks[0] = GameObject.Find("hpBlock");

        for (int i = 1; i < hpBlocks.Length; i++)
        {
            hpBlocks[i] = Instantiate(hpBlocks[i - 1]);
            hpBlocks[i].transform.Translate(hpBlocks[i].GetComponent<SpriteRenderer>().bounds.size.x + 0.05f, 0, 0);
        }

        timeScale = Time.timeScale;
        prevTimeScale = timeScale;
    }

    void Update()
    {
        if (prevTimeScale != timeScale)
        {
            prevTimeScale = timeScale;
            Time.timeScale = timeScale;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.F))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().SetBool("Moving", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<Animator>().SetBool("Moving", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Moving", false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal == new Vector2(-1, 0) || collision.contacts[0].normal == new Vector2(1, 0))
            return;

        if (collision.transform.tag == "spike")
        {
            hp--;
            if (hp < 0)
                hp = 0;

            UpdateHp();

            if (hp == 0)
                Die();

            GetComponent<Animator>().SetTrigger("OnSpike");
        }
        else if (collision.transform.tag == "floor")
        {
            hp++;
            if (hp > 10)
                hp = 10;
            UpdateHp();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "death")
        {
            Die();
            Time.timeScale = 0;
            replayButton.SetActive(true);
        }
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
        replayButton.SetActive(false);
    }

    private void Die()
    {
        Debug.Log("You lost");
    }

    private void UpdateHp()
    {
        GameObject.Find("hpText").GetComponent<Text>().text = "HP: " + hp;


        for (int i = 0; i < 10; i++)
        {
            if (hp >= i + 1)
            {
                hpBlocks[i].SetActive(true);
            }
            else
            {
                hpBlocks[i].SetActive(false);
            }
        }
    }
}
