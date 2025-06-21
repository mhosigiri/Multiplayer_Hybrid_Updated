using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AverageUIBox : MonoBehaviour
{
    [SerializeField] private Text DropdownLabel;
    [SerializeField] private int[] StaticValues;

    private TMPro.TextMeshProUGUI TextBox;
    //private int[] vals;
    private int Sum;
    private int StaticSum = 0;
    private double Avg;

    // Start is called before the first frame update
    private void Start()
    {
        //vals = new int[Sources.Length];
        // Find static sum value
        foreach (int i in StaticValues)
        {
            StaticSum += i;
        }

        TextBox = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void CalculateAverage()
    {
        /*
        sum = 0;
        
        for (int i = 0; i < Sources.Length; i++)
        {
            vals[i] = int.Parse(Sources[i].text);
            sum += vals[i];
        }
        */
        Sum = StaticSum + int.Parse(DropdownLabel.text);

        Avg = Math.Round((float) Sum / (StaticValues.Length + 1), 2);

        if (TextBox)
            TextBox.text = Avg.ToString();
    }
}
