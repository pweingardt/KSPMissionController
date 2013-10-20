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
           
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Current Science", GUILayout.Width(250), GUILayout.Height(40));
            GUILayout.Box((int) CM.Science + " Science", GUILayout.Width(250), GUILayout.Height(40));
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
                if (GUILayout.Button("Purchase", GUILayout.Width(160), GUILayout.Height(40)))
                {
                    CM.DeductScience(300);
                    manager.SetResearchRecycle();
                }
            }
            else
            {
                if (manager.ResearchRecycle != false)
                {
                    GUILayout.Box("Researched", GUILayout.Width(160), GUILayout.Height(40));
                }
                else
                {
                    GUILayout.Box("NOT AVAILABLE", GUILayout.Width(160), GUILayout.Height(40));
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Efficient Fuels", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box(" 500 Science", GUILayout.Width(160), GUILayout.Height(40));
            if (CM.Science >= 500 && manager.ResearchFuels != true)
            {
                if (GUILayout.Button("Purchase", GUILayout.Width(160), GUILayout.Height(40)))
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
                    GUILayout.Box("Researched", GUILayout.Width(160), GUILayout.Height(40));
                }
                else
                {
                    GUILayout.Box("NOT AVAILABLE", GUILayout.Width(160), GUILayout.Height(40));
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Construction 1", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box("120 Science", GUILayout.Width(160), GUILayout.Height(40));
            if (CM.Science >= 120 && manager.ResearchConstruction1 != true)
            {
                if (GUILayout.Button("Purchase", GUILayout.Width(160), GUILayout.Height(40)))
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
                    GUILayout.Box("Researched", GUILayout.Width(160), GUILayout.Height(40));
                }
                else
                {
                    GUILayout.Box("NOT AVAILABLE", GUILayout.Width(160), GUILayout.Height(40));
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Construction 2", GUILayout.Width(160), GUILayout.Height(40));
            GUILayout.Box("500 Science", GUILayout.Width(160), GUILayout.Height(40));
            if (CM.Science >= 500 && manager.ResearchConstruction2 != true)
            {
                if (GUILayout.Button("Purchase", GUILayout.Width(160), GUILayout.Height(40)))
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
                    GUILayout.Box("Researched", GUILayout.Width(160), GUILayout.Height(40));
                }
                else
                {
                    GUILayout.Box("NOT AVAILABLE", GUILayout.Width(160), GUILayout.Height(40));
                }
            }
            GUILayout.EndHorizontal();       

            GUILayout.Space(20);
            if (GUILayout.Button("Exit Window"))
            {

                //Difficulty.init(settings.difficulty);                

                SettingsManager.Manager.saveSettings();
                manager.saveProgram();
                FuelMode.fuelinit(manager.GetFuels);
                ConstructionMode.constructinit(manager.GetConstruction);

                ResearchTreeWindow(false);
            }

            GUILayout.EndVertical();
            GUI.DragWindow();

        }
    }
}
