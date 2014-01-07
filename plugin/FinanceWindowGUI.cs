using System;
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
            GUILayout.Box("Current budget: ", GUILayout.Width(190),GUILayout.Height(30));
            GUILayout.Box(manager.budget + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(30));
            GUILayout.EndHorizontal();           

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Mission Payouts: ", GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.Totalbudget + manager.TotalSpentVechicles + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Reycling Paid: ", GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalRecycleMoney + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Payouts Above: ",GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalRecycleMoney + manager.Totalbudget + manager.TotalSpentVechicles + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Vessel Cost: ", GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalSpentVechicles + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Profits: ", GUILayout.Width(190), GUILayout.Height(25));
            GUILayout.Box(manager.TotalRecycleMoney + manager.Totalbudget + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(25));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Kerbonaut Hire Cost: ", GUILayout.Height(30));
            int insuranceCost = Tools.GetValueDefault(Tools.MCSettings, "kerbalHireCost", 5000);
            GUILayout.Box(insuranceCost + CurrencySuffix, GUILayout.Width(110), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            if (manager.budget < 0)
            {
            GUILayout.Box("Borrowing Money", GUILayout.Height(30));
            GUILayout.BeginHorizontal();
            GUILayout.Box("Current Bank Loan: ", GUILayout.Width(150), GUILayout.Height(25));
            GUILayout.Box(manager.budget + CurrencySuffix, GUILayout.Width(150), GUILayout.Height(25));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Box(" Mission Payout Rate: ", GUILayout.Width(150), GUILayout.Height(25));
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
