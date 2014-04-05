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
        private int resetMissionCount = 0;
        private int resetGoalsCount = 0;
        private String[] resetStrings = new String[] {"Reset Your MCE SaveGame", "This Will Reset Your Save!"};
        private String[] resetMissions = new String[] {"Clear All Completed Missions", "Are You Sure?","THIS WILL DELETE All MISSIONS!"};
        private String[] resetGoals = new String[] {"Clear Any Completed Goals", "Are You Sure?","THIS WILL DELETE All GOALS!"};
        
        private void drawSettingsWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.Box(mainWindowTitle, GUILayout.Width(225), GUILayout.Height(45));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("MCE Developer Malkuth74",StyleBoxWhite, GUILayout.Width(225), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Co Developer NathanKell", StyleBoxWhite, GUILayout.Width(225), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Original MC Developer NoBody44", StyleBoxWhite, GUILayout.Width(225), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("ToolBar Mod By Blizzy78", StyleBoxWhite, GUILayout.Width(225), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.Space(25);

            ConstructionMode CM = new ConstructionMode();         
            
            GUILayout.BeginHorizontal();
            GUILayout.Box("Plugin Disabled", StyleBoxYellow, GUILayout.Width(112), GUILayout.Height(30));
            if (settings.disablePlugin == true)
            {
                GUILayout.Box("TRUE" ,StyleBoxGreen, GUILayout.Width(112), GUILayout.Height(30));                
            }

            if (settings.disablePlugin == false)
            {
                GUILayout.Box("FALSE", StyleBoxYellow, GUILayout.Width(112), GUILayout.Height(30));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("GameMode: ", StyleBoxYellow, GUILayout.Width(112), GUILayout.Height(30));
            if (settings.gameMode == 0)
            {
                GUILayout.Box("Normal", StyleBoxGreen, GUILayout.Width(112), GUILayout.Height(30));
            }

            if (settings.gameMode == 1)
            {
                GUILayout.Box("HardCore",StyleBoxYellow, GUILayout.Width(112), GUILayout.Height(40));
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Disable Plugin",styleButtonWordWrap))
            {
                settings.disablePlugin = !settings.disablePlugin;
            }

            if (GUILayout.Button("Normal Mode", styleButtonWordWrap))
            {
                settings.gameMode = 0;
            }

            if (GUILayout.Button("HardCore Mode", styleButtonWordWrap))
            {
                settings.gameMode = 1;
            }
            GUILayout.Space(10);

            if ((GUILayout.Button("GUI Skin Type",styleButtonWordWrap)))
            {
                settings.KSPSKIN = !settings.KSPSKIN;
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(resetStrings[resetCount],styleButtonWordWrap))
            {
                resetCount++;
                if (resetCount >= resetStrings.Length)
                {
                    resetCount = 0;
                    manager.resetSpaceProgram();
                    manager.saveProgramBackup();
                }
            }
            if (resetCount >= 1 && GUILayout.Button("NO", styleButtonWordWrap))
            {
                resetCount = 0;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(resetGoals[resetGoalsCount], styleButtonWordWrap))
            {
                resetGoalsCount++;
                if (resetGoalsCount >= resetGoals.Length)
                {
                    resetGoalsCount = 0;
                    manager.wipeAllMissionGoals();
                }
            }
            if (resetGoalsCount >= 1 && GUILayout.Button("NO", styleButtonWordWrap))
            {
                resetGoalsCount = 0;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(resetMissions[resetMissionCount], styleButtonWordWrap))
            {
                resetMissionCount++;
                if (resetMissionCount >= resetMissions.Length)
                {
                    resetMissionCount = 0;
                    manager.wipeAllFinishedMissions();
                }
            }
            if (resetMissionCount >= 1 && GUILayout.Button("NO", styleButtonWordWrap))
            {
                resetMissionCount = 0;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            //if (GUILayout.Button("Simulate Contract Resets", styleButtonWordWrap))
            //{
            //    manager.StartContractTypeRandom();
            //    manager.StartCompanyRandomizer();
            //    manager.setContractType();
            //    manager.StartContractType1Random();
            //    manager.setContractType1();
            //    manager.StartContractType2Random();
            //    manager.setContractType2();
            //    manager.SetClockCountdown();
            //    manager.setCompanyName();
            //    manager.chooseRandomValues();
            //}           

            //if (GUILayout.Button("FIND ASTERIODS AND CHOOSE NAME", styleButtonWordWrap))
            //{
            //    manager.clearAsteroidFindList();
            //    manager.findAsteriodCapture();
            //}
            //if (GUILayout.Button("FIND VESSEL REPAIRS", styleButtonWordWrap))
            //{               
            //    manager.findVeselWithRepairPart();
            //    manager.clearVesselRepairFromList();
            //} 

            //if (GUILayout.Button("window Test Recycle", styleButtonWordWrap))
            //{
            //    showRecycleWindow = true;
            //}

            //if (GUILayout.Button("window Test Finish mission", styleButtonWordWrap))
            //{
            //    showRandomWindow = true;
            //}

            if (GUILayout.Button("Reset Contract Time Check", styleButtonWordWrap))
            {
                manager.SetCurrentCheckTime(0);
                Debug.Log("Current Time Check Reset To 0");
            }
            if (GUILayout.Button("Reset Window Postions", styleButtonWordWrap))
            {
                ResetWindows();
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Save Settings", styleButtonWordWrap))
            {
                settingsWindow(!showSettingsWindow);
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