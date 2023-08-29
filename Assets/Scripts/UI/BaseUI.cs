using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    protected Dictionary<string, RectTransform> transforms; //UI의 상위 부모 역할

    protected Dictionary<string, Button> buttons;
    public Dictionary<string, TMP_Text> texts;
    public Dictionary<string, Slider> sliders;
    protected virtual void Awake()
    {
        BindChildren();
    }

    protected virtual void BindChildren()
    {
        transforms = new Dictionary<string, RectTransform>();
        buttons = new Dictionary<string, Button>();
        texts = new Dictionary<string, TMP_Text>();
        sliders = new Dictionary<string, Slider>();
        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        // GetComponentsInChildren => baseUI 를 기준으로 하위 모든 자식들을 가져온다
        // RectTransform은 UI에 모두 있기때문에 모든 하위 UI 자식들을 childeren 에 넣어주는 게 된다.
        foreach (RectTransform child in children)
        {
            string key = child.gameObject.name; // 오브젝트의 이름을 키값으로 사용할 것이다.

            if (transforms.ContainsKey(key))
                continue;

            transforms.Add(key, child);

            Button button = child.GetComponent<Button>();
            if (button != null)
                buttons.Add(key, button);

            TMP_Text text = child.GetComponent<TMP_Text>();
            if (text != null)
                texts.Add(key, text);

            Slider slider = child.GetComponent<Slider>();
            if (slider != null)
                sliders.Add(key, slider);
        }
    }

    public virtual void CloseUI()
    {

    }
}
