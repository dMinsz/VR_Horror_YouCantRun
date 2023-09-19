using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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

    bool canChange = true;
    bool isChange = false;

    public Transform EndPoint;

    public ElevatorController evCon;
    public Elevator ev;

    Collider coll;
    Coroutine mainRoutine;

    AudioSource sfx;

    [HideInInspector]public ControllersVibration vibe;
    private void Awake()
    {
        vibe = GetComponent<ControllersVibration>();
        sfx = GetComponent<AudioSource>();
        coll = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isChange && canChange)
        {
            if (other.gameObject.CompareTag("Player"))
            {

                if (nowFloor == floor.End)
                {
                    isChange = true;
                    mainRoutine = StartCoroutine(FinalSceneChange());
                }
                else 
                {
                    isChange = true;

                    Player player = other.gameObject.transform.root.GetComponent<Player>();

                    player.OffPhisics();

                    mainRoutine = StartCoroutine(SceneChange(other));
                }

            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isChange)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //other.gameObject.transform.root.GetComponent<Player>().nav.enabled = false;
                //other.gameObject.transform.root.GetComponent<NavMeshAgent>().enabled = false;
                mainRoutine = StartCoroutine(CloseDoor());

            }
        }
    }

    public void FadeIn(float fadeTime)
    {
        GameManager.UI.FadeIn(fadeTime);
    }

    public void FadeOut(float fadeTime)
    {
        GameManager.UI.FadeOut(fadeTime);
    }

    IEnumerator FinalSceneChange()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Scene.LoadScene("StartScene");

    }
    IEnumerator SceneChange(Collider other)
    {
        //Move to End Point
        var Player = other.gameObject.transform.root.GetComponent<Player>();


        while (true)
        {
            var distance = Vector3.Distance(Player.transform.position, EndPoint.position);

            if (distance > 0.5f)
            {
                Player.MoveTo(EndPoint.position);
            }

            if (distance <= 0.5f)
            {

                other.gameObject.transform.root.rotation = EndPoint.rotation;

                if (evCon.open == true)
                {
                    evCon.CloseDoor();
                }


                if (evCon.open == false)
                {

                    yield return new WaitForSeconds(1f); // EV Move Wait

                    switch (nowFloor)
                    {
                        case floor.Under:
                            ev.floorText.text = "1";
                            break;
                        case floor.First:
                            break;
                        case floor.Seconds:
                            ev.floorText.text = "1";
                            break;
                        case floor.Third:
                            {
                                ev.floorText.text = "2";
                                sfx.Play();
                                vibe.Vibe();
                                yield return new WaitForSeconds(2f);
                                GameManager.UI.FadeOut(1.6f);
                            }
                            break;
                    }

                    if (sfx != null && sfx.isPlaying == true)
                    {
                        yield return new WaitUntil(() => sfx.isPlaying == false);
                    }

                    yield return new WaitForSeconds(0.5f); // EV Change Floor Wait

                    //Scene change
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
                    break;
                }

                yield return new WaitForFixedUpdate();
            }
        

            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }


    public void OpenDoor()
    {
        canChange = false;
        //coll.enabled = false;
        evCon.ForceOpenDoor();
    }

    IEnumerator CloseDoor()
    {
        while (true) // close Door
        {
            if (evCon.open)
            {
                evCon.CloseDoor();
            }
            if (!evCon.open)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        canChange = true;

    }




}



