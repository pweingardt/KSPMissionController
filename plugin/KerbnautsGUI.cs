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

            GUILayout.Label("Kerbal Control Room", styleValueGreenBold);

            GUILayout.Label("Kerbonaut Hire cost: ", styleValueGreenBold);
            settings.HireKerbalNautCost = GUILayout.TextField(settings.HireKerbalNautCost);
            settings.HireKerbalNautCost = Regex.Replace(settings.HireKerbalNautCost, @"[^0-9]", "");
            GUILayout.Space(30);

            GUILayout.Label("Kerbonaut insurance cost: ", styleValueGreenBold);            
            settings.kerbonautCost = GUILayout.TextField(settings.kerbonautCost);
            settings.kerbonautCost = Regex.Replace(settings.kerbonautCost, @"[^0-9]", "");
            GUILayout.Label("Insurance Is Charged At Launch. Gets Returned When Recovered long as the Kerbals Lived through the mission", styleValueName);

            if (GUILayout.Button("Save Kerbal Cost", styleButton))
            {

                Difficulty.init(settings.difficulty);

                SettingsManager.Manager.saveSettings();
            }

            GUILayout.Space(30);

            GUILayout.Label("Use This To Simulate Hired Kerbals, At this Point it is not connected to the Actual Hire Button in Astronaut Complex. It is up to you if you want to use this option at this time When You Do Hire An Applicant you can use this button to charge your space Program. The price can be adjusted above", styleValueName);
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

                Difficulty.init(settings.difficulty);

                SettingsManager.Manager.saveSettings();

                kerbalNautsWindow(false);
            }


            
            
            GUILayout.EndVertical();
            GUI.DragWindow();
            





        }
    }
}
