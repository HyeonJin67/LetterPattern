using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest : MonoBehaviour, ICheck
{
    //UI�� ��ġ�Ǿ� ���� �ε����� �����ϴ� ����
    [SerializeField]
    int nextindex;
    [SerializeField]
    Vector2 nextVec;
    public (int, Vector2) CheckNext() //��ȯ���� �ΰ����� Ʃ��
    {
        return (nextindex, nextVec);
    }
}

//����Ȯ���� ���� �������̽� ���� �� CheckNext()�Լ� ����!
interface ICheck 
{
    public (int, Vector2) CheckNext();
    
}


