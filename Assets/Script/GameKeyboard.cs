using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Input Manager �� Ű ���� �����ϴ� ��ũ��Ʈ 
 * 
 * Ű������ Ű���� �ѹ��� �����ϱ� ���� ���� 
 * ���� �������� Ű������ ����� ���� �̸� �����ϴ� �� 
**/

public class GameKeyboard : MonoBehaviour
{
    // Ű����Ŵ��� �̱��� 
    public static GameKeyboard instance;

    // ������ ���� ���� 
    public enum KeyCodeTypes {
        LeftMove,
        RightMove,
        UpMove,
        DownMove,
        Jump,
        Hook,
        Attack,
        HookAcceleration
    }

    // ��ųʸ��� Ű ���� 
    private Dictionary<KeyCodeTypes, KeyCode> keyMappings;

    void Awake() {
        instance = this;
        // ��ųʸ� �ʱ�ȭ 
        keyMappings = new Dictionary<KeyCodeTypes, KeyCode>();
        
        // �� ��ųʸ� Ű�� �´� Ű���� ���� �߰� 
        keyMappings[KeyCodeTypes.LeftMove] = KeyCode.A;
        keyMappings[KeyCodeTypes.RightMove] = KeyCode.D;
        keyMappings[KeyCodeTypes.UpMove] = KeyCode.W;
        keyMappings[KeyCodeTypes.DownMove] = KeyCode.S;
        keyMappings[KeyCodeTypes.Jump] = KeyCode.Space;
        keyMappings[KeyCodeTypes.Hook] = KeyCode.Mouse0;
        keyMappings[KeyCodeTypes.Attack] = KeyCode.Mouse1;
        keyMappings[KeyCodeTypes.HookAcceleration] = KeyCode.LeftShift;        
    }

    public KeyCode GetKeyCode( KeyCodeTypes action ) {
        // Ű�� ��ȯ 
        return keyMappings[action];
    }

    public void SetKeyCode( KeyCodeTypes action, KeyCode keyCode ) {
        // Ű�� ���� 
        keyMappings[action] = keyCode;
    }

    void Update() {
        // ���� â ����� �� TextInput â�� ���� �ݿ���Ű�� ���� �߰� SetKeyCode() ��� 
    }
}
