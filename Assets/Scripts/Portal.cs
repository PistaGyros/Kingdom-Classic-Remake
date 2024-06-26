using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject globalLight;
    [SerializeField] GameObject troll;

    private float numberOfDays;
    private float amountOfWave;
    


    private void Start()
    {
        DayAndNightCycleBehaviour changeToSunriseEvent = globalLight.GetComponent<DayAndNightCycleBehaviour>();
        changeToSunriseEvent.OnChangeToMoonrise += ChangeToMoonriseEvent_OnChangeToMoonrise;
    }


    private void ChangeToMoonriseEvent_OnChangeToMoonrise(object sender, System.EventArgs e)
    {
        numberOfDays++;
        amountOfWave = Mathf.Round(Mathf.Log(numberOfDays, 1.5f));
        SpawnWave();
    }


    private void SpawnWave()
    {
        InvokeRepeating("SpawnTroll", 0.1f, 0.5f);
    }


    private void SpawnTroll()
    {
        if (amountOfWave > 0)
        {
            amountOfWave--;
            Instantiate(troll, new Vector2(transform.position.x, 0.55f), Quaternion.identity);
            Debug.Log("Troll has spawned");
        }
        else
            CancelInvoke("SpawnTroll");
    }
}