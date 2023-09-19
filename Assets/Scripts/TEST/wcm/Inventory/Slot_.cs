using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class Slot_ : XRSocketInteractor
{
    private Vector3 slotSize = new Vector3(0.2f, 0.1f, 0.2f);

    private Vector3 itemSize;
    private float ratio;

    public int slotNum;
    public string itemName;

    public UnityEvent<int, string> AddItemEvent;
    public UnityEvent<int> RemoveItemEvent;

    protected override void Start()
    {
        base.Start();
    }

    //[System.Obsolete]
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        GameObject obj = args.interactableObject.transform.gameObject;

        itemSize = obj.GetComponentInChildren<MeshRenderer>().bounds.size;

        if(args.interactableObject.transform.Find("SocketAttach"))
        {
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform = args.interactableObject.transform.Find("SocketAttach");
        }

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
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (args.interactableObject.transform.Find("Attach"))
        {
            args.interactableObject.transform.GetComponent<XRGrabInteractable>().attachTransform = args.interactableObject.transform.Find("Attach");
        }
        args.interactableObject.transform.localScale *= (1.0f / ratio);
    }

    public void socketCheck()
    {
        IXRSelectInteractable socketObj = this.GetOldestInteractableSelected();
        itemName = socketObj.transform.name;
        AddItemEvent?.Invoke(slotNum, itemName);
    }

    public void socketDrop()
    {
        IXRSelectInteractable obj = this.GetOldestInteractableSelected();

        itemName = "";
        RemoveItemEvent?.Invoke(slotNum);
    }
}