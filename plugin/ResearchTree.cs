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
          /// <summary>
          /// This is the Research Tree where Players Pick and Buy Research in the GUI
          /// </summary>
          /// <param name="id"></param>
        private void drawResearchTree(int id)
        {
            
            ConstructionMode CM = new ConstructionMode();
            SpaceProgram sp = new SpaceProgram();
            Mission ms = new Mission();
            PayoutLeveles PL = new PayoutLeveles();
           
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER)
            {
                GUILayout.Label("Research Not Available In SandboxMode");
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Current Science", GUILayout.Width(250), GUILayout.Height(40));
                GUILayout.Box((int)CM.Science + " Science", GUILayout.Width(250), GUILayout.Height(40));
                GUILayout.EndHorizontal();               

                GUILayout.BeginHorizontal();
                GUILayout.Box("          MC TechTree", styleValueGreenBold, GUILayout.Width(160), GUILayout.Height(30));
                GUILayout.Box("          Purchase Cost", styleValueGreenBold, GUILayout.Width(160), GUILayout.Height(30));
                GUILayout.Box("          Research Status", styleValueGreenBold, GUILayout.Width(160), GUILayout.Height(30));
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Vessel Recycling", GUILayout.Width(160), GUILayout.Height(40));
                GUILayout.Box("300 Science", GUILayout.Width(160), GUILayout.Height(40));
                if (CM.Science >= 300 && !manager.ResearchRecycle)
                {
                    if (GUILayout.Button("Purchase", GUILayout.Width(150), GUILayout.Height(40)))
                    {
                        CM.DeductScience(300);
                        manager.SetResearchRecycle();
                    }
                }
                else
                {
                    if (manager.ResearchRecycle != false)
                    {
                        GUILayout.Box("Researched", GUILayout.Width(140), GUILayout.Height(40));
                    }
                    else
                    {
                        GUILayout.Box("NOT AVAILABLE", GUILayout.Width(140), GUILayout.Height(40));
                    }
                }
                if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    messageEvent = "Someone decided that maybe its a good idea to add lots of parachutes to Spent stages and maybe resuse them?";
                    showEventWindow = true;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Rocket Assisted Auto Landing", GUILayout.Width(160), GUILayout.Height(40));
                GUILayout.Box("600 Science", GUILayout.Width(160), GUILayout.Height(40));               
                if (CM.Science >= 600 && !manager.GetRocketAutoLand && manager.ResearchRecycle)
                {
                    if (GUILayout.Button("Purchase", GUILayout.Width(150), GUILayout.Height(40)))
                    {
                        CM.DeductScience(600);
                        manager.SetRocketAutoLand();
                    }
                }
                else
                {
                    if (manager.ResearchRecycle != false)
                    {
                        GUILayout.Box("Researched", GUILayout.Width(140), GUILayout.Height(40));
                    }
                    else
                    {
                        GUILayout.Box("NOT AVAILABLE", GUILayout.Width(140), GUILayout.Height(40));
                    }
                }
                if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    messageEvent = "Jeb said  \"why don't we use the engines on a returning rocket to land the staged rockets?\"\n\n  Then the engineer said \"it can't be done!\"\n\n Jeb then" +
                    " strapped the engineer to an old rocket and launched it into the air. He used a new Prototype remote control thing to land the rocket kinda intact on the ground. \n\nAfter" +
                    " the hospital visit the engineer decided it was possible to land a spent stage via rocket engine! \n\nYou must have 1000 Delta V Left in rocket and 1.5 TWR. You need to research Recyling First!";
                    showEventWindow = true;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Efficient Fuels", GUILayout.Width(160), GUILayout.Height(40));
                GUILayout.Box(" 500 Science", GUILayout.Width(160), GUILayout.Height(40));
                if (CM.Science >= 500 && manager.ResearchFuels != true)
                {
                    if (GUILayout.Button("Purchase", GUILayout.Width(150), GUILayout.Height(40)))
                    {
                        CM.DeductScience(500);
                        manager.SetResearchFuels();
                        manager.SetFuels();
                    }
                }
                else
                {
                    if (manager.ResearchFuels != false)
                    {
                        GUILayout.Box("Researched", GUILayout.Width(140), GUILayout.Height(40));
                    }
                    else
                    {
                        GUILayout.Box("NOT AVAILABLE", GUILayout.Width(140), GUILayout.Height(40));
                    }
                }
                if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    messageEvent = "Someone accidently dumped some strange fluid in a Liquid Fuel tank.  It seems that adding this new fluid gives you more volume of fuel per unit without many ill effects. And it’s cheap to!";
                    showEventWindow = true;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Construction 1", GUILayout.Width(160), GUILayout.Height(40));
                GUILayout.Box("120 Science", GUILayout.Width(160), GUILayout.Height(40));
                if (CM.Science >= 120 && manager.ResearchConstruction1 != true)
                {
                    if (GUILayout.Button("Purchase", GUILayout.Width(150), GUILayout.Height(40)))
                    {
                        CM.DeductScience(120);
                        manager.SetResearchConstruction1();
                        manager.SetConstruction(1);
                    }
                }
                else
                {
                    if (manager.ResearchConstruction1 != false)
                    {
                        GUILayout.Box("Researched", GUILayout.Width(140), GUILayout.Height(40));
                    }
                    else
                    {
                        GUILayout.Box("NOT AVAILABLE", GUILayout.Width(140), GUILayout.Height(40));
                    }
                }
                if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    messageEvent = "Jeb has figured out that if you find something on the side of the road it's much cheaper to use than new stuff!";
                    showEventWindow = true;
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Construction 2", GUILayout.Width(160), GUILayout.Height(40));
                GUILayout.Box("500 Science", GUILayout.Width(160), GUILayout.Height(40));
                if (CM.Science >= 500 && manager.ResearchConstruction2 != true && manager.ResearchConstruction1 != false)
                {
                    if (GUILayout.Button("Purchase", GUILayout.Width(150), GUILayout.Height(40)))
                    {
                        CM.DeductScience(500);
                        manager.SetResearchConstruction2();
                        manager.SetConstruction(2);
                    }
                }
                else
                {
                    if (manager.ResearchConstruction2 != false)
                    {
                        GUILayout.Box("Researched", GUILayout.Width(140), GUILayout.Height(40));
                    }
                    else
                    {
                        GUILayout.Box("NOT AVAILABLE", GUILayout.Width(140), GUILayout.Height(40));
                    }
                }
                if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    messageEvent = "Bill and bob found a new way to construct ships! It's even more creative then Jeb's roadside junk! Jeb has no comment.";
                    showEventWindow = true;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Mission Payouts 2", GUILayout.Width(160), GUILayout.Height(40));
                GUILayout.Box("800 Science", GUILayout.Width(160), GUILayout.Height(40));
                if (CM.Science >= 800 && manager.MissionLevel2 != true)
                {
                    if (GUILayout.Button("Purchase", GUILayout.Width(150), GUILayout.Height(40)))
                    {
                        CM.DeductScience(800);
                        manager.SetCurrentPayoutLevel(1);
                        manager.SetMissionLevel2();
                    }
                }
                else
                {
                    if (manager.MissionLevel2 != false)
                    {
                        GUILayout.Box("Researched", GUILayout.Width(140), GUILayout.Height(40));
                    }
                    else
                    {
                        GUILayout.Box("NOT AVAILABLE", GUILayout.Width(140), GUILayout.Height(40));
                    }
                }
                if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    messageEvent = "Jeb had a talk with some of the contract companies!  Now all payouts have been increased and come with a note that says sorry?  Jeb, what did you do?";
                    showEventWindow = true;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Mission Payouts 3", GUILayout.Width(160), GUILayout.Height(40));
                GUILayout.Box("1,600 Science", GUILayout.Width(160), GUILayout.Height(40));
                if (CM.Science >= 1600 && manager.MissionLevel3 != true && manager.MissionLevel2 != false)
                {
                    if (GUILayout.Button("Purchase", GUILayout.Width(150), GUILayout.Height(40)))
                    {
                        CM.DeductScience(1600);
                        manager.SetCurrentPayoutLevel(2);
                        manager.SetMissionLevel3();
                    }
                }
                else
                {
                    if (manager.MissionLevel3 != false)
                    {
                        GUILayout.Box("Researched", GUILayout.Width(140), GUILayout.Height(40));
                    }
                    else
                    {
                        GUILayout.Box("NOT AVAILABLE", GUILayout.Width(140), GUILayout.Height(40));
                    }
                }
                if (GUILayout.Button("i", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    messageEvent = "Jeb won a game of poker against Bill, now all payouts for missions are even better! Yay Jeb. Bill now complains he has no money";
                    showEventWindow = true;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);

            }
            if (GUILayout.Button("Exit Window"))
            {

                //Difficulty.init(settings.difficulty);                

                SettingsManager.Manager.saveSettings();               
                FuelMode.fuelinit(manager.GetFuels);
                ConstructionMode.constructinit(manager.GetConstruction);
                PayoutLeveles.payoutlevels(manager.GetCurrentPayoutLevel);
                manager.saveProgram();
                ScienceResearch.TexturePath = mcetbState6 ? "MissionController/icons/research" : "MissionController/icons/researchr";
                mcetbState6 = !mcetbState6;

                researchWindow(!showResearchTreeWindow);
            }

            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
            
        }
    }
}
