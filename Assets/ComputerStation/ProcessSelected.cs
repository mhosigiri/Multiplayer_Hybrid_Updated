using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    public class ProcessSelected : UIScreenSpecialBehavior
    {
        [SerializeField] private ProcessSelector m_ProcessSelector;
        [SerializeField] private TMPro.TextMeshProUGUI ProcessText;
        [SerializeField] private UIWindow GUI;

        internal ProcessSelector.ProcessTypes m_ProcessType;
        internal bool ProcessTypeSet = false;

        private void Awake()
        {
            if (ProcessTypeSet)
                m_ProcessSelector.ProcessTypeSelected(m_ProcessType);

            if (ProcessText)
                ProcessText.text = "You chose: " + m_ProcessType.ToString();

            if (GUI)
            {
                //GUI.HideWindow();
                GUI.gameObject.SetActive(false);
            }

            // Report selected process to Cognitive3D
#if C3D_DEFAULT
            new Cognitive3D.CustomEvent("Process Type Selected")
                .SetProperty("Process Type", m_ProcessType.ToString())
                .Send();
#endif
        }

        override internal void ParameterSelected(string parameter)
        {
            
        }
    }
}
