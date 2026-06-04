using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 0f;
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public float frameTime = 0.15f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;
    public int playerHP = 0;
    public int playerAttack = 0;

    private bool WeaponGet = false;

    float score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        currentSprites = spriteDown;
        sr.sprite = currentSprites[0];

        moveSpeed = GameDataManager.Instance.GetPlayerMoveSpeed();
        playerHP = GameDataManager.Instance.GetPlayerHp();
        playerAttack = GameDataManager.Instance.GetPlayerAttack();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameDataManager.Instance.isTutorialFinished == 0)
        {
            Debug.Log("튜토리얼 오픈!");
            GameDataManager.Instance.isTutorialFinished = 1;
        }
        else
        {

        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(input.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            sr.sprite = currentSprites[frameIndex];
            return;
        }

        timer += Time.deltaTime;

        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;

            if (frameIndex >= currentSprites.Length)
                frameIndex = 0;

            sr.sprite = currentSprites[frameIndex];
        }

        if (WeaponGet)
        {
            
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites)
            return;

        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                    ChangeSprites(spriteRight);
                else
                    ChangeSprites(spriteLeft);
            }
            else
            {
                if (input.y > 0)
                    ChangeSprites(spriteUp);
                else
                    ChangeSprites(spriteDown);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            GameManager.Instance.GameOver();
        }

        if (collision.CompareTag("Finish"))
        {
            StageResultSaver.SaveStage(SceneManager.GetActiveScene().buildIndex, (int)score);

            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            GameManager.Instance.GameOver();
        }

        if (collision.CompareTag("weapon"))
        {
            WeaponGet = true;
        }
    }
    
}
