using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class DayAndNightCycleBehaviour : MonoBehaviour
{
    [SerializeField] GameObject globalLight;
    [SerializeField] private Gradient lightColour;

    Light2D globalLightComponent;

    public int days;

    //public int Days => days;

    public float time = 0f;

    private bool canChangeToNextDay = true;
    private bool canChangeToSunrise = true;
    private bool canChangeToSunset = true;
    private bool canChangeToMoonrise = true;
    private bool canChangeToMoonSet = true;

    public event EventHandler OnChangeToSunRise;
    public event EventHandler OnChangeToSunSet;
    public event EventHandler OnChangeToMoonrise;
    public event EventHandler <ChangeToNextDayArgs> OnChangeToNextDay;


    public class ChangeToNextDayArgs : EventArgs
    {
        public int actualDay;
    }


    private void Start()
    {
        globalLightComponent = globalLight.GetComponent<Light2D>();
        time = 20f;
    }

    private void FixedUpdate()
    {
        PartsOfDayChanges();   

        time += Time.fixedDeltaTime * 1;
        //Debug.Log("Actual time: " + time);

        globalLightComponent.color = lightColour.Evaluate(time * 0.0041322314049587f);

        if (Mathf.Sin((time * 0.0041322314049587f * Mathf.PI)) <= 0.05f)
            globalLightComponent.intensity = 0.05f;
        else
            globalLightComponent.intensity = Mathf.Sin((time * 0.0041322314049587f * Mathf.PI));
    }

    private void PartsOfDayChanges()
    {
        if (time >= 241)
        {
            time = 0;
            canChangeToSunrise = true;
            canChangeToSunset = true;
            canChangeToMoonrise = true;
            canChangeToMoonSet = true;
            canChangeToNextDay = true;
            OnChangeToNextDay?.Invoke(this, new ChangeToNextDayArgs{ actualDay = days });
        }
        else if (canChangeToSunrise && time >= 20f)
        {
            canChangeToSunrise = false;
            OnChangeToSunRise?.Invoke(this, EventArgs.Empty);
            Debug.Log("Actual time: " + time);
        }
        else if (canChangeToSunset && time >= 90f)
        {
            OnChangeToSunSet?.Invoke(this, EventArgs.Empty);
            canChangeToSunset = false;
            Debug.Log("Actual time: " + time);
        }
        else if (canChangeToMoonrise && time >= 165f)
        {
            OnChangeToMoonrise?.Invoke(this, EventArgs.Empty);
            canChangeToMoonrise = false;
            Debug.Log("Actual time: " + time);
        }
        else if (canChangeToMoonSet && time >= 205f)
        {
            canChangeToMoonSet = false;
            Debug.Log("Actual time: " + time);
        }
        else if (canChangeToNextDay && time >= 240f)
        {
            canChangeToNextDay = false;
            days++;
            Debug.Log("Actual time: " + time);
        }
    }
}