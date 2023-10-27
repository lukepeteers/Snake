using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 200.0f;
    public GameObject tailPrefab;
    public GameObject foodPrefab;
    public Transform gameBoard;

    private List<Transform> tail = new List<Transform>();
    private bool isDead = false;

    private Vector3 direction = Vector3.right;
    private float nextMoveTime;
    private float moveInterval = 0.5f;

    private void Start()
    {
        SpawnFood();
    }

    private void Update()
    {
        if (!isDead)
        {
            HandleInput();
            if (Time.time >= nextMoveTime)
            {
                Move();
                nextMoveTime = Time.time + moveInterval;
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) && direction != Vector3.down)
        {
            direction = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && direction != Vector3.up)
        {
            direction = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && direction != Vector3.right)
        {
            direction = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && direction != Vector3.left)
        {
            direction = Vector3.right;
        }
    }

    private void Move()
    {
        Vector3 previousPosition = transform.position;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (tail.Count > 0)
        {
            tail[0].position = previousPosition;
            Vector3 previousTailPosition = tail[0].position;

            for (int i = 1; i < tail.Count; i++)
            {
                Vector3 temp = tail[i].position;
                tail[i].position = previousTailPosition;
                previousTailPosition = temp;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            SpawnFood();
            AddTail();
        }
        else
        {
            isDead = true;
            // Handle game over logic (e.g., restart game, display score, etc.).
        }
    }

    private void SpawnFood()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-gameBoard.localScale.x / 2, gameBoard.localScale.x / 2),
            0.5f,
            Random.Range(-gameBoard.localScale.z / 2, gameBoard.localScale.z / 2)
        );
        Instantiate(foodPrefab, randomPosition, Quaternion.identity);
    }

    private void AddTail()
    {
        Transform newTailSegment = Instantiate(tailPrefab, tail[tail.Count - 1].position, Quaternion.identity).transform;
        tail.Add(newTailSegment);
    }
}
