using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DrawLineHJ : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabLine;
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    List<Vector2> point = new List<Vector2>();

    public GameObject arrow;
    Vector3 spot3 = new Vector3(245, 243, 0); //spot3의 위치값
    public GameObject star;
    bool mission1completed;
    [SerializeField]
    Image raycastTarget;
    [SerializeField]
    GameObject ex;
    Vector3 startpoint;

    void Start()
    {
        ex.SetActive(true);
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f; //투명한 부분은 그려지지 않게
        startpoint = arrow.transform.position;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ex.SetActive(false);
        arrow.SetActive(true);
        arrow.GetComponent<Animator>().enabled = false;
        arrow.GetComponent<Image>().enabled = true;

        //ex.GetComponent<Text>().text = "드래그 시작";
        if (OverGameObject(eventData)) //EventSystem.current.IsPointerOverGameObject() //안드로이드에서 안됨
        {
            //ex.GetComponent<Text>().text += "\nOverGameObject";
            GameObject go = Instantiate(prefabLine);
            lineRenderer = go.GetComponent<LineRenderer>();
            edgeCollider = go.GetComponent<EdgeCollider2D>();
            point.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            lineRenderer.positionCount =1;
            lineRenderer.SetPosition(0, point[0]);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        //print("델타포지션:");
        //print(eventData.delta);
        arrow.GetComponent<RectTransform>().transform.position = eventData.position;
        
        if (OverGameObject(eventData))
        {

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(point[point.Count-1], pos) > 0.1f)
            {
                if (eventData.delta.x >= 0)
                {
                    point.Add(pos);
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount-1, pos);
                    edgeCollider.points = point.ToArray();

                    GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();
                    List<RaycastResult> result = new List<RaycastResult>();
                    raycaster.Raycast(eventData, result);

                    if (result.Count > 0)
                    {
                        foreach (var a in result)
                        {
                            print("알파테스트"+a.gameObject.name +a.gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold);
                            if (a.gameObject.name == "Spot3")
                            {
                                //파라미터 체인지 : 변수... : 판단할 거리를 바꾸어 x를 그 이후엔 y를 바꾸는 형태로...
                                if (eventData.delta.y <= 0)
                                {
                                    //arrow.transform.localPosition = spot3; //position/localposition과의 차이점
                                    arrow.transform.rotation= Quaternion.Euler(0, 0, -90);
                                    //arrow.SetActive(true);
                                    mission1completed = true;
                                }
                            }
                        }
                    }
                }
                //else if()
                //{

                //}
                //if(eventData.delta.x < 0)
                //{
                //    print("거꾸로 그렸음!");

                //    lineRenderer.enabled = false; //Destroy(lineRenderer); 
                //}
            }
        }
        //else
        //{
        //    print("마우스포인트 밖으로 나감");
        //    arrow.SetActive(false);
        //    if (lineRenderer)
        //    {
        //        lineRenderer.enabled = false;
        //    }
        //}
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        print("충돌알파값"+gameObject.name+ ": " +gameObject.GetComponent<Image>().color.a);
        //ex.GetComponent<Text>().text += "드래그 끝남";
        if (OverGameObject(eventData))
        {
            point.Clear();

            GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();
            List<RaycastResult> result = new List<RaycastResult>();
            raycaster.Raycast(eventData, result);

            foreach (var h in result)
            {
                if (result != null) //result[0].gameObject.name.Contains("Spot1")
                {
                    print("첫번째충돌감지!");
                    //foreach (var a in result)
                    //    print(a);
                    //if (result[0].gameObject.name.Contains("Spot3"))
                    //{
                    //     //transform.GetChild(0).GetComponent<Image>().fillAmount = 1; //GetComponentInChildren<Image>().fillAmount = 1;
                    //    //raycastTarget.raycastTarget = false; //Spot1의 raycaster target 꺼주기

                    //    result[0].gameObject.GetComponent<Image>().raycastTarget = false;
                    //}

                    if (h.gameObject.GetComponent<Image>().name.Contains("Spot6"))
                    {
                        if (mission1completed == true)
                        {
                            //print("두번째충돌감지!");
                            arrow.SetActive(false);
                            Destroy(lineRenderer);
                            StartCoroutine(Fill(0));
                            //if (GetComponentInChildren<Image>().fillAmount > 0.8f)
                            //{
                            //    StartCoroutine(Fill(1));
                            //} //transform.GetChild(1).GetComponent<Image>().fillAmount = 1; //GetComponentInChildren<Image>().fillAmount = 1;
                            star.SetActive(true);
                            print("완성!");
                        }
                        else
                        {

                            lineRenderer.enabled = false;
                            arrow.SetActive(true);
                            arrow.transform.position = startpoint;
                            print("순서에 맞게 다시 해봅시다");
                        }
                    }
                    else
                    {
                        print("별까지 닿게 그려보세요");
                        arrow.transform.position = startpoint;
                        if (lineRenderer)
                        {
                            lineRenderer.enabled = false;
                        }
                        //result[0].gameObject.GetComponent<Image>().raycastTarget = false;
                    }
                }
            }
        }
        //else
        //{
        //    print("마우스포인트 밖으로 나감");
        //    arrow.SetActive(false);
        //    if (lineRenderer)
        //    {
        //        lineRenderer.enabled = false;
        //    }
        //}
    }
    IEnumerator Fill(int index)
    {
        float i;
        for (i = transform.GetChild(index).GetComponent<Image>().fillAmount; i<=1; i+=0.01f)
        {
            transform.GetChild(index).GetComponent<Image>().fillAmount  = i;
            yield return new WaitForSeconds(0.02f);
        }
        if (i > 1 && transform.childCount > index+1)
        {
            StartCoroutine(Fill(index+1));
            yield break;
        }
        yield break;
    }
    public bool OverGameObject(PointerEventData eventData)
    {
        int index = 0;
        GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();
        List<RaycastResult> result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);

        foreach (var item in result)
        {
            if (item.gameObject.transform.GetSiblingIndex() == index)
            {
                return true;
            }
        }
        return false;
        //if (result.Count > 0)
        //{
        //    if (result[0].gameObject.transform.root.GetSiblingIndex() == gameObject.transform.root.GetSiblingIndex()) 
        //        return true;
        //}
    }
}








/*

void Update()
{
    if (EventSystem.current.IsPointerOverGameObject())
    {
        if (Input.GetMouseButtonDown(0))
        {
            arrow.SetActive(false);
            GameObject go = Instantiate(prefabLine);
            lineRenderer = go.GetComponent<LineRenderer>();
            edgeCollider = go.GetComponent<EdgeCollider2D>();
            point.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //foreach (var a in point)
            //{
            //    print("마우스클릭위치:" + a);
            //}
            lineRenderer.positionCount =1;
            lineRenderer.SetPosition(0, point[0]);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(point[point.Count-1], pos)>0.1f)
            {
                point.Add(pos);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount-1, pos);
                edgeCollider.points = point.ToArray();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            point.Clear();
            GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
            List<RaycastResult> result = new List<RaycastResult>();
            //raycaster.Raycast(eventData, result);
            if (result.Count >= 2) //
            {
                print("첫번째충돌감지!");
                arrow.transform.localPosition = spot1; //position/localposition과의 차이점
                arrow.SetActive(true);
            }
        }
    }
    else
    {
        print("ㄱ위에 그려보세요");
    }
}
*/


