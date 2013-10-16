using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;


namespace MissionController
{
    /// <summary>
    /// This Draws the Window For Research Window
    /// </summary>
    public partial class MissionController
    {
               
        private void drawResearchTree(int id)
        {                       

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Current Research Level", GUILayout.Width(250), GUILayout.Height(40));
            GUILayout.Box("Level 7", GUILayout.Width(250), GUILayout.Height(40));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Vessel Recycling", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box(" Needed Level 7", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box("RESEARCHED", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Efficient Fuels", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box(" Needed Level 9", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box("NOT AVAILABLE", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Construction 1", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box(" Needed Level 4", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box("RESEARCHED", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Construction 2", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box(" Needed Level 10", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box("NOT AVAILABLE", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Repair Missions", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box(" Needed Level 6", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box("RESEARCHED", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            if (GUILayout.Button("Exit Window"))
            {

                Difficulty.init(settings.difficulty);                

                SettingsManager.Manager.saveSettings();

                ResearchTreeWindow(false);
            }

            GUILayout.EndVertical();
            GUI.DragWindow();

        }
    }
}
