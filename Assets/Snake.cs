using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Snake : MonoBehaviour
{
    public GameObject segmentPrefab;
    public GameObject applePrefab;
    public BoxCollider2D gameBox;
    public TextMeshProUGUI score;
    private GameArea ga;
    private GameObject apple;
    private Vector3 direction;
    private Vector3 newDirection;
    private List<Transform> body;
    private bool updateBody;
    private System.Random rand;

    void Start()
    {
        body = new List<Transform>();
        ga = new GameArea(gameBox);
        rand = new System.Random();
        apple = Instantiate(applePrefab);

        GameReset();
    }

    private void GameReset()
    {
        ClearGrid();
        for (int i = 1; i < body.Count; i++)
        {
            Destroy(body[i].gameObject);
        }
        body.Clear();
        body.Add(this.transform);
        
        this.transform.position = new Vector3(ga.Center, ga.Center, 0.0f);
        direction = Vector3.zero;
        newDirection = Vector3.zero;
        updateBody = true;

        UpdateGrid();
        apple.transform.position = NextFoodPos();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && direction != Vector3.right)
            newDirection = Vector3.left;
        if(Input.GetKeyDown(KeyCode.W) && direction != Vector3.down)
            newDirection = Vector3.up;
        if(Input.GetKeyDown(KeyCode.D) && direction != Vector3.left)
            newDirection = Vector3.right;
        if(Input.GetKeyDown(KeyCode.S) && direction != Vector3.up)
            newDirection = Vector3.down;
    }

    private void Grow()
    {
        body.Insert(1, Instantiate(segmentPrefab, this.transform.position, Quaternion.identity).transform);
        updateBody = false;
        score.SetText($"Your score is: {body.Count - 1}");
    }

    private void FixedUpdate()
    {
        ClearGrid();

        direction = newDirection;
        if (updateBody)
        {
            for (int i = body.Count - 1; i > 0; i--)
            {
                body[i].position = body[i - 1].position;
            }
        }
        else
        {
            updateBody = true;
        }

        body[0].position = new Vector3(
            Mathf.Round(this.transform.position.x) + direction.x,
            Mathf.Round(this.transform.position.y) + direction.y,
            0.0f
        );

        UpdateGrid();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            if (direction == Vector3.up || direction == Vector3.down)
            {
                body[0].position = new Vector3(
                    Mathf.Round(this.transform.position.x) + direction.x,
                    ga.Height - Mathf.Round(this.transform.position.y) + direction.y,
                    0.0f
                );
            }
            else if (direction == Vector3.left || direction == Vector3.right)
            {
                body[0].position = new Vector3(
                    ga.Width - Mathf.Round(this.transform.position.x) + direction.x,
                    Mathf.Round(this.transform.position.y) + direction.y,
                    0.0f
                );
            }
        }
        if (other.tag == "Player")
        {
            GameReset();
        }
    }

    private void LateUpdate()
    {
        if (body[0].position == apple.transform.position)
        {
            Grow();
            apple.transform.position = NextFoodPos();
        }
    }

    private void ClearGrid()
    {
        if (body.Count > 1 && body[^1].position.x < ga.Width && body[^1].position.y < ga.Height)
            ga.Grid[Mathf.RoundToInt(body[^1].position.x)][Mathf.RoundToInt(body[^1].position.y)] = false;
    }

    private void UpdateGrid()
    {
        if (body[0].position.x < ga.Width && body[0].position.y < ga.Height)
            ga.Grid[Mathf.RoundToInt(body[0].position.x)][Mathf.RoundToInt(body[0].position.y)] = true;
    }

    private Vector3 NextFoodPos()
    {
        List<int> avaliable = new List<int>();

        for (int i = 1; i < ga.Width; i++)
        {
            for (int j = 1; j < ga.Height; j++)
            {
                if (ga.Grid[i][j])
                {
                    continue;
                }
                avaliable.Add(i*ga.Width + j);
            }
        }

        int pos = avaliable[rand.Next(0, avaliable.Count)];
        return new Vector3(
            (float)(pos / ga.Width),
            (float)(pos % ga.Width),
            0.0f
        );
    }
}
