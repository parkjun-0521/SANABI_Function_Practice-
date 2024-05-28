using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {
    // Ŀ�� �̹���
    public Texture2D cursorTexture;

    void Start() {
        // Ŀ���� �߽��� ���� (�ֽ����� �̹����� �߽����� ����)
        Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        // Ŀ���� �����ϰ� �ֽ����� ����
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}