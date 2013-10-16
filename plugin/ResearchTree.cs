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
