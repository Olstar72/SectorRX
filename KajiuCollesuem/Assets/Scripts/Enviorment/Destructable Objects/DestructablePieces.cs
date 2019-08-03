﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// OLIVER

/*
    Script for destroy prefab pieces.

    Can reset the pieces. Fades out and deactivates pieces after time.
*/

public class DestructablePieces : MonoBehaviour
{
    public float surviveTime = 5f;
    public float fadeTime = 1f;

    private Vector3 _Position;
    private Quaternion _Rotation;
    private Vector3 _Scale;
    private Color _Color;
    
    private Rigidbody _rb;
    private Renderer _renderer;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();

        _Position = transform.position;
        _Rotation = transform.rotation;
        _Scale = transform.localScale;
        _Color = _renderer.material.color;

        //Code to change material to render mode to Fade /////////////
        _renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _renderer.material.SetInt("_ZWrite", 0);
        _renderer.material.DisableKeyword("_ALPHATEST_ON");
        _renderer.material.EnableKeyword("_ALPHABLEND_ON");
        _renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _renderer.material.renderQueue = 3000;
        //////////////////////////////////////////////////////////////

        StartCoroutine(surviveRoutine());
    }

    private IEnumerator surviveRoutine()
    {
        //Time before starting fade out
        yield return new WaitForSeconds(surviveTime);
        StartCoroutine(fadeRoutine());
    }

    private IEnumerator fadeRoutine()
    {
        float curAlpha = 1f;

        //Loop to change Alpha
        while (_renderer.material.color.a > 0.01f)
        {
            curAlpha -= fadeTime * Time.deltaTime;
            _renderer.material.color = new Color(_Color.r, _Color.g, _Color.b, curAlpha);
            yield return null;
        }

        //Decative the object once invisible
        transform.parent.gameObject.SetActive(false);
    }

    public void ResetTransform()
    {
        //Reset myself
        transform.position = _Position;
        transform.rotation = _Rotation;
        transform.localScale = _Scale;
        _rb.velocity = Vector3.zero;
        _renderer.material.color = _Color;

        StopAllCoroutines();
        StartCoroutine(surviveRoutine());
    }
}
