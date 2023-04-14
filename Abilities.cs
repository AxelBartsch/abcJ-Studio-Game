using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public float BoostSpeed = 5f;
    public float BoostDuration = 5f;
    public bool BoostOnCD = false;
    
    public float SlowAmount = 0.5f;
    public float SlowDuration = 5f;
    public float SlowSpeedBoost = 5f;
    
    PlayerBehavior player;

    void Awake()
    {
        this.player = GetComponent<PlayerBehavior>();
    }

    public void StartSpeedBoost()
    {
        StartCoroutine(SpeedBoost(this.BoostSpeed, this.BoostDuration, true));
    }
    
    private IEnumerator SpeedBoost(float SpeedChange, float SpeedDuration, bool setFlag)
    {
        Debug.Log("SpeedBoost!");
        player.MoveSpeed += SpeedChange;
        float EndTime = Time.time + SpeedDuration;
        if(setFlag)
        {
            this.BoostOnCD = true;
        }
        
        while(Time.time <= EndTime)
        {
            yield return null;
        }
        player.MoveSpeed -= SpeedChange;
        if(setFlag)
        {
            this.BoostOnCD = false; 
        }
       Debug.Log("End SpeedBoost");
    }

    public void StartSlowTime()
    {
        StartCoroutine(SpeedBoost(this.SlowSpeedBoost, this.SlowDuration * this.SlowAmount, false));
        StartCoroutine(SlowTime(this.SlowAmount, this.SlowDuration * this.SlowAmount));
    }

    private IEnumerator SlowTime(float Amount, float Duration)
    {
        Debug.Log("Slow Time!");
        float EndTime = Time.time + Duration;
        float fixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = Amount;
        
        while(Time.time <= EndTime)
        {
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
            yield return null;
        }
        Time.timeScale = 1f;
        Debug.Log("End time slow");
    }
}
