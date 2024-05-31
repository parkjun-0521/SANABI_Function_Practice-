using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ������Ʈ Ǯ�� ��ũ��Ʈ 
 * 
 * �޸𸮸� ȿ�������� ����ϱ� ���ؼ� ������Ʈ Ǯ���� ���� 
 * �ش� ������Ʈ Ǯ���� List�� �����Ͽ��� 
 * ������Ʈ�� �����Ǿ������� �Ǵ� 
 * ���� �Ǿ��ٸ� ��Ȱ��ȭ, Ȱ��ȭ�� �Ǵ��Ͽ� ������Ʈ�� ��Ȱ���ϰų� ������ 
 * ������Ʈ�� ���ٸ� �� ������Ʈ�� �����Ͽ� Pool�� ��Ƶд�.
**/

public class Pooling : MonoBehaviour {
    public GameObject[] prefabs;        // ������ ������Ʈ ������ 
    List<GameObject>[] pools;           // ������ ������Ʈ�� ���� ���� 

    void Awake() {
        // ����Ʈ ũ�� �ʱ�ȭ 
        pools = new List<GameObject>[prefabs.Length];

        // �������� ���� ��ŭ �⺻������ Pool ������ �ʱ�ȭ 
        for (int index = 0; index < prefabs.Length; index++) {
            pools[index] = new List<GameObject>();
        }
    }

    // prefabs �� ��ġ ��, �迭�� ��ġ ������ ������Ʈ�� ���� 
    public GameObject GetObject( int index ) {
        GameObject select = null;
        foreach (GameObject objects in pools[index]) {
            // ������Ʈ�� ��Ȱ��ȭ �Ǿ��ִ��� �Ǵ� 
            if (!objects.activeSelf) {
                // ��Ȱ��ȭ�� ������Ʈ�� Ȱ��ȭ �� ( ��, ����� �� �ִ� ���·� ��ȯ (������Ʈ ��Ȱ��) ) 
                select = objects;
                objects.SetActive(true);
                break;
            }
        }
        // ������Ʈ�� ���ٸ� 
        if (!select) {
            // ������Ʈ�� ���� 
            select = Instantiate(prefabs[index], transform);
            // ������ ������Ʈ�� Pool�� ��´�.
            pools[index].Add(select);
        }

        // �ش� ������Ʈ�� ��ȯ ( ��� ) 
        return select;
    }
}
