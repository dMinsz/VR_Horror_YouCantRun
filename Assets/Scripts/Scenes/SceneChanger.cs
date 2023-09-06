using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public enum floor
    {
        Under,
        First,
        Seconds,
        Third,
        None,
    }

    public floor nowFloor;


    bool isChange = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                switch (nowFloor) 
                {
                    case floor.Under:
                        GameManager.Scene.LoadScene("1F");
                        break;
                    case floor.First:
                        break;
                    case floor.Seconds:
                        GameManager.Scene.LoadScene("1F");
                        break;
                    case floor.Third:
                        GameManager.Scene.LoadScene("2F");
                        break;
                }
            }
         
            isChange = true;
        }
    }


}
