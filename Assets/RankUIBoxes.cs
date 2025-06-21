using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RankUIBoxes : MonoBehaviour
{
    
    [SerializeField]
    [Tooltip("Set to true if a higher number corresponds to a better rank. " +
             "Keep false if lower numbers have a lower rank.")]
    private bool RankDescending = false;
    [SerializeField] private TMPro.TextMeshProUGUI[] AverageTextBoxes;
    
    private TMPro.TextMeshProUGUI[] RankTextBoxes;

    private Dictionary<float, TMPro.TextMeshProUGUI> TextBoxValPairs;
    private SortedDictionary<float, TMPro.TextMeshProUGUI> SortedPairs;

    // Start is called before the first frame update
    void Start()
    {
        TextBoxValPairs = new Dictionary<float, TMPro.TextMeshProUGUI>();

        // Return child rank text boxes in Row 1 to Row 4 order
        RankTextBoxes = GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
    }

    public void CheckRank()
    {
        TextBoxValPairs.Clear();
        float key = 0;

        // Check that each text box has a value
        for (int i = 0; i < AverageTextBoxes.Length; i++)
        {
            // If this check box doesn't have a value, return
            if (AverageTextBoxes[i].text == "")
                return;

            // Add this value to the dictionary
            
            // If there is already another row with this rank, increase the key value 
            // Until it no longer matches
            key = float.Parse(AverageTextBoxes[i].text);
            while (TextBoxValPairs.ContainsKey(key))
                key += 0.01f;

            // Connect float average value in the row to its corresponding rank row
            TextBoxValPairs.Add(key, RankTextBoxes[i]);
        }

        // Sort average values
        SortedPairs = new SortedDictionary<float, TMPro.TextMeshProUGUI>(TextBoxValPairs);

        // Calculate rank for each row from average values
        int rank;

        // If rank ascending, start rank 1 at lowest values
        if (RankDescending == false)
        {
            rank = 1;
            foreach (KeyValuePair<float, TMPro.TextMeshProUGUI> kvp in SortedPairs)
            {
                kvp.Value.text = rank.ToString();
                rank++;
            }
        }
        // If rank descending, assign value at start of dictionary worst rank
        else
        {
            rank = SortedPairs.Count;
            foreach (KeyValuePair<float, TMPro.TextMeshProUGUI> kvp in SortedPairs)
            {
                kvp.Value.text = rank.ToString();
                rank--;
            }
        }
    }
}
