using UnityEngine;

public class WheelMoveAssist : MonoBehaviour
{
    //[HideInInspector]public bool isAssist = false;

    [Tooltip("Wheel Move Assist Power")]
    public float power;

    Rigidbody m_Rigidbody;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            m_Rigidbody.AddTorque(m_Rigidbody.angularVelocity.normalized * power);
        }
    }


}
