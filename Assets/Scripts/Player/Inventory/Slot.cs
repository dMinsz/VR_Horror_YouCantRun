using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class Slot : XRSocketInteractor
{
    //기존 slotSize
    private Vector3 slotSize = new Vector3(0.2f, 0.1f, 0.2f);

    //습득된 item의 Size / ratio
    private Vector3 itemSize;
    private float ratio;

    //슬롯이름과 들어가있는 아이템 이름
    public int slotNum;
    public string itemName;

    // 아이템 제거 / 습득 이벤트
    public UnityEvent<int, string> AddItemEvent;
    public UnityEvent<int> RemoveItemEvent;

    private bool canSocket = true;
    private GameObject[] highlightlist;

    //xrsocketinteraciton 상속
    protected override void Start()
    {
        base.Start();
        highlightlist = new GameObject[2];
        highlightlist[0] = transform.GetChild(1).gameObject;
        highlightlist[1] = transform.GetChild(2).gameObject;
    }

    private void HoverHighglightOn()
    {
        if (canSocket)
            highlightlist[0].SetActive(true);
        else
            highlightlist[1].SetActive(true);
    }
    private void HoverHighlightOff()
    {
        highlightlist[0].SetActive(false);
        highlightlist[1].SetActive(false);
    }

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);
        HoverHighglightOn();
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        HoverHighglightOn();
        base.OnHoverEntered(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        HoverHighlightOff();
    }
    //[System.Obsolete]
    //소켓에들어갈떄
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        //접촉된 물체 할당
        GameObject obj = args.interactableObject.transform.gameObject;

        //item크기
        itemSize = obj.GetComponentInChildren<MeshRenderer>().bounds.size;

        //socketAttach 포인트가 있을경우 ? grab interactable attachpoint 변경
        if(args.interactableObject.transform.Find("SocketAttach"))
        {
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform = args.interactableObject.transform.Find("SocketAttach");
        }

        //크기바꾸기
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

        canSocket = false;
        base.OnSelectEntering(args);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        canSocket = false;
        HoverHighlightOff();
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
    }

    // socket에서 빠진후
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (args.interactableObject.transform.Find("Attach"))
        {
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform = args.interactableObject.transform.Find("Attach");
        }
        args.interactableObject.transform.localScale *= (1.0f / ratio);
        canSocket = true;
    }

    // 소켓에 물체가 들어왔을 때
    public void socketCheck()
    {
        IXRSelectInteractable socketObj = this.GetOldestInteractableSelected();


        itemName = socketObj.transform.name;
        //socketObj.transform.gameObject.GetComponent<XRGrabInteractable>().attachTransform.position = socketObj.transform.position;

        AddItemEvent?.Invoke(slotNum, itemName);

        //Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

    //소켓에 물체가 떨어졌을때
    public void socketDrop()
    {
        IXRSelectInteractable obj = this.GetOldestInteractableSelected();

        itemName = "";
        RemoveItemEvent?.Invoke(slotNum);
    }
}