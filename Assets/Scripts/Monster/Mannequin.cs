using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
/// <summary>
/// 마네킹 몬스터 구현 2023.08.31 백인권
/// </summary>
/// 

public enum Mannequin_State { Dormant, Chase, Stop, Attack, Size }

public class Mannequin : BaseMonster
{
    private Mannequin_State curState;
    public Mannequin_State CurState { get { return curState; } }

    private MonsterStateBase<Mannequin>[] states;

    [SerializeField] Light horrorLight;
    [SerializeField] private GameObject[] mannequinParts;
    [SerializeField] private Quaternion[] mannequinPartsQuaternion;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float angle = 360f;
    [SerializeField] private float senseRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [SerializeField] LayerMask targetMask;

    public Light HorrorLight { get { return horrorLight; } }
    public GameObject[] MannequinParts { get { return  mannequinParts; } }
    public Quaternion[] MannequinPartsQuaternion { get { return mannequinPartsQuaternion; } }
    public float MoveSpeed { get { return moveSpeed;  } }
    public float Angle { get { return angle; } }
    public float SenseRange { get { return senseRange; } }
    public float ChaseRange { get { return chaseRange; } }
    public float AttackRange { get { return attackRange; } }
    public LayerMask TargetMask { get { return targetMask; } }

    public Coroutine mannequinMoveCoroutine;
    public Coroutine mannequinSoundPlayCoroutine;
    public Coroutine mannequinAnimationCoroutine;
    public Coroutine mannequinStopCoroutine;
    public Coroutine mannequinChaseBGMPlayCoroutine;


    public override void Awake()
    {
        base.Awake();

        StateInit();
        PartsRotationInit();
    }

    private void PartsRotationInit()
    {
        mannequinPartsQuaternion = new Quaternion[mannequinParts.Length];

        for (int i = 0; i < mannequinParts.Length; i++)
        { 
            mannequinPartsQuaternion[i] = mannequinParts[i].transform.rotation;
        }
    }

    private void StateInit()
    {
        states = new MonsterStateBase<Mannequin>[(int)Mannequin_State.Size];
        states[(int)Mannequin_State.Dormant] = new StateDorant(this);
        states[(int)Mannequin_State.Chase] = new StateChase(this);
        states[(int)Mannequin_State.Stop] = new StateStop(this);
        states[(int)Mannequin_State.Attack] = new StateAttack(this);
        curState = Mannequin_State.Dormant;
        //SetCurrentStateText();
    }

    private void Update()
    {
        // 테스트용 State Text
        //currentText.transform.parent.LookAt(playerPos);

        states[(int)curState].Update();
        states[(int)curState].Transition();
    }

    private void LateUpdate()
    {
        states[(int)curState].LateUpdate();
    }

    public void ChangeState(Mannequin_State state)
    {
        states[(int)curState].Exit();
        curState = state;
        //SetCurrentStateText();
        states[(int)curState].Setup();
        states[(int)curState].Enter();
    }

    public void SetCurrentStateText()
    {
        //currentText.text = curState.ToString();
    }

    public void MannequinBecameVisible()
    {
        if(curState != Mannequin_State.Attack)
            ChangeState(Mannequin_State.Stop);
    }

    public void MannequinBecameInvisible()
    {

        if (Vector3.Distance(playerPos.position, transform.position) < AttackRange)  // 플레이어가 공격범위에 들어오면 Attack으로 상태 변경
        {
            if(curState != Mannequin_State.Attack)
                ChangeState(Mannequin_State.Attack);
            return;
        }

        if (PlayerInColliderRange(ChaseRange).Length > 0)
        {
            // Chase , 안에 있음
            if (curState != Mannequin_State.Chase && curState != Mannequin_State.Dormant && curState != Mannequin_State.Attack)
                ChangeState(Mannequin_State.Chase);
            return;
        } else 
        {
            // Dormant
            ChangeState(Mannequin_State.Dormant);
            return;
        }
    }

    public void MonsterDestroyAndRespawn()
    {
        //리스폰 부분
        GameManager.Monster.SpawnMonster("Monster/Monster_Mannequin");
        //현재 몬스터는 삭제
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 아래 함수와 사용되는 각종 Sensitive 관련 변수들 따로 스크립트로 분리할 것
    /// </summary>
    public Collider[] PlayerInColliderRange(float range)
    {
        return Physics.OverlapSphere(transform.position, range, TargetMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SenseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SenseRange - 5);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, AttackRange + 3);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

}
