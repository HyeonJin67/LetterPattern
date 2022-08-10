using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//������ �´��� üũ
public class CheckDirec : MonoBehaviour
{
    [Tooltip("��� ȭ��ǥ�� �־��ּ���")]
    [SerializeField]
    Sprite[] arrowSprites;
    [Tooltip("ȹ�� �߿� �ִ� ���̴� ������ ���̵��� ������� �־��ּ���")]
    [SerializeField]
    Image[] spotArrow;
    

    public Func<Vector2, Vector2, bool> CheckDir; //Func<�Ű�����,�Ű�����,��ȯ��> �̺�Ʈ
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

