using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScarePosterLight : MonoBehaviour
{

    [SerializeField] Light[] posterLights;
    private EventStartZone eventZone;

    private void Awake()
    {
        eventZone = GetComponentInChildren<EventStartZone>();
    }
    
    public void TurnOnTheLights()
    {
        Vector3 soundPosition = Camera.main.transform.position;
        soundPosition.x = soundPosition.x - 1.5f;
        soundPosition.y = soundPosition.y - 0f;
        soundPosition.z = soundPosition.z + 8f;
        eventZone.IsRunning = false;

        GameManager.Sound.PlaySound("JumpScare_4", Audio.UISFX, new Vector3(), 0.3f);

        for (int i = 0; i < posterLights.Length; i++)
        {
            posterLights[i].enabled = true;
        }

        GameManager.Sound.PlaySound("PosterGhost",Audio.SFX, soundPosition, 0.5f,0.8f);
    }
}
