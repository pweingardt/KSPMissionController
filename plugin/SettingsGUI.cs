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

            GUILayout.BeginHorizontal();
            GUILayout.Box("Multi Vessel Landings Allowed", StyleBoxYellow, GUILayout.Width(112), GUILayout.Height(50));
            if (settings.allowApolloLandings == true)
            {
                GUILayout.Box("TRUE", StyleBoxGreen, GUILayout.Width(112), GUILayout.Height(50));
            }

            if (settings.allowApolloLandings == false)
            {
                GUILayout.Box("FALSE", StyleBoxYellow, GUILayout.Width(112), GUILayout.Height(50));
            }            
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Disable Plugin",styleButtonWordWrap))
            {
                settings.disablePlugin = !settings.disablePlugin;
            }
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "Will disable MCE plugin, any ship you launch while plugin is disabled will be flagged and can never be used in a Mission! While disabled nothing cost money.";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Normal Mode", styleButtonWordWrap))
            {
                settings.gameMode = 0;
            }
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "Nornal mode all prices and Mission Payouts are Default.";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("HardCore Mode", styleButtonWordWrap))
            {
                settings.gameMode = 1;
            }
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "Hardcore mode all mission payouts are reduced by 40%.  Making MCE much more difficult to play!.";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Allow Multi Stage Landings", styleButtonWordWrap))
            {
                settings.allowApolloLandings = !settings.allowApolloLandings;
            }
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "This setting allows you to land vessel during mission's Apollo Style. Traditional Missions only allow 1 Vessel and It's ID to complete missions. This Setting allows you to use 2 sepeate vessels with Sepeate Id's, and not suffer the Ship ID Problems During Landing Phase of a mission!\n\n"+ 
                    "DO NOT change vessel while landing, if you switch to a vessel that is already landed then that will be recorded as a landing.  If you don't plan on doing Apollo style mission then TURN THIS TO FALSE, and you can switch vessel without any issues!  You have been warned!";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if ((GUILayout.Button("GUI Skin Type",styleButtonWordWrap)))
            {
                settings.KSPSKIN = !settings.KSPSKIN;
            }
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "Change the GUI skin from Kerbal GUI to the Smoke GUI.";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();

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
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "This will reset your whole space program. Meaning everything will be reset to StartUp Values!  Do this if restarting a game with the same name.";
                showEventWindow = true;
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
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "This will reset all mission goals in your current game.  This can help if for some reason your mission is stuck! Most of the time this is not needed because most issues have been fixed.  But just in case.";
                showEventWindow = true;
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
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "This will erase everysingle mission you have done in this game. Why do you need to do this? No idea but I suggest you don't.  Will keep budget intact though, and everything else that is not mission releated.";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (settings.MCEDebug != false)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Debug Tools Start", StyleBoxYellow, GUILayout.Width(225), GUILayout.Height(30));
                GUILayout.EndHorizontal();
            }

            if (settings.MCEDebug != false){
                if (GUILayout.Button("Simulate Contract Resets", styleButtonWordWrap))
                {
                    manager.StartContractTypeRandom();
                    manager.StartCompanyRandomizer();
                    manager.setContractType();
                    manager.StartContractType1Random();
                    manager.setContractType1();
                    manager.StartContractType2Random();
                    manager.setContractType2();
                    manager.SetClockCountdown();
                    manager.setCompanyName();
                    manager.chooseRandomValues();
                }
            }
            if (settings.MCEDebug != false)
            {
                if (settings.MCEDebug != false && GUILayout.Button("FIND ASTERIODS AND CHOOSE NAME", styleButtonWordWrap))
                {
                    manager.clearAsteroidFindList();
                    manager.findAsteriodCapture();
                }
            }
            if (settings.MCEDebug != false)
            {
                if (settings.MCEDebug != false && GUILayout.Button("FIND VESSEL REPAIRS", styleButtonWordWrap))
                {
                    manager.findVeselWithRepairPart();
                    manager.clearVesselRepairFromList();
                }
            }
            if (settings.MCEDebug != false)
            {
                if (GUILayout.Button("Add 100000 Credits", styleButtonWordWrap))
                {
                    manager.modReward(100000, "Using Debug Menu");
                }
            }
            if (settings.MCEDebug != false)
            {
                if (GUILayout.Button("Add 1000 Science", styleButtonWordWrap))
                {
                    manager.sciencereward(1000);
                }
            }
            if (settings.MCEDebug != false)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Debug Tools End", StyleBoxYellow, GUILayout.Width(225), GUILayout.Height(30));
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Contract Time Check", styleButtonWordWrap))
            {
                manager.SetCurrentCheckTime(0);
                Debug.Log("Current Time Check Reset To 0");
            }
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "If for some reason you think that the Random Contracts is stuck and not giving you contracts every 24 hours, this will reset the whole system and start it back up!";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Window Postions", styleButtonWordWrap))
            {
                ResetWindows();
            }
            if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
            {
                messageEvent = "This will reset all your windows in MCE. Use this if your window disapears or gets messed up in some other way";
                showEventWindow = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            //if (GUILayout.Button("Disable Vessel", styleButtonWordWrap))
            //{
            //    settings.disableFlight = !settings.disableFlight;
            //    messageEvent = "Warning You Current pilot is not qualified to Pilot this vessel, Please go back to VAB and select a qualified pilot or commander!\n\n Your Vessel is disabled, if you launch it at this time your pilot will have no control and bad things will happen!";
            //    showEventWindow = true;
            //}
            if (GUILayout.Button("Save Settings", styleButtonWordWrap))
            {
                settingsWindow(!showSettingsWindow);
                //Difficulty.init(settings.difficulty);
                FuelMode.fuelinit(manager.GetFuels);
                ConstructionMode.constructinit(manager.GetConstruction);
                PayoutLeveles.payoutlevels(manager.GetCurrentPayoutLevel);
                SettingsManager.Manager.saveSettings();
                button.TexturePath = mcetbState1 ? "MissionController/icons/settings" : "MissionController/icons/settingsr";
                mcetbState1 = !mcetbState1;
            }
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
            
        }
    }
}