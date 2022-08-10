using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//방향이 맞는지 체크
public class CheckDirec : MonoBehaviour
{
    [Tooltip("모든 화살표를 넣어주세요")]
    [SerializeField]
    Sprite[] arrowSprites;
    [Tooltip("획순 중에 있는 꺾이는 구간의 스팟들을 순서대로 넣어주세요")]
    [SerializeField]
    Image[] spotArrow;
    

    public Func<Vector2, Vector2, bool> CheckDir; //Func<매개변수,매개변수,반환값> 이벤트
    private void Awake()
    {
        CheckDir += (a, b) =>
        {
            return Vector2.Dot(a.normalized, b.normalized) >= 0;
        };
        ArrowChangeCheck();
    }
    void ArrowChangeCheck()
    {
        spotArrow[0].sprite = arrowSprites[1];
        spotArrow[0].gameObject.SetActive(true);
        
    }
}

