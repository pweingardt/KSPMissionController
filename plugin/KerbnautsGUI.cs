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
        private String[] kerbalHire = new String[] { "Charge For hire Kerbals?", "Are you sure Charge is 5000?" };

        private void drawKerbalnautWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            if (GUILayout.Button(kerbalHire[kerbalCount]))
            {
                kerbalCount++;
                if (kerbalCount >= kerbalHire.Length)
                {
                    kerbalCount = 0;
                    manager.costs(res.kerbal());
                }
            }

            GUILayout.Label("Kerbonaut Hire cost: ", styleValueGreenBold);
            settings.HireKerbalNautCost = GUILayout.TextField(settings.HireKerbalNautCost);
            settings.HireKerbalNautCost = Regex.Replace(settings.HireKerbalNautCost, @"[^0-9]", "");


            if (GUILayout.Button("Save and Close KerbalNaut Window", styleButton))
            {
                kerbalNautsWindow(false);

                Difficulty.init(settings.difficulty);

                SettingsManager.Manager.saveSettings();
            }
            
            GUILayout.EndVertical();
            GUI.DragWindow();
            





        }
    }
}
