using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class ProblemStatement : UIScreenSpecialBehavior
    {
        [SerializeField] private GameObject RepairScreen;
        [SerializeField] private GameObject ReplaceScreen;
        [SerializeField] private UIScreen SelectedScreen;

        override internal void ParameterSelected(String parameter)
        {
            if (String.Equals(parameter, "REPAIR"))
            {
                GetComponent<UIScreen>().NextScreen = RepairScreen;
                SelectedScreen.PreviousScreen = RepairScreen;
            }
            else if (String.Equals(parameter, "REPLACE"))
            {
                GetComponent<UIScreen>().NextScreen = ReplaceScreen;
                SelectedScreen.PreviousScreen = ReplaceScreen;
            }
        }
    }
}
