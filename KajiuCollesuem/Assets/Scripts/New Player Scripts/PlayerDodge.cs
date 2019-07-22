﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public bool doneDodge = false;

    public float dashCooldown = 1.0f;
    private float _dashDelay = 0.0f;

    [SerializeField] private float longDodgeDuration = 2f;
    [SerializeField] private float shortDodgeDuration = 1f;

    [SerializeField] private float longDodgeDistance = 5.0f;
    [SerializeField] private float shortDodgeDistance = 3.5f;

    public void Dodge(bool pShortDodge)
    {
        //Cooldown
        if (Time.time > _dashDelay)
        {
            if (pShortDodge)
            {
                StartCoroutine(DodgeRoutine(shortDodgeDistance, shortDodgeDuration));
            }
            else
            {
                StartCoroutine(DodgeRoutine(longDodgeDistance, longDodgeDuration));
            }
        }
    }

    IEnumerator DodgeRoutine(float pDistance, float pDuration)
    {
        //Setting Delay
        _dashDelay = Time.time + dashCooldown + pDuration;
        
        //Running Dodge
        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * (pDistance/ pDuration), ForceMode.VelocityChange);
        yield return new WaitForSeconds(pDuration);
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        doneDodge = true;
    }
}
