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
        private String[] resetStrings = new String[] {"Reset Your MCE SaveGame", "This Will Reset Your Save!"};
        
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
            GUILayout.Space(5);
            GUILayout.Label("Will Reset SpaceProgram Default Values");
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
            GUILayout.Space(5);
            GUILayout.Label("Will Reset Mission Goals");
            if (GUILayout.Button("Clear Saved mission Goals", styleButtonWordWrap))
            {
                manager.wipeAllMissionGoals();
            }
            GUILayout.Space(5);
            GUILayout.Label("Will Delete All Finish Missions");
            if (GUILayout.Button("Clear All Finished Missions", styleButtonWordWrap))
            {
                manager.wipeAllFinishedMissions();
            }
            GUILayout.Space(5);
            
            //if (GUILayout.Button("Set Values Randoms + Find Vessel", styleButtonWordWrap))
            //{
            //    manager.StartContractTypeRandom();
            //    manager.StartContractType1Random();
            //    manager.StartContractType2Random();
            //    manager.StartCompanyRandomizer();
            //    manager.setContractType();
            //    manager.setContractType1();
            //    manager.setContractType2();
            //    manager.setCompanyName();
            //    manager.clearVesselRepairFromList();
            //    manager.findVeselWithRepairPart();
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