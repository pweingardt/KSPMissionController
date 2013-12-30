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
        private String[] resetStrings = new String[] {"Reset To Start Values!", "Are you sure?"};

        private int rewindCount = 0;
        private String[] rewindStrings = new String[] {"REVERT Space Program", "Revert To Before Last SC Visit?"};
        
        private String[] difficulties = new String[] {"NormalMode","HardCoreMode"};

        //private String[] fueltech = new String[] { "Fuel Level 0", "Fuel Level 1" };

        //private String[] constructtech = new String[] { "Tech 0", "Tech 1", "Tech 2" };

        private void drawSettingsWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            settings.disablePlugin = GUILayout.Toggle(settings.disablePlugin, "Disable Plugin");

            GUILayout.Space(10);
            GUILayout.Box("Chose Your Game Modes",GUILayout.Height(30));
            settings.gameMode = GUILayout.SelectionGrid(settings.gameMode, difficulties, 2);

            //GUILayout.Space(10);
            //GUILayout.Box("Fuel Modes", GUILayout.Height(30));
            //settings.fuelmode = GUILayout.SelectionGrid(settings.fuelmode, fueltech, 2);

            //GUILayout.Space(10);
            //GUILayout.Box("Construction Modes", GUILayout.Height(30));
            //settings.constructmode = GUILayout.SelectionGrid(settings.constructmode, constructtech, 3);
           
            GUILayout.Space(10);

            if (FlightDriver.CanRevertToPrelaunch && HighLogic.LoadedSceneIsFlight)
            {
                GUILayout.Box("Revert Your Missions", GUILayout.Height(30));
                if (GUILayout.Button(rewindStrings[rewindCount], styleGreenButton))
                {
                    rewindCount++;
                    if (rewindCount >= rewindStrings.Length)
                    {
                        rewindCount = 0;
                        manager.loadProgramBackup(HighLogic.CurrentGame.Title);
                        FlightDriver.RevertToPrelaunch(GameScenes.EDITOR);
                    }
                }
            }

            if (GUILayout.Button(resetStrings[resetCount],styleGreenButton))
            {
                resetCount++;
                if (resetCount >= resetStrings.Length)
                {
                    resetCount = 0;
                    manager.resetSpaceProgram();
                }
            }
            GUILayout.Space(10);
            GUILayout.Box("Save Your Settings", GUILayout.Height(30));
            if (GUILayout.Button("Save and Close Settings", styleGreenButton))
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