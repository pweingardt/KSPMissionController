using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

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

        private String contributions = "Original Mission Controller\n" +
            "MC Main developer: nobody44\n" +
            "MC developer: vaughner81\n" +
            "(Mission Controller Extended Version)\n" +
            "MCE Developer malkuth .11 up \n" +
            "MCE Developer NathanKell .11 up \n" +
            "images: bac9\n" +
            "old images: BlazingAngel665\n" +
            "ideas: BaphClass\n" +
            "ideas: tek_604\n" +
            "MCE Edits and Modifications Done Under GPL3";

#if DEBUG
        private String dLiquid = "0.7",
            dMono = "3",
            dSolid = "5",
            dXenon = "10",
            dMass = "3500",
            dOxidizer = "6",
            dEngine = "1.5";
#else
        // edited malkuth changed stats and got rid of most of the difficutly parts.. Still some parts exist for hard mode in settings.
        private String[] difficulties = new String[] { "Flight Testing", "Normal Flight Mode",};
#endif

        private void drawSettingsWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            settings.disablePlugin = GUILayout.Toggle(settings.disablePlugin, "Disable Plugin");

            GUILayout.Label("Kerbonaut insurance cost: ", styleValueGreenBold);
            settings.kerbonautCost = GUILayout.TextField(settings.kerbonautCost);
            settings.kerbonautCost = Regex.Replace(settings.kerbonautCost, @"[^0-9]", "");

#if DEBUG  
            GUILayout.Label ("Liquid Fuel: ", styleCaption);
            dLiquid = GUILayout.TextField (dLiquid);
            GUILayout.Label ("Mono Fuel: ", styleCaption);
            dMono = GUILayout.TextField (dMono);
            GUILayout.Label ("Solid Fuel: ", styleCaption);
            dSolid = GUILayout.TextField (dSolid);
            GUILayout.Label ("Xenon Fuel: ", styleCaption);
            dXenon = GUILayout.TextField (dXenon);
            GUILayout.Label ("Materials Construction: ", styleCaption);
            dMass = GUILayout.TextField (dMass);
            GUILayout.Label ("Engine Cost And Cooling: ", styleCaption);
            dEngine = GUILayout.TextField (dEngine);
            GUILayout.Label ("dOxizier: ", styleCaption);
            dOxidizer = GUILayout.TextField (dOxidizer);

            dLiquid = Regex.Replace (dLiquid, "[^0-9\\.]", "");
            dMono = Regex.Replace (dMono, "[^0-9\\.]", "");
            dSolid = Regex.Replace (dSolid, "[^0-9\\.]", "");
            dXenon = Regex.Replace (dXenon, "[^0-9\\.]", "");
            dMass = Regex.Replace (dMass, "[^0-9\\.]", "");
            dEngine = Regex.Replace (dEngine, "[^0-9\\.]", "");
            dOxidizer = Regex.Replace (dOxidizer, "[^0-9\\.]", "");
#else
            // Edited Malkuth add captions for info on how modes work
            GUILayout.Label("Chose Your Game Modes", styleValueGreenBold);
            settings.difficulty = GUILayout.SelectionGrid(settings.difficulty, difficulties, 2);
            GUILayout.Label("Flight Test Mode Is 3% Cost To Test Craft No Missions", styleValueGreenBold);
            GUILayout.Label("Launch Mode is Regular Gameplay Full Cost And Missions", styleValueGreenBold);
            //GUILayout.Label("HardCore Mode Is 40% Reduction in Mission Payouts", styleValueGreenBold);
            
#endif

            GUILayout.Space(30);

            GUILayout.Label(contributions, styleText);

           

            if (GUILayout.Button(rewindStrings[rewindCount], styleButtonWordWrap))
            {
                rewindCount++;
                if (rewindCount >= rewindStrings.Length)
                {
                    rewindCount = 0;
                    manager.rewind();
                }
            }

            if (GUILayout.Button(resetStrings[resetCount], styleButtonWordWrap))
            {
                resetCount++;
                if (resetCount >= resetStrings.Length)
                {
                    resetCount = 0;
                    manager.resetSpaceProgram();
                }
            }

            if (GUILayout.Button("Save and Close Settings", styleButton))
            {
                settingsWindow(false);

#if DEBUG
                Difficulty.init (double.Parse(dLiquid), double.Parse (dMono), double.Parse (dSolid), double.Parse (dXenon),
                                 double.Parse (dMass), double.Parse (dOxidizer), double.Parse (dEngine));
#else
                Difficulty.init(settings.difficulty);
#endif

                SettingsManager.Manager.saveSettings();
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}

