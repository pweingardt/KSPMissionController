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

            //if (GUILayout.Button("Set Values Randoms + Find Vessel", styleButtonWordWrap))
            //{
            //    manager.StartContractType1Random();
            //    manager.StartContractType2Random();
            //    manager.StartCompanyRandomizer();
            //    manager.setContractType();
            //    manager.setContractType2();
            //    manager.setCompanyName();
            //    manager.findVeselWithRepairPart();
            //}

            //if (GUILayout.Button("Set Vessel Repair To Save", styleButtonWordWrap))
            //{
            //    manager.chooseVesselRepairFromList();
            //}

            //if (GUILayout.Button("Show Work Window (Finish Mission)", styleButtonWordWrap))
            //{
            //    showRandomWindow = !showRandomWindow;
            //}           

            //if (GUILayout.Button("CheckHired Kerbals", styleButtonWordWrap))
            //{
            //    manager.isKerbalHired();
            //}

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