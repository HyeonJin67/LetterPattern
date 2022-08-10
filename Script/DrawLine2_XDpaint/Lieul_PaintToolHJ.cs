using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//칠하기 기능 예시 : Canvas는 무조건 Overlay에서만 작동
public class Lieul_PaintToolHJ : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region 변수
    //기본 페인트 매니저
    [SerializeField]
    XDPaint.PaintManager paintManager;
    //색칠할 것
    [SerializeField]
    UnityEngine.UI.RawImage paintOb;
    //처음부터 화살표를 배열로 받기... : 배열은 순서: 화살표 순서를 알려주는 인덱스 파라미터(변수)를 변경하는 것은 DragObjectCheck하는 쪽에서 늘려주는 형태로 : ++로하면 > 여러번 지나면>>버그 생성
    //길이를 임의로 사람이 배치할 수 있음 > Inspector창에서 수정 : 화살표 인덱스 파라미터

    //[SerializeField]
    //Image[] arrowArray;
    [SerializeField]
    RawImage puzzle;
    
    [SerializeField]
    Image arrow2;
    [SerializeField]
    GameObject arrow3;
    Vector3 startPosition;
    bool missionCompleted;
    public GameObject star;
    int a = 0;
    /*
    private enum Direction { Up, Down, Left, Right };
    [SerializeField]
    private Direction direction;
    */

    Vector2 horizontal = new Vector2(1, 0);
    Vector2 vertical = new Vector2(0, -1);

    #endregion


    void Start()
    {
        startPosition = transform.position;
        paintManager.Init();
        paintManager.PaintObject.ProcessInput = false;

    }
    //드래그 시작시
    public void OnBeginDrag(PointerEventData eventData)
    {
        //arrow2.GetComponent<Image>().enabled = true;
        
            transform.position = eventData.position;
            print(" 화살이랑 충돌 확인");
            missionCompleted = false;
            GetComponent<Image>().raycastTarget = false;
        
    }
    //커브에서 꺾임 체크 함수
    void TurnCheck()
    {
        missionCompleted = true;
    }
    //드래그중
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        bool check1 = GetComponent<ForDirection>().CheckDirection(horizontal, eventData);
        bool check2 = GetComponent<ForDirection>().CheckDirection(vertical, eventData);

        paintManager.PaintObject.OnMouseButton(eventData.position);
        GameObject ob = Meet(eventData);
        if (ob != null) //&& arrow2.raycastTarget == false
        {
            if (missionCompleted == false)
            {
                paintManager.PaintObject.ProcessInput = true;
                if (check1)
                {
                    if (ob.name == "Spot1")
                    {
                        TurnCheck();
                        paintManager.PaintObject.ProcessInput = true;
                        print("꺾임");
                        transform.rotation = Quaternion.Euler(0, 0, -90);
                        arrow3.SetActive(true);
                    }
                }
                else if (check1 ==false)
                {
                    print("x축틀림");
                    paintManager.PaintObject.OnMouseUp(eventData.position);
                    paintManager.PaintObject.ProcessInput = false;
                    ResetTexture();
                }
            }
            else if (missionCompleted)
            {
                paintManager.PaintObject.ProcessInput = true;
                if (check2)
                {
                    //missionCompleted = false;
                    paintManager.PaintObject.ProcessInput = true;
                    print("y축맞음");
                    arrow3.SetActive(true);
                }
                else
                {
                    print("y축틀림");
                    paintManager.PaintObject.OnMouseUp(eventData.position);
                    paintManager.PaintObject.ProcessInput = false;
                    ResetTexture();
                }
            }
        }
        else
        {
            paintManager.PaintObject.OnMouseUp(eventData.position);
            paintManager.PaintObject.ProcessInput = false;
            ResetTexture();
        }
        /*
        GameObject ob = Meet(eventData);
        if (ob != null && ob.name == "Puzzle")
        {
            bool check1 = GetComponent<Tessst>().CheckDirection(horizontal, eventData);
            if (check1)
            {
                paintManager.PaintObject.ProcessInput = true;
            }
            else paintManager.PaintObject.ProcessInput = false;
        }
        else if(ob.name == "Spot")
        {
            missionCompleted = true;
            
        }
        */
    }
    //드래그 끝났을 때
    public void OnEndDrag(PointerEventData eventData)
    {

        GameObject ob = Meet(eventData);
        GetComponent<Image>().raycastTarget = true;
        if (ob != null)
        {
            if (missionCompleted && ob.name == "fin")
            {
                puzzle.transform.GetChild(1).gameObject.SetActive(true);
                //transform.GetChild(1).gameObject.SetActive(true);
                StartCoroutine(Fill(0));
                arrow2.enabled = false;
                arrow3.SetActive(false);
                paintManager.PaintObject.OnMouseUp(eventData.position);
                paintManager.PaintObject.ProcessInput = false;
                ResetTexture();
            }
            else
            {
                if (missionCompleted) arrow2.transform.rotation = Quaternion.Euler(0, 0, 0);
                paintManager.PaintObject.OnMouseUp(eventData.position);
                paintManager.PaintObject.ProcessInput = false;
                ResetTexture();
                arrow2.GetComponent<RectTransform>().transform.position = startPosition;
            }
        }
        else
        {
            if (missionCompleted) arrow2.transform.rotation = Quaternion.Euler(0, 0, 0);
            paintManager.PaintObject.OnMouseUp(eventData.position);
            paintManager.PaintObject.ProcessInput = false;
            ResetTexture();
            arrow2.GetComponent<RectTransform>().transform.position = startPosition;
        }
    }
    //자음의 획에 따라 꺾이는 구간에 심어둔 충돌처리 함수 
    public GameObject Meet(PointerEventData eventData)
    {
        GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();
        List<RaycastResult> result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);
        foreach (var item in result)
        {
            //print("충돌체확인 : "+ item);
            return item.gameObject;
        }
        return null;
    }
    //자음 완성시 마스크모양으로 이미지가 채워지는 모습 지연시켜주는 함수
    IEnumerator Fill(int index)
    {
        float i;
        for (i = puzzle.transform.GetChild(1).GetChild(index).GetComponent<Image>().fillAmount; i<=1; i+=0.01f)
        {
            puzzle.transform.GetChild(1).GetChild(index).GetComponent<Image>().fillAmount  = i;
            yield return new WaitForSeconds(0.02f);
        }
        if (i > 1 && puzzle.transform.childCount > index+1)
        {
            StartCoroutine(Fill(index+1));
            yield break;
        }
        star.SetActive(true);
        yield break;
    }
    /*
    public void CheckDirectionVector(int state, PointerEventData eventData)
    {
        switch (state)
        {
            case 0://위로 그리기
                print("위로!");
                if (eventData.delta.y > 0) { paintManager.PaintObject.ProcessInput = true; } //true일때만 실행 //페인트 실행 여부 판단하는 파라미터
                    break;
            case 1://아래로 그리기
                print("아래로!");
                if (eventData.delta.y < 0) { paintManager.PaintObject.ProcessInput = true; } 
                break;
            case 2://좌로 그리기
                print("좌로!");
                if (eventData.delta.x < 0) { paintManager.PaintObject.ProcessInput = true; } 
                break;
            case 3://우로 그리기
                print("우로!");
                if (eventData.delta.x > 0) { paintManager.PaintObject.ProcessInput = true; } 
                break;
                
        }
    }
    */
    #region 함수
    //버튼 기능 : 브러시 크기 조정
    public void ChangeSlider(UnityEngine.UI.Slider slider)
    {
        paintManager.Brush.Size = slider.value;
    }
    //브러시 색깔 바꾸기
    public void ChangeColor(Color color)
    {
        paintManager.Brush.SetColor(color);
    }
    //지우기 : 마우스 포인터 부근 지우기
    public void Eraser()
    {
        paintManager.Brush.SetPaintTool(XDPaint.Core.PaintTool.Erase);
    }
    //칠하기
    public void Painting()
    {
        paintManager.Brush.SetPaintTool(XDPaint.Core.PaintTool.Brush);
    }
    //실행 취소
    public void Undo()
    {
        if (paintManager.PaintObject.TextureKeeper.CanUndo())
        {
            paintManager.PaintObject.TextureKeeper.Undo();
        }
    }
    //되돌리기
    public void Redo()
    {
        if (paintManager.PaintObject.TextureKeeper.CanRedo())
        {
            paintManager.PaintObject.TextureKeeper.Redo();
        }
    }
    //배경 제외 그린 것만
    public RenderTexture PaintOnly()
    {
        return paintManager.GetResultRenderTexture();
    }
    //결과 텍스쳐 가져오기
    public Texture2D PaintResult()
    {
        return paintManager.GetResultTexture();
    }
    //그리기 다 지우기
    public void ResetTexture()
    {
        paintManager.PaintObject.TextureKeeper.Reset();
        paintManager.PaintObject.ClearTexture();
        paintManager.PaintObject.Render();
        paintManager.Render();
    }
    #endregion

}
