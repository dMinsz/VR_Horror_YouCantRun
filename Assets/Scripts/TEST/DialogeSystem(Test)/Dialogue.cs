using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * 이전에 사용하던 다이알로그 코드입니다
 * 사용하기전에 어느정도 다뜯어고쳐도되고 안써도됩니다
 * 
 * 연결된건 
 * Dialogues (ScriptableObject) , DialogueUI 코드입니다.
 * 
 * 
 */


//기본 다이알로그 데이터 셋업
[System.Serializable]
public class Dialog_Data
{
    public string name; // 다이알로그의 이름
    //public Sprite image; 만약이미지가들어가면 사용 (옆에 뜨는 사람그림이라던가)
    [TextArea(3, 5)]
    public string content; // 다이알로그 내용
    public bool checkRead; // 읽었는지 확인용

    public Dialog_Data Clone()
    {
        return new Dialog_Data();
    }
}

//실제 사용하는 다이알로그 데이터 셋업
//다이알로그 데이터 뭉치
[System.Serializable]
public class DialogDatas
{
    public string name;
    public List<Dialog_Data> Datas = new List<Dialog_Data>();
    public int index;
    public bool datasCheckRead;
}


public class Dialogue : MonoBehaviour
{
    [SerializeField]
    public List<DialogDatas> dialogs; //대화 지문 그룹
    public Queue<string> text_seq = new Queue<string>();                //대화 지문들의 내용을 큐로 저장
    public string name_;                                                //임시로 저장할 대화 지문의 이름
    public string text_;                                                //임시로 저장할 대화 지문의 내용

    public TMP_Text nameing;                                                //대화 오브젝트에 있는 것을 표시할 오브젝트
    public TMP_Text DialogT;                                                //대화 내용 오브젝트
    public Button NextButton;                                               //다음 버튼
    public DialogueUI dialog_obj;                                       //대화 지문 오브젝트

    IEnumerator seq_;
    IEnumerator skip_seq;

    public float delay;
    public bool running = false;

    //public DialogueSystem system; // 임시 저장용

    public void ResetDialogue()
    {
        running = false;
        //system = null;

        StopCoroutine(seq_);
        StopCoroutine(skip_seq);

        text_seq = new Queue<string>();
    }
    public void SetUp() 
    {
        //system = null;

        //if (dialogs == null)
        //{
        //    var datas = GameManager.Resource.Load<Dialogues>("Datas/DialogueDatas");

        //    dialogs = datas.Dialog;
        //}

        //if (dialog_obj == null)
        //{
        //    var robj = GameManager.Resource.Load<Dialogues>("UI/DialogueUI");
        //    var obj = GameManager.Pool.GetUI(robj, GameManager.UI.mainCanvas.transform);
        //    dialog_obj = obj;

        //    GameManager.Pool.ReleaseUI(obj);
        //}


    }

    private void OnDisable() //diable 될때 초기화
    {
        foreach (var item in dialogs)
        {
            item.datasCheckRead = false;
            foreach (var info in item.Datas)
            {
                info.checkRead = false;
            }
        }
    }

    public void ResetData()
    {
        foreach (var item in dialogs)
        {
            item.datasCheckRead = false;
            foreach (var info in item.Datas)
            {
                info.checkRead = false;
            }
        }
    }

    public bool IsVelvetRoom = false;
    public IEnumerator dialog_system_start(int index)//다이얼로그 출력 시작
    {
        nameing = dialog_obj.dname;   //다이얼로그 오브젝트에서 각 변수 받아오기
        DialogT = dialog_obj.Contents;
        NextButton = dialog_obj.NextButton;
        
        running = true;
        foreach (Dialog_Data dialog_temp in dialogs[index].Datas)  //대화 단위를 큐로 관리하기 위해 넣는다.
        {
            text_seq.Enqueue(dialog_temp.content);
        }

        dialog_obj.gameObject.SetActive(true);
        for (int i = 0; i < dialogs[index].Datas.Count; i++) //대화 단위를 순서대로 출력
        {
            if (!IsVelvetRoom)
            {
                dialog_obj.Igor.gameObject.SetActive(false);
                //dialog_obj.bustUp.gameObject.SetActive(true);
                //dialog_obj.bustUp.sprite = dialogs[index].Datas[i].BustUp; // 이미지 변환
            }
            else 
            {
                dialog_obj.Igor.gameObject.SetActive(true);
                //dialog_obj.bustUp.gameObject.SetActive(false);
            }

            nameing.text = dialogs[index].Datas[i].name;

            text_ = text_seq.Dequeue();                                  
            
            seq_ = seq_sentence(index, i);                               //대화 지문 출력 코루틴
            StartCoroutine(seq_);                                        //코루틴 실행


            yield return new WaitUntil(() =>
            {
                if (dialogs[index].Datas[i].checkRead)            //현재 대화를 읽었는지 아닌지
                {
                    return true;                                        //읽었다면 진행
                }
                else
                {
                    return false;                                       //읽지 않았다면 다시 검사
                }
            });
        }


                                  

        dialogs[index].datasCheckRead = true;                   //해당 대화 그룹 읽음
        running = false;
    }

    public void DisplayNext(int index, int number)                      //다음 지문으로 넘어가기
    {
        
        if (dialog_obj.selectUI.gameObject.activeSelf == true)        // 선택지가 나오면 무시
        {
            return;
        }


        NextButton.gameObject.SetActive(false);

        if (text_seq.Count == 0)                                        //다음 지문이 없다면
        {
            dialog_obj.gameObject.SetActive(false);                     //다이얼로그 끄기
        }
        StopCoroutine(seq_);                                            //실행중인 코루틴 종료

        dialogs[index].Datas[number].checkRead = true;            //현재 지문 읽음으로 표시
    }

    public IEnumerator seq_sentence(int index, int number)              //지문 텍스트 한글자식 연속 출력
    {
        skip_seq = touch_wait(seq_, index, number);                     //터치 스킵을 위한 터치 대기 코루틴 할당
        StartCoroutine(skip_seq);                                       //터치 대기 코루틴 시작
        DialogT.text = "";                                              //대화 지문 초기화
        foreach (char letter in text_.ToCharArray())                    //대화 지문 한글자씩 뽑아내기
        {
            DialogT.text += letter;                                     //한글자씩 출력
            yield return new WaitForSeconds(delay);                     //출력 딜레이
        }
        NextButton.gameObject.SetActive(true);
        //Next_T.text = "next";
        StopCoroutine(skip_seq);                                        //지문 출력이 끝나면 터치 대기 코루틴 해제
        IEnumerator next = next_touch(index, number);                   //버튼 이외에 부분을 터치해도 넘어가는 코루틴 시작
        StartCoroutine(next);
    }

    public IEnumerator touch_wait(IEnumerator seq, int index, int number)//터치 대기 코루틴
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        StopCoroutine(seq);                                              //대화 지문 코루틴 해제
        DialogT.text = text_;                                            //스킵시 모든 지문 한번에 출력
        NextButton.gameObject.SetActive(true);
        //Next_T.text = "next";
        IEnumerator next = next_touch(index, number);                    //대화 지문 코루틴 해제 됬기 때문에 다음 지문으로 가는 코루틴 시작
        StartCoroutine(next);                                                   
    }

    public IEnumerator next_touch(int index, int number)    //터치로 다음 지문 넘어가는 코루틴
    {
        StopCoroutine(seq_);
        StopCoroutine(skip_seq);
        yield return new WaitForSeconds(0.3f);

        yield return new WaitUntil(() => Input.GetMouseButton(0));
        //!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0)
        DisplayNext(index, number);
    }

    public bool dialog_read(int check_index)          //index의 부분을 읽었는지 확인하는 함수
    {
        if (!dialogs[check_index].datasCheckRead)
        {
            return true;
        }
        
        return false;
    }
}
