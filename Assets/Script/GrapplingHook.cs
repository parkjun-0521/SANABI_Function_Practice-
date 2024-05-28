using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameKeyboard;

public class GrapplingHook : MonoBehaviour {
    public LineRenderer line;       // ���� ������ assinged �� ����� �Ѵ�. 
    public Transform hook;

    public float speed = 10;
    public float backSpeed= 200;
    public float maxDistance;

    Vector2 mousedir;           // ���� ���콺�� ������ �����ϱ� ���� ���� 

    public bool isHookAction;
    public bool isLineMax;

    public bool isAttach;
    public bool isGround = false;

    GameKeyboard Keyboard;
    Player player;

    void Awake() {
        Keyboard = GameKeyboard.instance.GetComponent<GameKeyboard>();
        player = GetComponent<Player>();    
    }

    void Start() {
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    void Update() {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !isHookAction) {

            isAttach = false;
            isLineMax = false;
            hook.GetComponent<Hooking>().joint2D.enabled = false;
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            hook.gameObject.SetActive(false);

            // ���� ���콺 �����Ϳ� Player�� ��ġ ������ ������ ���� 
            hook.position = transform.position;
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            isHookAction = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
        }

        if (isHookAction && !isLineMax && !isAttach) {
            hook.Translate(mousedir.normalized * Time.deltaTime * speed);
            // Player�� ��ġ�� hook�� ��ġ�� ���̰� 5�� ���� �� 
            if (Vector2.Distance(transform.position, hook.position) > maxDistance) {
                isLineMax = true;
            }
        }
        else if (isHookAction && isLineMax) {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * backSpeed);
            if (Vector2.Distance(transform.position, hook.position) < 0.1f) {
                isHookAction = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
        }
        else if (isAttach) {
            if (!Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !player.isAttack) { 
                // �÷��̾� ���� �ʱ�ȭ 
                player.isJumpCheck = false;
                player.jumpCount = 1;
                player.isAcceleration = false;
                player.rigid.gravityScale = 2;

                isAttach = false;
                isHookAction = false;
                isLineMax = false;
                hook.GetComponent<Hooking>().joint2D.enabled = false;
                hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
                hook.gameObject.SetActive(false);

                if(Vector2.Distance(transform.position, hook.position) > maxDistance) {
                    hook.GetComponent<Hooking>().joint2D.distance = maxDistance;
                }

                hook.SetParent(hook.gameObject.GetComponent<Hooking>().originalParent);
                hook.gameObject.GetComponent<Hooking>().hookedObject = null;
            }

            // �÷��̾ ���� �ְ� ��ũ�� �ɷ��ִ��� Ȯ��
            if (isGround) {
                AdjustHookLength();
            }
        }
    }

    void AdjustHookLength() {
        // ���� �÷��̾� ��ġ�� ��ũ ��ġ ���� �Ÿ� ���
        float distanceChange = speed * Time.deltaTime;

        Hooking hookScript = hook.gameObject.GetComponent<Hooking>();

        float targetDistance = hookScript.joint2D.distance; // ��ǥ �Ÿ�

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
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            isGround = true;
        }
    }

    private void OnCollisionExit2D( Collision2D collision ) {
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = true;
            hook.GetComponent<Hooking>().distanceJoint2D.distance = hook.GetComponent<Hooking>().distanceJoint2D.distance;
            isGround = false;
        }
    }
}
