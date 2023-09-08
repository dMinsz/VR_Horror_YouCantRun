using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public enum floor
    {
        Under,
        First,
        Seconds,
        Third,
        End,
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
                        GameManager.Gimmick.UnderTo1F = true;
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
                    case floor.End:
                        GameManager.Scene.LoadScene("StartScene");
                        break;
                }
            }
         
            isChange = true;
        }
    }


}
