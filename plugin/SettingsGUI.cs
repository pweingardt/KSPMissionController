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
        private String[] resetStrings = new String[] {"Reset MC To Start Values!", "Are you sure?"};
        
        private String[] difficulties = new String[] {"NormalMode","HardCoreMode"};

        //private String[] fueltech = new String[] { "Fuel Level 0", "Fuel Level 1" };

        //private String[] constructtech = new String[] { "Tech 0", "Tech 1", "Tech 2" };

        private void drawSettingsWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Plugin Disabled", GUILayout.Width(112), GUILayout.Height(40));
            if (settings.disablePlugin == true)
            {
                GUILayout.Box("TRUE", GUILayout.Width(112), GUILayout.Height(40));                
            }

            if (settings.disablePlugin == false)
            {
                GUILayout.Box("FALSE", GUILayout.Width(112), GUILayout.Height(40));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("GameMode: ", GUILayout.Width(112), GUILayout.Height(40));
            if (settings.gameMode == 0)
            {
                GUILayout.Box("Normal", GUILayout.Width(112), GUILayout.Height(40));
            }

            if (settings.gameMode == 1)
            {
                GUILayout.Box("HardCore", GUILayout.Width(112), GUILayout.Height(40));
            }
            GUILayout.EndHorizontal();


            //settings.disablePlugin = GUILayout.Toggle(settings.disablePlugin, "Disable Plugin");
            if (GUILayout.Button("Disable Plugin",styleButtonWordWrap))
            {
                settings.disablePlugin = !settings.disablePlugin;
            }
            //settings.gameMode = GUILayout.SelectionGrid(settings.gameMode, difficulties, 2);

            if (GUILayout.Button("Normal Mode", styleButtonWordWrap))
            {
                settings.gameMode = 0;
            }

            if (GUILayout.Button("HardCore Mode", styleButtonWordWrap))
            {
                settings.gameMode = 1;
            }

            //GUILayout.Space(10);
            //GUILayout.Box("Fuel Modes", GUILayout.Height(30));
            //settings.fuelmode = GUILayout.SelectionGrid(settings.fuelmode, fueltech, 2);

            //GUILayout.Space(10);
            //GUILayout.Box("Construction Modes", GUILayout.Height(30));
            //settings.constructmode = GUILayout.SelectionGrid(settings.constructmode, constructtech, 3);
           
            if (GUILayout.Button(resetStrings[resetCount],styleButtonWordWrap))
            {
                resetCount++;
                if (resetCount >= resetStrings.Length)
                {
                    resetCount = 0;
                    manager.resetSpaceProgram();
                }
            }

            if (GUILayout.Button("Save Settings", styleButtonWordWrap))
            {
                settingsWindow(false);
                //Difficulty.init(settings.difficulty);
                FuelMode.fuelinit(manager.GetFuels);
                ConstructionMode.constructinit(manager.GetConstruction);
                PayoutLeveles.payoutlevels(manager.GetCurrentPayoutLevel);
                SettingsManager.Manager.saveSettings();
            }
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
            
        }
    }
}