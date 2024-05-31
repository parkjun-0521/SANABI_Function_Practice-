using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameKeyboard;

/**
 * ���� �߻� ���� ��ũ��Ʈ 
 * 
 * ���� �׼��� ���� ������ �߻縦 ����
 * ���� ���콺�� ��ġ �������� ������ �߻�ǵ��� �� 
**/

public class GrapplingHook : MonoBehaviour {
    public float speed;             // ���� �߻� �ӵ� 
    public float backSpeed= 200;    // ���� ���ƿ��� �ӵ� 
    public float maxDistance;       // ���� �ִ� �Ÿ� 

    public bool isHookAction;       // ������ ���ư����� �Ǵ� 
    public bool isLineMax;          // ������ �ִ�Ÿ����� �Ǵ� 
    public bool isAttach;           // ������ �پ��ִ��� �Ǵ� 

    public bool isGround = false;   // �ٴ����� Ȯ�� ���� ( ������ �ɰ� �̵��� �Ϸ��� ���� ) 

    Vector2 mousedir;               // ���� ���콺�� ������ �����ϱ� ���� ����

    // ������Ʈ �� ��ũ��Ʈ 
    public LineRenderer line;      
    public Transform hook;
    GameKeyboard Keyboard;
    Player player;

    void Awake() {
        Keyboard = GameKeyboard.instance.GetComponent<GameKeyboard>();
        player = GetComponent<Player>();    
    }

    void Start() {
        // LinRenderer�� �ʱⰪ ���� 
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        // ó�� ��ġ, ������ ��ġ ���� 
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    void Update() {
        // ���� �����Ͽ� ���� �� ���� ���� �߻縦 ���ϰ� ���� 
        if (GameManager.instance.player.GetComponent<Player>().isEnemyControll) {
            return;
        }

        // ���� ��ġ�� ������ ��ġ�� ���������� ���� 
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        // ���� �߻� ( ���콺 ��Ŭ�� ) 
        if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !isHookAction) {
            // �߻� �ʱ⿡ ���� �ʱ�ȭ 
            isAttach = false;
            isLineMax = false;
            hook.GetComponent<Hooking>().joint2D.enabled = false;
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            hook.gameObject.SetActive(false);

            // ���� ��ġ ���� 
            hook.position = transform.position;
            // ���� ���콺 �����Ϳ� Player�� ��ġ ������ ������ ���� 
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            isHookAction = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
        }

        // ���� �׼� ó�� 
        if (isHookAction && !isLineMax && !isAttach) {      // ������ ���ư��µ� �ִ�Ÿ��� �������� ��� 
            hook.Translate(mousedir.normalized * Time.deltaTime * speed);
            // Player�� ��ġ�� hook�� ��ġ�� ���̰� �ִ밡 ���� �� 
            if (Vector2.Distance(transform.position, hook.position) > maxDistance) {
                isLineMax = true;
            }
        }
        else if (isHookAction && isLineMax) {               // �ִ�Ÿ��� �������� �� ���ƿ��� ��� 
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * backSpeed);
            // ������ ���̰� Player�� ���� �������� �� ���� �ʱ�ȭ 
            if (Vector2.Distance(transform.position, hook.position) < 0.1f) {
                isHookAction = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
        }       
        else if (isAttach) {                                // ������ Ư�� ������Ʈ�� �پ��� ��� 
            // ������ �پ��� �� ���콺 ��ư�� ���� ��� ( ��, ������ ������ �� ) 
            if (!Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !player.isAttack) { 
                // �÷��̾� ���� �ʱ�ȭ 
                player.isJumpCheck = false;
                player.jumpCount = 1;
                player.isAcceleration = false;
                player.rigid.gravityScale = 2;

                // ���� �ʱ�ȭ 
                isAttach = false;
                isHookAction = false;
                isLineMax = false;
                
                // ���� ��Ȱ��ȭ  
                hook.GetComponent<Hooking>().joint2D.enabled = false;
                hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
                hook.gameObject.SetActive(false);

                // �ִ� �Ÿ� ���� 
                if(Vector2.Distance(transform.position, hook.position) > maxDistance) {
                    hook.GetComponent<Hooking>().joint2D.distance = maxDistance;
                }

                // ������ �پ��ִ� �θ� ������Ʈ �ʱ�ȭ 
                hook.SetParent(hook.gameObject.GetComponent<Hooking>().originalParent);
                hook.gameObject.GetComponent<Hooking>().hookedObject = null;
            }

            // �÷��̾ ���� �ְ� ��ũ�� �ɷ��ִ��� Ȯ��
            if (isGround) {
                AdjustHookLength();
            }
        }
    }

    // ����ϰ� �������� ���� ( �������� ã�ƺ��� �� �� ���� ) 
    void AdjustHookLength() {
        // ���� �÷��̾� ��ġ�� ��ũ ��ġ ���� �Ÿ� ���
        float distanceChange = speed * Time.deltaTime;

        Hooking hookScript = hook.gameObject.GetComponent<Hooking>();
        float targetDistance = hookScript.joint2D.distance;             // ��ǥ �Ÿ�

        // ��ũ�� �÷��̾�� �����ʿ� �ִ� ���
        if (hook.position.x > transform.position.x) {
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove))) {
                // �÷��̾ �������� �̵��ϸ� ���� ���� ����
                targetDistance = Mathf.Min(hookScript.joint2D.distance + distanceChange, maxDistance);
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) {
                // �÷��̾ ���������� �̵��ϸ� ���� ���� ����
                targetDistance = Mathf.Max(hookScript.joint2D.distance - distanceChange, 0);
            }
        }
        // ��ũ�� �÷��̾�� ���ʿ� �ִ� ���
        else if (hook.position.x < transform.position.x) {
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove))) {
                // �÷��̾ �������� �̵��ϸ� ���� ���� ����
                targetDistance = Mathf.Max(hookScript.joint2D.distance - distanceChange, 0);
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) {
                // �÷��̾ ���������� �̵��ϸ� ���� ���� ����
                targetDistance = Mathf.Min(hookScript.joint2D.distance + distanceChange, maxDistance);
            }
        }
        // ��ũ�� �÷��̾��� ��ġ�� ���� ���
        else {
            // �÷��̾ �̵��ص� ���� ���̸� ����
            targetDistance = hookScript.joint2D.distance;
        }

        // ���� ������ ����Ͽ� ��ǥ �Ÿ��� �ε巴�� �̵�
        hookScript.joint2D.distance = Mathf.Lerp(hookScript.joint2D.distance, targetDistance, Time.deltaTime * player.speed * 10);
    }

    void OnCollisionStay2D( Collision2D collision ) {
        // ������ �ɷ��ְ� ���鿡 ���� ������ �� 
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            isGround = true;
        }
    }

    private void OnCollisionExit2D( Collision2D collision ) {
        // ������ �ɷ��ְ� ���鿡 ���� ������ �� 
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = true;
            // ������ �Ÿ��� ���� �Ÿ��� ���� 
            hook.GetComponent<Hooking>().distanceJoint2D.distance = hook.GetComponent<Hooking>().joint2D.distance;
            isGround = false;
        }
    }
}