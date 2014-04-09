using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        public bool shipStatMissionBool = false;
        public bool shipStatcontractBool = false;
        private Vector2 minScrollPosition;
        private void drawShipStatsWindow(int id)
        {            
            Status status = calculateStatus(currentMission, true, activeVessel);
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            minScrollPosition = GUILayout.BeginScrollView(minScrollPosition);
            if (currentMission != null)
            {
                drawMiniContractsGoals(currentMission, status);
            }
            else { GUILayout.Label("No Missions Selected"); }
            GUILayout.EndScrollView();
            if (GUILayout.Button("Normal View", styleButtonWordWrap))
            {
                showShipStatsWindow = false;
                showMissionStatusWindow = false;
                if (shipStatcontractBool != false) { showContractStatusWindow = true; shipStatcontractBool = false; }
                if (shipStatMissionBool != false) { showMissionStatusWindow = true; shipStatMissionBool = false; }
            }
            GUILayout.EndVertical();           
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        private void drawMiniContractsGoals(Mission mission, Status s)
        {
            foreach (MissionGoal c in mission.goals)
            {
                List<Value> values = c.getValues(activeVessel, s.events);

                foreach (Value v in values)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(v.name, styleValueName);                    
                    
                    GUILayout.Label(v.shouldBe + " : " + v.currentlyIs, (v.done ? styleValueGreen : styleValueRed));
                                     
                    GUILayout.EndHorizontal();
                }


            }

        }
    }
}
