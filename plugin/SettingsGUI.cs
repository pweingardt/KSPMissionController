using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

namespace MissionController
{
    /// <summary>
    /// Responsible for the settings window
    /// </summary>
    public partial class MissionController
    {
        private int resetCount = 0;
        private String[] resetStrings = new String[] {"Reset the space program!", "Are you sure?"};

        private int rewindCount = 0;
        private String[] rewindStrings = new String[] {"Rewind", "Are you sure?"};
        
        private String[] difficulties = new String[] { "Flight Testing", "Flight Mode","HardCoreMode"};

        private void drawSettingsWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            settings.disablePlugin = GUILayout.Toggle(settings.disablePlugin, "Disable Plugin");

            GUILayout.Space(20);
            GUILayout.Label("Chose Your Game Modes", styleValueGreenBold);
            settings.difficulty = GUILayout.SelectionGrid(settings.difficulty, difficulties, 3);            

            GUILayout.Space(20);
            GUILayout.Label("Revert Your Missions", styleValueGreenBold);
            if (GUILayout.Button(rewindStrings[rewindCount], styleButtonWordWrap))
            {
                rewindCount++;
                if (rewindCount >= rewindStrings.Length)
                {
                    rewindCount = 0;
                    manager.rewind();
                }
            }

            if (GUILayout.Button(resetStrings[resetCount], styleButtonWordWrap))
            {
                resetCount++;
                if (resetCount >= resetStrings.Length)
                {
                    resetCount = 0;
                    manager.resetSpaceProgram();
                }
            }
            GUILayout.Space(20);
            GUILayout.Label("Save Your Settings", styleValueGreenBold);
            if (GUILayout.Button("Save and Close Settings", styleButton))
            {
                settingsWindow(false);
                Difficulty.init(settings.difficulty);
                SettingsManager.Manager.saveSettings();
                GUISave();
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}