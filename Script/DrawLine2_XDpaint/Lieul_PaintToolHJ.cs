using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//ĥ�ϱ� ��� ���� : Canvas�� ������ Overlay������ �۵�
public class Lieul_PaintToolHJ : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
                        print("����");
                        transform.rotation = Quaternion.Euler(0, 0, -90);
                        arrow3.SetActive(true);
                    }
                }
                else if (check1 ==false)
                {
                    print("x��Ʋ��");
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
                    print("y�����");
                    arrow3.SetActive(true);
                }
                else
                {
                    print("y��Ʋ��");
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
    //�巡�� ������ ��
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
    //������ ȹ�� ���� ���̴� ������ �ɾ�� �浹ó�� �Լ� 
    public GameObject Meet(PointerEventData eventData)
    {
        GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();
        List<RaycastResult> result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);
        foreach (var item in result)
        {
            //print("�浹üȮ�� : "+ item);
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
    /*
    public void CheckDirectionVector(int state, PointerEventData eventData)
    {
        switch (state)
        {
            case 0://���� �׸���
                print("����!");
                if (eventData.delta.y > 0) { paintManager.PaintObject.ProcessInput = true; } //true�϶��� ���� //����Ʈ ���� ���� �Ǵ��ϴ� �Ķ����
                    break;
            case 1://�Ʒ��� �׸���
                print("�Ʒ���!");
                if (eventData.delta.y < 0) { paintManager.PaintObject.ProcessInput = true; } 
                break;
            case 2://�·� �׸���
                print("�·�!");
                if (eventData.delta.x < 0) { paintManager.PaintObject.ProcessInput = true; } 
                break;
            case 3://��� �׸���
                print("���!");
                if (eventData.delta.x > 0) { paintManager.PaintObject.ProcessInput = true; } 
                break;
                
        }
    }
    */
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
