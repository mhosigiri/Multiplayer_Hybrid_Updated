using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class RecordSurveyResponse : MonoBehaviour
    {
        private Toggle[] Checkboxes;
        private Button Next;

        private void Start()
        {
            // Get answer checkboxes for this question
            Checkboxes = GetComponentsInChildren<Toggle>(true);

            if (Checkboxes == null)
            {
                Destroy(this);
            }

            // Get reference to navigation button to go to next screen
            foreach(Button b in GetComponentsInChildren<Button>(true))
            {
                if(b.gameObject.name == "Next")
                {
                    Next = b;
                    break;
                }
            }

            if (Next == null)
            {
                Destroy(this);
            }

            Next.onClick.AddListener(RecordResponse);
        }

        private void RecordResponse()
        {
            string answerText = string.Empty;
            string selectedAnswers = string.Empty;


            // Record each answer that is checked
            foreach (Toggle t in Checkboxes)
            {
                if(t.isOn)
                {
                    // Get answer text
                    answerText = string.Copy(t.transform.parent.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text);

                    // If first selected answer, just copy answer text
                    if (selectedAnswers.CompareTo(string.Empty) == 0)
                    {
                        selectedAnswers = string.Copy(answerText);
                    }

                    // If other answers found first, add comma and space before answer text
                    else
                    {
                        selectedAnswers = string.Concat(selectedAnswers, ", " + answerText);
                    }
                }
            }

            // If no answers selected, report that none selected
            if (selectedAnswers.CompareTo(string.Empty) == 0)
            {
                selectedAnswers = string.Copy("No Answers Selected");
            }

            // Report answers to Cognitive3D
#if C3D_DEFAULT
            try
            {
                new Cognitive3D.CustomEvent("Quiz Question Answered")
                    .SetProperty("Quiz Question", transform.Find("Question").GetComponent<TMPro.TextMeshProUGUI>().text)
                    .SetProperty("Answers Selected", selectedAnswers)
                    .Send();
            }
            catch (System.Exception)
            {
                Debug.Log("Error sending survey response data to Cognitive3D");
            }
#endif
        }
    }
}
