using System;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        private void drawSettingsWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical ();

            settings.DisablePlugin = GUILayout.Toggle (settings.DisablePlugin, "Disable plugin. No launch costs");

            GUILayout.EndVertical ();
            GUI.DragWindow ();

            SettingsManager.Manager.saveSettingsIfChanged ();
        }
    }
}

