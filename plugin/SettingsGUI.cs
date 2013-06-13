using System;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        private int resetCount = 0;
        private String[] resetStrings = new String[] {"Reset the space program!", "Are you sure?", "Are you *really* sure?",
            "I don't think you are...", "Ok... fine...", "Last chance!"
        };

        private String contributions = "Plugin information and contributions:\nMain developer: nobody44\ndeveloper: vaughner81\nimages: BlazingAngel665\n" +
            "ideas: BaphClass\nideas: tek_604\nand of course the great community around KSP! You guys are awesome :)!";


        private void drawSettingsWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical ();

            settings.DisablePlugin = GUILayout.Toggle (settings.DisablePlugin, "Disable plugin. No launch costs");

            GUILayout.Label (contributions, styleText);

            if (GUILayout.Button (resetStrings[resetCount], styleButton)) {
                resetCount++;
                if (resetCount >= resetStrings.Length) {
                    resetCount = 0;
                    manager.resetSpaceProgram ();
                }
            }

            if (GUILayout.Button ("Close Settings", styleButton)) {
                showSettingsWindow = false;
            }

            GUILayout.EndVertical ();
            GUI.DragWindow ();

            SettingsManager.Manager.saveSettingsIfChanged ();
        }
    }
}

