﻿using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;


namespace MissionController
{
    /// <summary>
    /// Responsible for the new finace window
    /// </summary>
    public partial class MissionController
    {
        
        private Vector2 scrollPosition2 = new Vector2(0, 0);
        private void drawFinaceWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box(" Current budget: ", StyleBoxWhite, GUILayout.Width(190), GUILayout.Height(30));
            GUILayout.Box(manager.budget + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(30));
            GUILayout.EndHorizontal();
           
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Box("Space Program expenditure's ",StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(25));            
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(" Total Spent Vessels: ", StyleBoxWhite, GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalSpentVechicles + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(" Total Kerbal Hire Cost: ", StyleBoxWhite, GUILayout.Height(30));
            GUILayout.Box(manager.TotalHiredKerbCost + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(" Total Expenditure's: ", StyleBoxWhite, GUILayout.Height(30));
            GUILayout.Box(manager.TotalHiredKerbCost + manager.TotalSpentVechicles + CurrencySuffix, StyleBoxGreen, GUILayout.Width(110), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Kerbal Hire Log",GUILayout.Height(20)))
            {
                showKerbalLogbookHire = !showKerbalLogbookHire;
            }

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Box("Space Program Profits", StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(" Total Mission Payouts: ", StyleBoxWhite, GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.Totalbudget + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(" Total Recyling Paid: ", StyleBoxWhite, GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalRecycleMoney + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(" Total Recyling + Payouts: ", StyleBoxWhite, GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalRecycleMoney + manager.Totalbudget + CurrencySuffix, StyleBoxGreen, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

 
            if (GUILayout.Button("Kerbal Mission Log", GUILayout.Height(20)))
            {
                showMissionLogbookWindow = !showMissionLogbookWindow;
            }


            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Box("Net Profits: ", StyleBoxYellow, GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalRecycleMoney + manager.Totalbudget - manager.TotalSpentVechicles - manager.TotalHiredKerbCost + CurrencySuffix,StyleBoxGreen, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box(" Kerbonaut Hire Cost: ", StyleBoxWhite, GUILayout.Height(30));
            int HC = Tools.GetValueDefault(Tools.MCSettings, "kerbalHireCost", 5000);
            GUILayout.Box(HC + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(30));
            GUILayout.EndHorizontal();
           
            GUILayout.Space(20);
            if (manager.budget < 0)
            {
                GUILayout.Box(" Borrowing Money", StyleBoxWhite, GUILayout.Height(30));
            GUILayout.BeginHorizontal();
            GUILayout.Box(" Current Bank Loan: ", StyleBoxWhite, GUILayout.Width(150), GUILayout.Height(25));
            GUILayout.Box(manager.budget + CurrencySuffix, GUILayout.Width(150), GUILayout.Height(25));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Box(" Mission Payout Rate: ", StyleBoxWhite, GUILayout.Width(150), GUILayout.Height(25));
            GUILayout.Box(FinanceMode.currentloan * 100 + "%", GUILayout.Width(150), GUILayout.Height(25));
            GUILayout.EndHorizontal();
             }

            GUILayout.Space(20);
            GUILayout.Box("Current Passive Mission Payouts");
            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, GUILayout.Width(300), GUILayout.Height(100));
            drawPassiveMissions(manager.getActivePassiveMissions());
            GUILayout.EndScrollView();

            GUILayout.Space(20);
            if (GUILayout.Button("Exit Window"))
            {
                financeWindow(false);
            }

            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
            
        }

        /// <summary>
        /// Draws the currently active passive missions
        /// </summary>
        /// <param name="missions">Missions.</param>
        private void drawPassiveMissions(List<MissionStatus> missions)
        {
            if (missions.Count > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Next payment in", styleValueName);
                GUILayout.Label(MathTools.formatTime(60.0 * 60.0 * 24.0 - (Planetarium.GetUniversalTime() - lastPassiveReward)), styleValueGreen);
                GUILayout.EndHorizontal();

                int total = 0;
                foreach (MissionStatus m in missions)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(m.missionName, styleValueName);
                    GUILayout.Label(m.passiveReward + CurrencySuffix, styleValueGreen);
                    GUILayout.EndHorizontal();
                    total += m.passiveReward;
                }

                GUILayout.Space(20);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Total", styleValueName);
                GUILayout.Label(total + CurrencySuffix, styleValueGreen);
                GUILayout.EndHorizontal();
            }
        }
    }
}
