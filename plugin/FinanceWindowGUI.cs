using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace MissionController
{
    /// <summary>
    /// Responsible for the new finace window
    /// </summary>
    public partial class MissionController
    {
        private void drawFinaceWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            if (manager.budget < 0)
            {
            GUILayout.Label("Your In Red And Borrowing Money At Current Interest", styleWarning);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Bank Loan: ", styleValueGreen);
            GUILayout.Label(manager.budget + CurrencySuffix, styleValueYellow);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(" Current Interest Rate: ", styleValueGreen);
            GUILayout.Label("15%", styleValueYellow);
            GUILayout.EndHorizontal();
             }
            GUILayout.Label("Current Passive Mission Payouts", styleValueGreenBold);

            scrollPosition2 = GUILayout.BeginScrollView(scrollPosition);
            drawPassiveMissions(manager.getActivePassiveMissions());
            GUILayout.Space(30);
            GUILayout.EndScrollView();

            GUILayout.Space(30);

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
