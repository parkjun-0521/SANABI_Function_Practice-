using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameKeyboard;

public class Player : MonoBehaviour
{
    public int health;

    public float speed;                 // 플레이어 이동속도 
    public float maxSpeed;              // 플레이어 최대 가속도 

    public float jumpPower;             // 점프 높이 
    [HideInInspector]
    public int jumpCount = 0;           // 2단 점프 유무 
    [HideInInspector]
    public bool isJumpCheck = false;    // 점프 상태 확인 


    public bool isAttack= false;        // 공격을 하는 상태 ( 적에게 사슬 발사 시 ) 
    public bool isAcceleration = false; // 사슬을 걸고 shift를 눌러 가속도를 주는 상태
    bool isMoving = false;

    public Transform wallCheck;         // 벽타기 범위 
    public float wallCheckDistance;     // 벽타기 거리 
    public LayerMask wall_Layer;        // 벽을 탈 Layer 확인    
    bool isWall;                        // 벽을 타는 상태인가 

    public bool isEnemyControll = false;
    public int collierEnemyId;

    // 스크립트
    GrapplingHook grappling;
    AfterImage afterImage;
    GameKeyboard Keyboard;

    // 컴포넌트
    public Rigidbody2D rigid;
    public CircleCollider2D circleCollider;
    public SpriteRenderer spriteRenderer;

    Camera mainCamera;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Keyboard = GameKeyboard.instance.GetComponent<GameKeyboard>();

        grappling = GetComponent<GrapplingHook>();
        afterImage = GetComponent<AfterImage>();

        mainCamera = Camera.main;
    }

    void Start() {
        health = 4;
    }

    void Update()
    {
        PlayerMove();
        PlayerHook();
        PlayerJupm();
        PlayerWallMove();
        PlayerAttack();
        // MAX 속도 
        Vector2 velocity = rigid.velocity;
        if (velocity.magnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
            rigid.velocity = velocity;
        }
    }

    void PlayerMove() {
        if (!isWall) {
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove)) && grappling.isAttach) {
                rigid.AddForce(Vector3.left * speed * Time.deltaTime, ForceMode2D.Impulse);
                spriteRenderer.flipX = true;
                wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
                wallCheck.gameObject.SetActive(true);
            }
            else if (Input.GetKey((Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) && grappling.isAttach) {
                rigid.AddForce(Vector3.right * speed * Time.deltaTime, ForceMode2D.Impulse);
                spriteRenderer.flipX = false;
                wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
                wallCheck.gameObject.SetActive(true);
            }
            else if (Input.GetKey((Keyboard.GetKeyCode(KeyCodeTypes.LeftMove)))) {
                transform.position += Vector3.left * speed * Time.deltaTime;
                spriteRenderer.flipX = true;
                wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
                wallCheck.gameObject.SetActive(true);
            }
            else if (Input.GetKey((Keyboard.GetKeyCode(KeyCodeTypes.RightMove)))) {
                transform.position += Vector3.right * speed * Time.deltaTime;
                spriteRenderer.flipX = false;
                wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
                wallCheck.gameObject.SetActive(true);
            }
        }
        else {
            if (spriteRenderer.flipX == true && Input.GetKey((Keyboard.GetKeyCode(KeyCodeTypes.RightMove)))) {
                wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
            }
            else if(spriteRenderer.flipX == false && Input.GetKey((Keyboard.GetKeyCode(KeyCodeTypes.LeftMove)))) {
                wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
            }
        }
    }

    void PlayerHook() {
        if (!isAttack) {
            isMoving = false;
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove)) && Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.HookAcceleration)) && !isAcceleration && grappling.isAttach) {
                afterImage.particle.gameObject.SetActive(true);
                isMoving = true;
                StartCoroutine(DelayedAcceleration(Vector3.left));
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove)) && Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.HookAcceleration)) && !isAcceleration && grappling.isAttach) {
                afterImage.particle.gameObject.SetActive(true);
                isMoving = true;
                StartCoroutine(DelayedAcceleration(Vector3.right));
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.UpMove)) && Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.HookAcceleration)) && grappling.isAttach) {
                afterImage.particle.gameObject.SetActive(true);
                
                Vector3 dirPos = transform.position - grappling.hook.position;
                rigid.AddForce(dirPos.normalized * 10 * Time.deltaTime, ForceMode2D.Force);

                float dir = transform.position.x - grappling.hook.gameObject.transform.position.x;
                if (dir < 0) {
                    spriteRenderer.flipX = false;
                    wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
                }
                else {
                    spriteRenderer.flipX = true;
                    wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
                }

                if (grappling.hook.GetComponent<Hooking>().joint2D.distance > 0.5f)
                    grappling.hook.GetComponent<Hooking>().joint2D.distance -= 0.05f;

                isMoving = true;
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.DownMove)) && Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.HookAcceleration)) && grappling.isAttach) {
                Vector3 dirPos = transform.position - grappling.hook.position;
                rigid.AddForce(dirPos.normalized * 10 * Time.deltaTime, ForceMode2D.Force);

                float dir = transform.position.x - grappling.hook.gameObject.transform.position.x;
                if (dir < 0) {
                    spriteRenderer.flipX = false;
                    wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
                }
                else {
                    spriteRenderer.flipX = true;
                    wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
                }

                if (grappling.hook.GetComponent<Hooking>().joint2D.distance <= grappling.maxDistance) {
                    grappling.hook.GetComponent<Hooking>().joint2D.distance += 0.005f;
                    grappling.hook.GetComponent<Hooking>().distanceJoint2D.distance = grappling.hook.GetComponent<Hooking>().joint2D.distance;
                }
            }
            else if (!Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.UpMove)) && 
                     !Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove)) &&
                     !Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove)) &&
                      Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.HookAcceleration)) && grappling.isAttach) {
                Vector3 dirPos = transform.position - grappling.hook.position;
                rigid.AddForce(dirPos.normalized * 10 * Time.deltaTime, ForceMode2D.Force);

                float dir = transform.position.x - grappling.hook.gameObject.transform.position.x;
                if (dir < 0) {
                    spriteRenderer.flipX = false;
                    wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
                }
                else {
                    spriteRenderer.flipX = true;
                    wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
                }

                if (grappling.hook.GetComponent<Hooking>().joint2D.distance > 0.5f)
                    grappling.hook.GetComponent<Hooking>().joint2D.distance -= 0.005f;

                isMoving = true;
            }

            if (!isMoving && !isAcceleration) {
                afterImage.particle.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator DelayedAcceleration( Vector3 direction ) {
        // 멈추기 전에 이동 멈춤
        rigid.velocity = Vector3.zero;
        rigid.gravityScale = 0;
        yield return new WaitForSeconds(0.05f); // 0.1초 지연

        isAcceleration = true;
        // 지연 후 이동 시작
        rigid.AddForce(direction * 800 * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.25f);

        rigid.gravityScale = 2;
        afterImage.particle.gameObject.SetActive(false);
    }

    void PlayerJupm() {
        if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump)) && !isJumpCheck) {
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
            if(jumpCount == 2) {
                isJumpCheck = true;
            }
        }
    }

    void PlayerWallMove() {
        // 벽 타기
        if (wallCheck.gameObject.activeSelf == false) {
            return;
        }

        if (spriteRenderer.flipX)
            isWall = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, wall_Layer);
        else
            isWall = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wall_Layer);

        if (isWall) {
            afterImage.particle.gameObject.SetActive(false);
            rigid.gravityScale = 0;
            rigid.velocity = Vector2.zero;
            jumpCount = 0;
            isJumpCheck = false;

            if (grappling.isAttach) {
                rigid.gravityScale = 2;
            }

            if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump))) {
                rigid.gravityScale = 2;

                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

                wallCheck.gameObject.SetActive(false);
                isWall = false;
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.UpMove))) {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.DownMove))) {
                transform.position += Vector3.down * 2f * speed * Time.deltaTime;
                rigid.gravityScale = 2;
            }
        }
        else if (!isWall && !grappling.isAttach) {
            rigid.gravityScale = 2;
        }
    }


    void PlayerAttack() {
        if (isAttack) {
            afterImage.particle.gameObject.SetActive(true);
            circleCollider.enabled = true;

            transform.position = grappling.hook.transform.position;

            float dir = transform.position.x - grappling.hook.gameObject.transform.position.x;
            if (dir < 0) {
                spriteRenderer.flipX = false;
                wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
            }
            else {
                spriteRenderer.flipX = true;
                wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
            }
            grappling.hook.GetComponent<Hooking>().joint2D.distance = 0f;
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.gameObject.CompareTag("Enemy") && isAttack) {
            collierEnemyId = collision.GetComponent<PlayerOfEnemyMoveController>().id;
        }
    }

    void OnTriggerStay2D( Collider2D collision ) {
        if (collision.gameObject.CompareTag("Enemy") && isAttack) {
            isEnemyControll = true;         
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.Attack))) {
                grappling.hook.SetParent(grappling.hook.gameObject.GetComponent<Hooking>().originalParent);
                grappling.hook.gameObject.GetComponent<Hooking>().hookedObject = null;

                Vector3 playerPosition = transform.position;
                Vector3 mouseScreenPosition = Input.mousePosition;
                Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.nearClipPlane));

                // 플레이어 위치에서 마우스 위치로의 방향 벡터 계산
                Vector3 directionToMouse = mouseWorldPosition - playerPosition;

                // 반대 방향 계산
                Vector3 oppositeDirection = -directionToMouse.normalized;

                // 일정 속도로 날아가게 설정 (예: 10 유닛/초)
                float flySpeed = 40f;
                rigid.velocity = oppositeDirection * flySpeed;

                isAttack = false;
                isEnemyControll = false;
                collision.gameObject.SetActive(false);
                circleCollider.enabled = false;
            }
        }
    }

    void OnCollisionEnter2D( Collision2D collision ) {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Ring")) {
            jumpCount = 0;
            isJumpCheck = false;
        }
    }
}
