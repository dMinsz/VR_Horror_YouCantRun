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

    [SerializeField] private float moveSpeed;
    [SerializeField] private float angle = 360f;
    [SerializeField] private float senseRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [SerializeField] LayerMask targetMask;

    public float MoveSpeed { get { return moveSpeed;  } }
    public float Angle { get { return angle; } }
    public float SenseRange { get { return senseRange; } }
    public float ChaseRange { get { return chaseRange; } }
    public float AttackRange { get { return attackRange; } }
    public LayerMask TargetMask { get { return targetMask; } }

    public override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;

        StateInit();
    }

    private void StateInit()
    {
        states = new MonsterStateBase<Mannequin>[(int)Mannequin_State.Size];
        states[(int)Mannequin_State.Dormant] = new StateDorant(this);
        states[(int)Mannequin_State.Chase] = new StateChase(this);
        states[(int)Mannequin_State.Stop] = new StateStop(this);
        states[(int)Mannequin_State.Attack] = new StateAttack(this);
        curState = Mannequin_State.Dormant;
        SetCurrentStateText();
    }
    private void Start()
    {
    }

    private void Update()
    {
        currentText.transform.parent.LookAt(playerPos);
        states[(int)curState].Update();   
    }

    private void LateUpdate()
    {
        states[(int)curState].LateUpdate();
    }

    public void ChangeState(Mannequin_State state)
    {
        states[(int)curState].Exit();
        curState = state;
        SetCurrentStateText();
        states[(int)curState].Setup();
        states[(int)curState].Enter();
    }

    public void SetCurrentStateText()
    {
        currentText.text = curState.ToString();
    }

    public void MannequinBecameVisible()
    {
        ChangeState(Mannequin_State.Stop);
    }

    public void MannequinBecameInvisible()
    {
        // Dormant or Chase
        if (PlayerInSensetiveRange().Length > 0)
        {
            // Chase
            ChangeState(Mannequin_State.Chase);

        } else
        {
            // Dormant
            ChangeState(Mannequin_State.Dormant);
        }

    }

    /// <summary>
    /// 아래 함수와 사용되는 각종 Sensitive 관련 변수들 따로 스크립트로 분리할 것
    /// </summary>
    /// 
    public Collider[] PlayerInSensetiveRange()
    {
        return Physics.OverlapSphere(transform.position, SenseRange, TargetMask);
    }

    public Collider[] PlayerInChaseSensetiveRange()
    {
        return Physics.OverlapSphere(transform.position, ChaseRange, TargetMask);
    }

    public Collider[] PlayerInAttackRange()
    {
        return Physics.OverlapSphere(transform.position, AttackRange, TargetMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SenseRange);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + Angle * 0.5f); // 대상이 바라보고 있는 각도 + 앵글의 1/2
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - Angle * 0.5f);  // 대상이 바라보고 있는 각도 - 앵글의 1/2
        Debug.DrawRay(transform.position, rightDir * SenseRange, Color.green);
        Debug.DrawRay(transform.position, leftDir * SenseRange, Color.green);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Vector3 rightDir2 = AngleToDir(transform.eulerAngles.y + Angle * 0.5f); // 대상이 바라보고 있는 각도 + 앵글의 1/2
        Vector3 leftDir2 = AngleToDir(transform.eulerAngles.y - Angle * 0.5f);  // 대상이 바라보고 있는 각도 - 앵글의 1/2
        Debug.DrawRay(transform.position, rightDir2 * SenseRange, Color.red);
        Debug.DrawRay(transform.position, leftDir2 * SenseRange, Color.red);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Vector3 rightDir3 = AngleToDir(transform.eulerAngles.y + Angle * 0.5f); // 대상이 바라보고 있는 각도 + 앵글의 1/2
        Vector3 leftDir3 = AngleToDir(transform.eulerAngles.y - Angle * 0.5f);  // 대상이 바라보고 있는 각도 - 앵글의 1/2
        Debug.DrawRay(transform.position, rightDir3 * SenseRange, Color.yellow);
        Debug.DrawRay(transform.position, leftDir3 * SenseRange, Color.yellow);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

}
