using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/*//파라미터를 던지기 위한 이벤트
[System.Serializable]
public class MyEvent : UnityEvent<MyParameters> { }

//이벤트 던지기 위한 파라미터
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

    //소켓 select아이템 확인후 추가
    public void socketCheck()
    {
        //selecte된 object 확인
        IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
        itemName = objName.transform.name;
        //아이템 추가 이벤트
        AddItemEvent?.Invoke(slotNum, itemName);



        //Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

    // 소켓 내용물 드랍
    public void socketDrop()
    {
        IXRSelectInteractable obj = socket.GetOldestInteractableSelected();

        
        itemName = "";
        //삭제 이벤트발생
        RemoveItemEvent?.Invoke(slotNum);

    }
}