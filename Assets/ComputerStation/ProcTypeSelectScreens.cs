using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class ProcTypeSelectScreens : UIScreenSpecialBehavior
    {
        [SerializeField] private ProcessSelected SelectedScreen;

        override internal void ParameterSelected(string parameter)
        {
            if (Enum.IsDefined(typeof(ProcessSelector.ProcessTypes), parameter))
            {
                SelectedScreen.m_ProcessType = (ProcessSelector.ProcessTypes) Enum.Parse(
                        typeof(ProcessSelector.ProcessTypes), parameter);
                SelectedScreen.ProcessTypeSet = true;
            }
                
        }
    }
}
