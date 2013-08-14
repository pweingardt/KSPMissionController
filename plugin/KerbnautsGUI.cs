using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace MissionController
{
    /// <summary>
    /// This Draws the Window For KerbalNauts Recruitment Screen
    /// </summary>
    public partial class MissionController
    {
        
        private void drawKerbalnautWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    
    
      
    
       
    }
}
