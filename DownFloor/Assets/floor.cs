using UnityEngine;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
    [SerializeField] int moveSpeed = 1;
    [SerializeField] GameObject[] floorTypes;
    GameObject ceiling;
    // Start is called before the first frame update
    void Start()
    {
        ceiling = GameObject.Find("ceiling");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, Time.deltaTime * moveSpeed, 0);

        if (GetComponent<SpriteRenderer>().bounds.max.y >= ceiling.GetComponent<BoxCollider2D>().bounds.min.y)
        {
            GameObject floor = Instantiate(floorTypes[new System.Random().Next(0, floorTypes.Length)]);
            floor.transform.position = new Vector3(new System.Random().Next(-3, 4), -6, 0);

            Destroy(gameObject);

            Gv.score++;
            GameObject.Find("score").GetComponent<Text>().text = "Score: " + Gv.score;
        }
    }
}
