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

    private int days;

    public int Days => days;

    private float time = 0f;

    private bool canChangeToNextDay = true;
    private bool canChangeToSunrise = true;
    private bool canChangeToSunset = true;
    private bool canChangeToMoonrise = true;
    private bool canChangeToMoonset = true;

    public event EventHandler OnChangeToSunRise;
    public event EventHandler OnChangeToSunSet;
    public event EventHandler OnChangeToMoonrise;
    public event EventHandler OnChangeToNextDay; 




    private void Start()
    {
        globalLightComponent = globalLight.GetComponent<Light2D>();
        time = 20f;
    }

    private void FixedUpdate()
    {
        PartsOfDayChanges();   

        time += Time.fixedDeltaTime;

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
            canChangeToMoonset = true;
            canChangeToNextDay = true;
            OnChangeToNextDay?.Invoke(this, EventArgs.Empty);
        }

        else if (canChangeToSunrise && time >= 20f)
        {
            canChangeToSunrise = false;
        }

        else if (canChangeToSunset && time >= 90f)
        {
            OnChangeToSunSet?.Invoke(this, EventArgs.Empty);
            canChangeToSunset = false;
        }

        else if (canChangeToMoonrise && time >= 165f)
        {
            OnChangeToMoonrise?.Invoke(this, EventArgs.Empty);
            canChangeToMoonrise = false;
        }

        else if (canChangeToMoonset && time >= 205f)
        {
            canChangeToMoonset = false;
        }

        else if (canChangeToNextDay && time >= 240f)
        {
            canChangeToNextDay = false;
            days++;
        }
    }
}