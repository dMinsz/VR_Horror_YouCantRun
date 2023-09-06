using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEncounterSequence : MonoBehaviour
{
    [SerializeField] GhostEncounterBase owner;
    [SerializeField] Transform[] ghostSpawnZone;
    //[SerializeField] int ghostTotalShowCount;
    private Light[] lights;
    private bool isStarted;
    public bool IsStarted { get { return isStarted; } set { isStarted = value; } }

    private void Awake()
    {
        isStarted = false;
        owner = GetComponent<GhostEncounterBase>();
        LightArrayInit();
    }

    private void Update()
    {
        if (!isStarted)
            return;

        owner.EncounterTime -= Time.deltaTime;
        if (owner.EncounterTime <= 0)
            isStarted = false;
    }

    public void LightArrayInit()
    {
        lights = new Light[owner.BlinkingLights.Length];
        for (int i = 0;i < owner.BlinkingLights.Length;i++)
        {
            lights[i] = owner.BlinkingLights[i].GetComponentInChildren<Light>();
        }
    }

    public void GhostEncounterStart()
    {
        Debug.Log("유령출몰 시작함수 진입");
        if (isStarted)
            return;
        // 유령출몰 시퀀스 시작

        Debug.Log("유령출몰 시작함수 시작");
        isStarted = true;
        SetDoor(false);
        owner.EncounterCoroutine = StartCoroutine(EncounterCoroutine());
    }

    public void GhostEncounterEnds()
    {
        Debug.Log("유령출몰 끝");
        owner.EndSequence();
        SetDoor(true);
    }

    public void GhostEncounterJumpScare()
    {
        Debug.Log("점프스케어");
        SwitchLight(true);
    }

    public IEnumerator EncounterCoroutine()
    {
        while (isStarted)
        {
            SwitchGhost(false);
            SwitchLight(false);
            // 몬스터 안보이게
            yield return new WaitForSeconds(Random.Range(0.2f,0.6f));
            SwitchLight(true);
            // 몬스터 보이게
            if(Random.Range(0,101) > 10)    // 70%의 확률로 몬스터 등장
            {
                SwitchGhost(true);
                SpawnGhost();
            }
            // 깜빡이게
            for(int i = 0; i < Random.Range(1,6); i++)
            {
                //SwitchGhost(false);
                SwitchLight(false);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
                //SwitchGhost(true);
                SwitchLight(true);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }
            yield return new WaitForSeconds(Random.Range(0.6f,1.2f));
        }

        SwitchLight(false);
        GhostEncounterJumpScare();
        yield return new WaitForSeconds(2f);
        GhostEncounterEnds();
        yield return null;
    }  
    
    public void SwitchLight(bool state)
    {
        for (int i = 0; i < owner.BlinkingLights.Length; i++)
        {
            MeshRenderer meshRenderer = owner.BlinkingLights[i].gameObject.GetComponent<MeshRenderer>(); // Material 의 EMISSION  Enable/Disable을 위한 lightObject의 MeshRenderer 컴포넌트
            Material[] material = meshRenderer.materials;                                                // 위의 메시렌더러의 머테리얼
            lights[i].enabled = state;                                                                   // 조명 스위치
            if (state)                                                                                   // State에 따른 EMISSION Switch
            {
                material[1].EnableKeyword("_EMISSION");
            }
            else
            {
                material[1].DisableKeyword("_EMISSION");
            }
        }
    }

    public void SwitchGhost(bool state)
    {
        owner.GhostObject.gameObject.SetActive(state);
    }

    public void SpawnGhost()
    {
        owner.GhostObject.transform.position = ghostSpawnZone[Random.Range(0, ghostSpawnZone.Length)].position;
    }

    // true : 열기 , false : 닫기 
    public void SetDoor(bool state)
    {
        if (state)
        {
            // 문 열 수 있게
        }
        else
        {
            // 문 닫고 열 수 없게
            owner.Door.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
