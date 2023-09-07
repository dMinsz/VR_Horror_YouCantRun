using UnityEngine;


public class GoToUnderGround : MonoBehaviour
{
    public GameObject destroyObjects;
    public bool isChange = false;

   
    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                this.gameObject.SetActive(false);
                isChange = true;
            }
        }
    }
}
