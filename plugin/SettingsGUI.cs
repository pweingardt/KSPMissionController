using System;
using UnityEngine;
using System.Text.RegularExpressions;

namespace MissionController
{
    /// <summary>
    /// Responsible for the settings window
    /// </summary>
    public partial class MissionController
    {
        private int resetCount = 0;
        private String[] resetStrings = new String[] {"Reset the space program!", "Are you sure?", "Are you *really* sure?",
            "I don't think you are...", "Ok... fine...", "Last chance!"
        };

        private int rewindCount = 0;
        private String[] rewindStrings = new String[] {"Rewind", "Are you sure?", "So you messed up?", "LOL! And you want to get your latest expenses back?",
            "Well, let me see what I can do...", "Good news and bad news: You should not be running a space program and you will get the latest expenses back ;).",
            "Come on man, take that joke...", "Ok, last chance!"
        };

        private String contributions = "Plugin information and contributions:\nMain developer: nobody44\ndeveloper: vaughner81\nimages: BlazingAngel665\n" +
            "ideas: BaphClass\nideas: tek_604\nand of course the great community around KSP! You guys are awesome :)!";

        private String[] difficulties = new String[] { "easy", "medium", "hard" };

        private void drawSettingsWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical ();

            settings.disablePlugin = GUILayout.Toggle (settings.disablePlugin, "Disable plugin. No launch costs");

            GUILayout.Label ("Kerbonaut insurance cost: ", styleCaption);
            settings.kerbonautCost = GUILayout.TextField (settings.kerbonautCost);
            settings.kerbonautCost = Regex.Replace(settings.kerbonautCost, @"[a-zA-Z\\. ]", "");

            GUILayout.Label ("Difficulty: ", styleCaption);
            settings.difficulty = GUILayout.SelectionGrid (settings.difficulty, difficulties, 3);

            GUILayout.Space (30);

            GUILayout.Label (contributions, styleText);

            if (GUILayout.Button (rewindStrings[rewindCount], styleButtonWordWrap)) {
                rewindCount++;
                if (rewindCount >= rewindStrings.Length) {
                    rewindCount = 0;
                    manager.rewind ();
                }
            }

            if (GUILayout.Button (resetStrings[resetCount], styleButtonWordWrap)) {
                resetCount++;
                if (resetCount >= resetStrings.Length) {
                    resetCount = 0;
                    manager.resetSpaceProgram ();
                }
            }

            if (GUILayout.Button ("Save and Close Settings", styleButton)) {
                showSettingsWindow = false;
                Difficulty.init (settings.difficulty);
                SettingsManager.Manager.saveSettings ();
            }

            GUILayout.EndVertical ();
            GUI.DragWindow ();
        }
    }
}

