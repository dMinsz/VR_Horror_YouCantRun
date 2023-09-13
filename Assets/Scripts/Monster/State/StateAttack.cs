using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
public class StateAttack : MonsterStateBase<Mannequin>
{
    AudioClip audioClip;

    public StateAttack(Mannequin owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.HorrorLight.enabled = true;
        owner.Agent.speed = 0;
        GameManager.Sound.PlaySound(audioClip, Audio.UISFX, owner.transform.position, 0.6f);
        owner.StartCoroutine(RegenMonster());
    }
            
    public override void Exit()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Setup()
    {
        audioClip = GameManager.Resource.Load<AudioClip>("Sounds/JumpScare_1");
    }

    public override void Transition()
    {
        owner.transform.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z) + new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * 1.5f;
        owner.transform.LookAt(owner.playerPos);
    }

    public override void Update()
    {
    }

    IEnumerator RegenMonster()
    {
        yield return new WaitForSeconds(audioClip.length);
        owner.MonsterDestroyAndRespawn();
    }
}
