using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
public class StateAttack : MonsterStateBase<Mannequin>
{
    AudioClip audioClip;
    Transform originTransform;
    
    public StateAttack(Mannequin owner) : base(owner)
    {
    }

    public override void Enter()
    {
        originTransform = owner.transform;
        owner.HorrorLight.enabled = true;
        owner.Agent.speed = 0;
        GameManager.Sound.PlaySound(audioClip, Audio.UISFX, owner.transform.position, 0.6f);
        MannequinJumpScare();
        owner.StartCoroutine(RegenMonster());
    }
            
    public override void Exit()
    {
    }

    public void MannequinJumpScare()
    {
        owner.transform.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z) + new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * 1.5f;

        var look = Camera.main.transform.position;
        originTransform.LookAt(look);

        var origin = originTransform.rotation;
        origin.x = 0;
        origin.z = 0;

        owner.transform.rotation = origin;
    }

    public override void LateUpdate()
    {

    }

    public override void Setup()
    {
        audioClip = GameManager.Resource.Load<AudioClip>($"Sounds/JumpScare_{Random.Range(1,8)}");
    }

    public override void Transition()
    {
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
