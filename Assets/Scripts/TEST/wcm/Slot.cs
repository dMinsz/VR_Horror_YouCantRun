using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/*//�Ķ���͸� ������ ���� �̺�Ʈ
[System.Serializable]
public class MyEvent : UnityEvent<MyParameters> { }

//�̺�Ʈ ������ ���� �Ķ����
[System.Serializable]
public class MyParameters
{
    public int parameter1;
    public string parameter2;
}*/

public class Slot : MonoBehaviour
{
    public int slotNum;
    public string itemName;
    XRSocketInteractor socket;

    public UnityEvent<int, string> AddItemEvent;
    public UnityEvent<int> RemoveItemEvent;

    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    //���� select������ Ȯ���� �߰�
    public void socketCheck()
    {
        //selecte�� object Ȯ��
        IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
        itemName = objName.transform.name;
        //������ �߰� �̺�Ʈ
        AddItemEvent?.Invoke(slotNum, itemName);



        //Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

    // ���� ���빰 ���
    public void socketDrop()
    {
        IXRSelectInteractable obj = socket.GetOldestInteractableSelected();

        
        itemName = "";
        //���� �̺�Ʈ�߻�
        RemoveItemEvent?.Invoke(slotNum);

    }
}