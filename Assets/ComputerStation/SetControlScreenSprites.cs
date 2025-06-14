using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetControlScreenSprites : MonoBehaviour
{
    [SerializeField] private ScreenController ControlScreen;
    [SerializeField] private Sprite[] Screens;
    /*
    private void Awake()
    {
        if(isActiveAndEnabled)
        {
            if(ControlScreen && Screens.Length > 0)
            {
                ControlScreen.Screens = this.Screens;
            }
        }
    }
    */

    public void SetSprites()
    {
        if (ControlScreen && Screens.Length > 0)
        {
            ControlScreen.Screens = this.Screens;
        }
    }
}
