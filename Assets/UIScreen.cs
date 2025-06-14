using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] internal GameObject NextScreen;
        [SerializeField] internal GameObject PreviousScreen;

        //private ControlScreen m_ControlScreen;
        private UIScreenSpecialBehavior m_SpecialBehavior;
        private Button[] m_NavButtons;
        private Toggle[] m_Toggles;

        private void Start()
        {
            m_NavButtons = GetComponentsInChildren<Button>(true);
            if (m_NavButtons == null)
            {
                Debug.Log("No Back or Next buttons found in the child Game Objects of "
                    + gameObject.name + ". Aborting UIScreen.");
                Destroy(this);
            }

            foreach (Button b in m_NavButtons)
            {
                // Convert to uppercase to avoid case mismatch errors
                string b_name = b.name.ToUpper();

                // if NEXT button, add next screen hook
                if (String.Equals(b_name, "NEXT"))
                {
                    b.onClick.AddListener(Next);
                }
                // if BACK button, add previous screen hook
                else if (String.Equals(b_name, "BACK"))
                {
                    b.onClick.AddListener(Previous);
                }
            }

            m_Toggles = GetComponentsInChildren<Toggle>(true);
            if (m_Toggles == null)
            {
                Debug.Log("No toggle buttons found in the child Game Objects of "
                    + gameObject.name + ". Aborting UIScreen.");
                Destroy(this);
            }

            foreach (Toggle t in m_Toggles)
            {
                // Convert to uppercase to avoid case mismatch errors
                string t_name = t.name.ToUpper();

                // Add a parameter set listener for when toggle set on
                t.onValueChanged.AddListener((on) => ParameterSelected(t, on, t_name));
            }
            /*
            m_ControlScreen = GetComponentInParent<ControlScreen>(true);
            if (m_ControlScreen == null)
            {
                Debug.Log("No Control Screen component found in parent Game Object to " +
                    gameObject.name + ". This may cause UIScreen to behave unexpectedly.");
            }
            */
            m_SpecialBehavior = GetComponent<UIScreenSpecialBehavior>();
        }

        private void ParameterSelected(Toggle t, bool on, String parameter)
        {
            if (on)
            {
                /*
                if (m_ControlScreen)
                    m_ControlScreen.ParameterSelected(parameter);
                */
                t.targetGraphic.color = t.colors.selectedColor;

                if (m_SpecialBehavior != null)
                    m_SpecialBehavior.ParameterSelected(parameter);
            }
            else
            {
                t.targetGraphic.color = t.colors.normalColor;
            }

            Debug.Log(parameter + ": " + on);
        }

        private void Next()
        {
            if (NextScreen)
            {
                NextScreen.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        private void Previous()
        {
            if (PreviousScreen)
            {
                PreviousScreen.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    public abstract class UIScreenSpecialBehavior : MonoBehaviour
    {
        internal abstract void ParameterSelected(String parameter);
    }
}
