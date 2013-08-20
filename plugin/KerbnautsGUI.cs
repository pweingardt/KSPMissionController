using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;


namespace MissionController
{
    /// <summary>
    /// This Draws the Window For KerbalNauts Recruitment Screen
    /// </summary>
    public partial class MissionController
    {
        VesselResources res = new VesselResources();
      
        private int kerbalCount = 0;
        private String[] HireKerbal = new String[] { "Hire Kerbals?", "Are you sure?"};

        private void drawKerbalnautWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.Label("Hire Kerbals And Adjust Hire Cost In This Window", styleValueGreenBold);
            GUILayout.Label("This Window Is Work In Progress", styleValueGreenBold);

            GUILayout.Label("Kerbonaut Hire cost: ", styleValueGreenBold);
            settings.HireKerbalNautCost = GUILayout.TextField(settings.HireKerbalNautCost);
            settings.HireKerbalNautCost = Regex.Replace(settings.HireKerbalNautCost, @"[^0-9]", "");

            if (GUILayout.Button("Save Kerbal Cost", styleButton))
            {

                Difficulty.init(settings.difficulty);

                SettingsManager.Manager.saveSettings();
            }

            GUILayout.Space(15);

            GUILayout.Label("Use This Button To Charge Your Space Program For New Hires You select in KerablNaut Complex", styleValueGreenBold);
            if (GUILayout.Button(HireKerbal[kerbalCount]))
            {
                kerbalCount++;
                if (kerbalCount >= HireKerbal.Length)
                {
                    kerbalCount = 0;
                    manager.costs(res.kerbal());
                }
            }
            GUILayout.Space(15);

            if (GUILayout.Button("Exit Window"))
            {
                kerbalNautsWindow(false);
            }


            
            
            GUILayout.EndVertical();
            GUI.DragWindow();
            





        }
    }
}
