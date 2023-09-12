using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostEncounterSequence : MonoBehaviour
{
    [SerializeField] GhostEncounterAction owner;
    [SerializeField] Transform[] ghostSpawnZone;
    //[SerializeField] int ghostTotalShowCount;
    private Light[] lights;
    private bool isStarted;
    public bool IsStarted { get { return isStarted; } set { isStarted = value; } }

    private void Awake()
    {
        isStarted = false;
        owner = GetComponent<GhostEncounterAction>();
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
        LightArrayInit();

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

    public void GhostSetJumpScarePose()
    {
        owner.Ghost.JumpScarePose();
    }

    public IEnumerator EncounterCoroutine()
    {
        while (isStarted)
        {
            PlayLampSwitchingSound();
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
                PlayLampSwitchingSound();
                SwitchLight(false);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
                //SwitchGhost(true);
                SwitchLight(true);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
            }
            yield return new WaitForSeconds(Random.Range(0.6f,1.2f));
        }

        owner.JumpScareCoroutine = StartCoroutine(JumpScareCoroutine());
        yield return null;
    }  

    public IEnumerator JumpScareCoroutine()
    {
        PlayLampSwitchingSound();
        SwitchLight(false);
        SwitchGhost(false);
        yield return new WaitForSeconds(1f);
        //SwitchLight(true);
        SwitchGhost(true);
        SpawnGhost(true);
        GhostSetJumpScarePose();
        yield return new WaitForSeconds(2f);
        SwitchLight(false);
        SwitchGhost(false);
        GhostEncounterEnds();
        yield return new WaitForSeconds(0.1f);
        PlayLampSwitchingSound();
        SwitchLight(true);
        yield return null;
    }

    public void SwitchLight(bool state)
    {
        for (int i = 0; i < owner.BlinkingLights.Length; i++)
        {
            if (owner.BlinkingLights[i].gameObject.GetComponent<MeshRenderer>())
            {
                MeshRenderer meshRenderer = owner.BlinkingLights[i].gameObject.GetComponent<MeshRenderer>(); // Material 의 EMISSION  Enable/Disable을 위한 lightObject의 MeshRenderer 컴포넌트
                Material[] material = meshRenderer.materials;                                                // 위의 메시렌더러의 머테리얼
                if (state)                                                                                   // State에 따른 EMISSION Switch
                {
                    material[1].EnableKeyword("_EMISSION");
                }
                else
                {
                    material[1].DisableKeyword("_EMISSION");
                }
            }
            lights[i].enabled = state;                                                                   // 조명 스위치
        }
    }

        public void SwitchGhost(bool state)
    {
        owner.GhostObject.gameObject.SetActive(state);
    }

    public void SpawnGhost(bool isJumpScare = false)
    {
        Vector3 spawnTransform = new Vector3();
        if (isJumpScare)
        {
            spawnTransform = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z) + new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * 1.7f;
        } else
        {
            spawnTransform = ghostSpawnZone[Random.Range(0, ghostSpawnZone.Length)].position;
        }

        owner.GhostObject.transform.position = spawnTransform;


    }

    public void PlayLampSwitchingSound()
    {
        GameManager.Sound.PlaySound($"Lamp_{Random.Range(1,8)}",Audio.SFX,transform.position);
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
            // 문 닫고
            //owner.Door.transform.localRotation = Quaternion.Euler(0, 0, 0);
            // 문 열수 없게



        }
    }
}
