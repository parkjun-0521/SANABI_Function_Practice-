using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 오브젝트 풀링 스크립트 
 * 
 * 메모리를 효율적으로 사용하기 위해서 오브젝트 풀링을 구현 
 * 해당 오브젝트 풀링을 List로 구현하였음 
 * 오브젝트가 생성되었는지를 판단 
 * 생성 되었다면 비활성화, 활성화를 판단하여 오브젝트를 재활용하거나 생성함 
 * 오브젝트가 없다면 그 오브젝트를 생성하여 Pool에 담아둔다.
**/

public class Pooling : MonoBehaviour {
    public GameObject[] prefabs;        // 생성될 오브젝트 프리펩 
    List<GameObject>[] pools;           // 생성된 오브젝트를 담을 공간 

    void Awake() {
        // 리스트 크기 초기화 
        pools = new List<GameObject>[prefabs.Length];

        // 프리펩의 개수 만큼 기본적으로 Pool 공간을 초기화 
        for (int index = 0; index < prefabs.Length; index++) {
            pools[index] = new List<GameObject>();
        }
    }

    // prefabs 의 위치 즉, 배열의 위치 값으로 오브젝트를 생성 
    public GameObject GetObject( int index ) {
        GameObject select = null;
        foreach (GameObject objects in pools[index]) {
            // 오브젝트가 비활성화 되어있는지 판단 
            if (!objects.activeSelf) {
                // 비활성화면 오브젝트를 활성화 함 ( 즉, 사용할 수 있는 상태로 전환 (오브젝트 재활용) ) 
                select = objects;
                objects.SetActive(true);
                break;
            }
        }
        // 오브젝트가 없다면 
        if (!select) {
            // 오브젝트를 생성 
            select = Instantiate(prefabs[index], transform);
            // 생성한 오브젝트를 Pool에 담는다.
            pools[index].Add(select);
        }

        // 해당 오브젝트를 반환 ( 사용 ) 
        return select;
    }
}
