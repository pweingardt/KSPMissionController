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
            GUILayout.Box("Current budget: ", styleValueGreenBold);
            GUILayout.Box(manager.budget + CurrencySuffix, styleValueGreenBold);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Money Made Missions: ", styleValueGreenBold);
            GUILayout.Box(manager.Totalbudget + manager.TotalSpentVechicles + CurrencySuffix);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Recycle Made: ", styleValueGreenBold);
            GUILayout.Box(manager.TotalRecycleMoney + CurrencySuffix);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Profit For All Above: ", styleValueGreenBold);
            GUILayout.Box(manager.TotalRecycleMoney + manager.Totalbudget + manager.TotalSpentVechicles + CurrencySuffix);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Spent On Vessels: ", styleValueGreenBold);
            GUILayout.Box(manager.TotalSpentVechicles + CurrencySuffix);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.Label("Kerbonaut insurance cost: ",styleValueGreenBold);
            settings.kerbonautCost = GUILayout.TextField(settings.kerbonautCost);
            settings.kerbonautCost = Regex.Replace(settings.kerbonautCost, @"[^0-9]", "");    

            GUILayout.Space(20);
            if (manager.budget < 0)
            {
            GUILayout.Label("Borrowing Money", styleWarning);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Bank Loan: ", styleValueGreen);
            GUILayout.Label(manager.budget + CurrencySuffix, styleValueYellow);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Current Interest Rate: ", styleValueGreen);
            GUILayout.Label("25%", styleValueYellow);
            GUILayout.EndHorizontal();
             }

            GUILayout.Space(20);
            scrollPosition2 = GUILayout.BeginScrollView (scrollPosition2, GUILayout.Width(290));
            GUILayout.Label("Current Passive Mission Payouts", styleValueGreenBold);
            drawPassiveMissions(manager.getActivePassiveMissions());
            GUILayout.EndScrollView();
            


            GUILayout.Space(20);
            if (GUILayout.Button("Exit Window"))
            {
                financeWindow(false);
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
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
