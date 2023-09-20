using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Slot : XRSocketInteractor
{
    //���� slotSize
    private Vector3 slotSize = new Vector3(0.2f, 0.1f, 0.2f);

    //����� item�� Size / ratio
    private Vector3 itemSize;
    private float ratio;

    //�����̸��� ���ִ� ������ �̸�
    public int slotNum;
    public string itemName;

    // ������ ���� / ���� �̺�Ʈ
    public UnityEvent<int, string> AddItemEvent;
    public UnityEvent<int> RemoveItemEvent;

    
    private GameObject[] highlightlist;

    //xrsocketinteraciton ���
    protected override void Start()
    {
        base.Start();
        highlightlist = new GameObject[2];
        highlightlist[0] = transform.GetChild(1).gameObject;
        highlightlist[1] = transform.GetChild(2).gameObject;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (isEntered == false)
        {
            if (hasSelection)
            {
                highlightlist[1].SetActive(true);
            }
            else
            {
                highlightlist[0].SetActive(true);
            }
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        highlightlist[0].SetActive(false);
        highlightlist[1].SetActive(false);

        //canSocket = true;
        //curState = State.None;
    }
    //���Ͽ�����
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        Debug.Log("selectEntering");


        //���˵� ��ü �Ҵ�
        GameObject obj = args.interactableObject.transform.gameObject;

        //itemũ��
        itemSize = obj.GetComponentInChildren<MeshRenderer>().bounds.size;

        //socketAttach ����Ʈ�� ������� ? grab interactable attachpoint ����
        if (args.interactableObject.transform.Find("SocketAttach"))
        {
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform = args.interactableObject.transform.Find("SocketAttach");
        }

        //ũ��ٲٱ�
        if (itemSize.x > 0.2f || itemSize.z > 0.2f)
        {
            if (itemSize.x >= itemSize.z)
                ratio = slotSize.x / itemSize.x;
            else
                ratio = (slotSize.z / itemSize.z);

            args.interactableObject.transform.localScale *= ratio;
        }
        else
            ratio = 1.0f;

        base.OnSelectEntering(args);
    }

    bool isEntered = false;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        highlightlist[0].SetActive(false);
        highlightlist[1].SetActive(false);

        StartCoroutine(changeValue());
    }

    IEnumerator changeValue()
    {
        isEntered = true;
        yield return new WaitForSeconds(0.5f);
        isEntered = false;
    }



    // socket���� ������
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (args.interactableObject.transform.Find("Attach"))
        {
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform = args.interactableObject.transform.Find("Attach");
        }
        args.interactableObject.transform.localScale *= (1.0f / ratio);
        //canSocket = true;
        //curState = State.None;
    }

    // ���Ͽ� ��ü�� ������ ��
    public void socketCheck()
    {
        IXRSelectInteractable socketObj = this.GetOldestInteractableSelected();


        itemName = socketObj.transform.name;
        //socketObj.transform.gameObject.GetComponent<XRGrabInteractable>().attachTransform.position = socketObj.transform.position;

        AddItemEvent?.Invoke(slotNum, itemName);

        //Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

    //���Ͽ� ��ü�� ����������
    public void socketDrop()
    {
        IXRSelectInteractable obj = this.GetOldestInteractableSelected();

        itemName = "";
        RemoveItemEvent?.Invoke(slotNum);
    }
}