using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        private void drawShipStatsWindow(int id)
        {
            Vessel v = new Vessel();
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            //GUILayout.Label(v.orbit.ApA.ToString());
            //GUILayout.Label(v.orbit.PeA.ToString());
            //GUILayout.Label("Orbital Period: " + String.Format(MathTools.SingleDoubleValue, v.orbit.period.ToString()));
            //GUILayout.Label("Orbital Altitude: " + v.orbit.altitude.ToString());
            //GUILayout.Label("Inclination: " + String.Format(MathTools.SingleDoubleValue, v.orbit.inclination.ToString()));
            //GUILayout.Label("Eccentricity: " + String.Format(MathTools.SingleDoubleValue, v.orbit.eccentricity.ToString()));
            //GUILayout.Label("Vessel Mass: " + String.Format(MathTools.SingleDoubleValue, v.GetTotalMass().ToString()));
            
            GUILayout.EndVertical();
            if (GUILayout.Button("Exit", styleButtonWordWrap))
            {
                showShipStatsWindow = false;
            }            
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
    }
}
