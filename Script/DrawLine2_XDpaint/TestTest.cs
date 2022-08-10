using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTest : MonoBehaviour, ICheck
{
    //UI에 배치되어 다음 인덱스를 지정하는 역할
    [SerializeField]
    int nextindex;
    [SerializeField]
    Vector2 nextVec;
    public (int, Vector2) CheckNext() //반환값을 두개갖는 튜플
    {
        return (nextindex, nextVec);
    }
}

//방향확인을 위한 인터페이스 생성 및 CheckNext()함수 선언만!
interface ICheck 
{
    public (int, Vector2) CheckNext();
    
}


