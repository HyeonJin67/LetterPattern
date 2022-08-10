using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
//using Testinterface;
//ĥ�ϱ� ��� ���� : Canvas�� ������ Overlay������ �۵�
public class PaintToolHJ : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region ����
    //�⺻ ����Ʈ �Ŵ���
    [SerializeField]
    XDPaint.PaintManager paintManager;
    //��ĥ�� ��
    [SerializeField]
    UnityEngine.UI.RawImage paintOb;
    //ó������ ȭ��ǥ�� �迭�� �ޱ�... : �迭�� ����: ȭ��ǥ ������ �˷��ִ� �ε��� �Ķ����(����)�� �����ϴ� ���� DragObjectCheck�ϴ� �ʿ��� �÷��ִ� ���·� : ++���ϸ� > ������ ������>>���� ����
    //���̸� ���Ƿ� ����� ��ġ�� �� ���� > Inspectorâ���� ���� : ȭ��ǥ �ε��� �Ķ����

    [SerializeField]
    RawImage puzzle;
    
    Vector3 startPosition;
    bool missionCompleted;
    public GameObject star;
    
    /*
    private enum Direction { Up, Down, Left, Right };
    [SerializeField]
    private Direction direction;
    */

    Vector2 horizontal = new Vector2(1, 0);
    Vector2 vertical = new Vector2(0, -1);

    CheckDirec dir;
    int index;
    [Flags]
    private enum Direction
    {
        Up = 1,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        All = int.MaxValue
    };
    [SerializeField]
    List<Direction> directions;
    #endregion
    
    void Start()
    {
        startPosition = transform.position;
        paintManager.Init();
        paintManager.PaintObject.ProcessInput = false;
        dir = FindObjectOfType<CheckDirec>();
        index = 0;
    }
    //�巡�� ���۽�
    public void OnBeginDrag(PointerEventData eventData)
    {
        //arrow2.GetComponent<Image>().enabled = true;
        transform.position = eventData.position;
        print(" ȭ���̶� �浹 Ȯ��");
        missionCompleted = false;
        GetComponent<Image>().raycastTarget = false;
    }
    //Ŀ�꿡�� ���� üũ �Լ�
    void TurnCheck()
    {
        missionCompleted = true;
    }
    //�巡����
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        paintManager.PaintObject.OnMouseButton(eventData.position);
        
        GameObject ob = Meet(eventData);

        
        var vec1 = CheckDirection(index);//�ν�����â���� �־��� ���Ͱ�

        //bool check1 = GetComponent<ForDirection>().CheckDirection(vec2, eventData);
        //bool check2 = GetComponent<ForDirection>().CheckDirection(vertical, eventData);

        if (dir.CheckDir(eventData.delta, CheckDirection(index)))
        {
            paintManager.PaintObject.ProcessInput = true;
            if (ob.gameObject.GetComponent<ICheck>() != null)
            {
                print("����");

                var vec2 = ob.GetComponent<ICheck>().CheckNext();
                print("vec2"+vec2);
                if (vec2.Item2 != Vector2.zero)
                {
                    if (CheckDirection(vec2.Item1) == vec2.Item2)
                    {
                        index = vec2.Item1;
                        ob.GetComponent<Image>().raycastTarget = false;
                    }
                }
                else
                {
                    print("�������׽�Ʈ");
                    puzzle.transform.GetChild(1).gameObject.SetActive(true);
                    StartCoroutine(Fill(0));
                    GetComponent<Image>().enabled = false;
                }
                
            }
            else
            {
                print("�浹���� ����");
                //paintManager.PaintObject.OnMouseUp(eventData.position);
                //paintManager.PaintObject.ProcessInput = false;
                //ResetTexture();
            }
        }
        else
        {
            print("x��Ʋ��");
            paintManager.PaintObject.OnMouseUp(eventData.position);
            paintManager.PaintObject.ProcessInput = false;
            ResetTexture();
        }
        
        //if (ob != null) //&& arrow2.raycastTarget == false
        //{
        //    if (missionCompleted == false)
        //    {
        //        paintManager.PaintObject.ProcessInput = true;
        //        if (check1)
        //        {
        //            if (ob.gameObject.GetComponent<ICheck>() != null)
        //            {
        //                //if (ob.gameObject.GetComponent<ICheck>() != null) print("�������̽��� ������ �ִ� ������ üũ");
        //                paintManager.PaintObject.ProcessInput = true;
        //                print("����");
        //                TurnCheck();
        //            }
        //        }
        //        else
        //        {
        //            print("x��Ʋ��");
        //            paintManager.PaintObject.OnMouseUp(eventData.position);
        //            paintManager.PaintObject.ProcessInput = false;
        //            ResetTexture();
        //            //transform.position = startPosition;
        //            missionCompleted = false;
        //        }
        //    }
        //    else if(missionCompleted)
        //    {
        //        paintManager.PaintObject.ProcessInput = true;
        //        if (check2)
        //        {
        //            //missionCompleted = false;
        //            paintManager.PaintObject.ProcessInput = true;
        //            print("y�����");
        //            //spotArrow[0].gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            print("y��Ʋ��");
        //            paintManager.PaintObject.OnMouseUp(eventData.position);
        //            paintManager.PaintObject.ProcessInput = false;
        //            ResetTexture();
        //            //transform.position = startPosition;
        //            missionCompleted = false;
        //        }
        //    }
        //}
        //else 
        //{
        //    paintManager.PaintObject.OnMouseUp(eventData.position);
        //    paintManager.PaintObject.ProcessInput = false;
        //    ResetTexture();
        //    //transform.position = startPosition;
        //    missionCompleted = false;
        //}
        
    }
    //�巡�� ������ ��
    public void OnEndDrag(PointerEventData eventData)
    {
        
        GameObject ob = Meet(eventData);
        GetComponent<Image>().raycastTarget = true;
        if (ob != null)
        {
            if (missionCompleted == true && ob.name == "fin")
            {
                puzzle.transform.GetChild(1).gameObject.SetActive(true);
                //transform.GetChild(1).gameObject.SetActive(true);
                StartCoroutine(Fill(0));
                //arrow2.enabled = false;
                GetComponent<Image>().enabled = false;
                //spotArrow[0].gameObject.SetActive(false);
               
                paintManager.PaintObject.OnMouseUp(eventData.position);
                paintManager.PaintObject.ProcessInput = false;
                ResetTexture();
            }
            else
            {
                //if (missionCompleted) GetComponent<Image>().sprite = arrowSprites[0];
                paintManager.PaintObject.OnMouseUp(eventData.position);
                paintManager.PaintObject.ProcessInput = false;
                ResetTexture();
                GetComponent<RectTransform>().transform.position = startPosition;
            }
        }
        else
        {
            //if (missionCompleted) GetComponent<Image>().sprite = arrowSprites[0];
            paintManager.PaintObject.OnMouseUp(eventData.position);
            paintManager.PaintObject.ProcessInput = false;
            ResetTexture();
            GetComponent<RectTransform>().transform.position = startPosition;
        }
    }
    //���� ���� ��ȯ  �ú��� �ε��� ����ڿ�.
    public Vector2 CheckDirection(int LineNumber)
    {
        Vector2 dirVec = new Vector2(0, 0);
        if ((directions[LineNumber] & Direction.Up) != 0) { dirVec += Vector2.up; }
        if ((directions[LineNumber] & Direction.Down) != 0) { dirVec += Vector2.down; }
        if ((directions[LineNumber] & Direction.Right) != 0) { dirVec += Vector2.right; }
        if ((directions[LineNumber] & Direction.Left) != 0) { dirVec += Vector2.left; }
        return dirVec;
    }
    //������ ȹ�� ���� ���̴� ������ �ɾ�� �浹ó�� �Լ� 
    public GameObject Meet(PointerEventData eventData)
    {
        GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();
        List<RaycastResult> result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);
        foreach (var item in result)
        {
            //item != this
            //if (item.gameObject.name != "Arrow2")  
                return item.gameObject;
        }
        return null;
    }
    //���� �ϼ��� ����ũ������� �̹����� ä������ ��� ���������ִ� �Լ�
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
    
    #region �Լ�
    //��ư ��� : �귯�� ũ�� ����
    public void ChangeSlider(UnityEngine.UI.Slider slider)
    {
        paintManager.Brush.Size = slider.value;
    }
    //�귯�� ���� �ٲٱ�
    public void ChangeColor(Color color)
    {
        paintManager.Brush.SetColor(color);
    }
    //����� : ���콺 ������ �α� �����
    public void Eraser()
    {
        paintManager.Brush.SetPaintTool(XDPaint.Core.PaintTool.Erase);
    }
    //ĥ�ϱ�
    public void Painting()
    {
        paintManager.Brush.SetPaintTool(XDPaint.Core.PaintTool.Brush);
    }
    //���� ���
    public void Undo()
    {
        if (paintManager.PaintObject.TextureKeeper.CanUndo())
        {
            paintManager.PaintObject.TextureKeeper.Undo();
        }
    }
    //�ǵ�����
    public void Redo()
    {
        if (paintManager.PaintObject.TextureKeeper.CanRedo())
        {
            paintManager.PaintObject.TextureKeeper.Redo();
        }
    }
    //��� ���� �׸� �͸�
    public RenderTexture PaintOnly()
    {
        return paintManager.GetResultRenderTexture();
    }
    //��� �ؽ��� ��������
    public Texture2D PaintResult()
    {
        return paintManager.GetResultTexture();
    }
    //�׸��� �� �����
    public void ResetTexture()
    {
        paintManager.PaintObject.TextureKeeper.Reset();
        paintManager.PaintObject.ClearTexture();
        paintManager.PaintObject.Render();
        paintManager.Render();
    }
    #endregion
    
}
