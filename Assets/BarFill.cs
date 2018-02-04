using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFill : MonoBehaviour {

    private Image image;
    private RectTransform rt;
    private float fullWidth;

    void Start ()
    {
        rt = GetComponent<RectTransform>();
        fullWidth = rt.rect.width;
        image = GetComponent<Image>();

    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            int percentage = 10;
            ReduceBarByPercentage(percentage);
        }

         if (Input.GetKeyDown(KeyCode.L))
        {
            FillBarToPercentage(100f);
        }
    }

    private void ReduceBarByPercentage(float percentage)
    {
        float newWidth = rt.rect.width - fullWidth * (percentage/100f);
        Mathf.Clamp(newWidth, 0, fullWidth);
        rt.sizeDelta = new Vector2 (newWidth, rt.rect.height);
    }

    public void FillBarToPercentage(float percentage)
    {
        float newWidth = (percentage/100f) * fullWidth;
        rt.sizeDelta = new Vector2 (newWidth, rt.rect.height);

    }
}
