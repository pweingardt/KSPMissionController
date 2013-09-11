using System;

namespace MissionController
{
    public class Settings 
    {
        public int KerbalsHired;

        public bool disablePlugin = false;

        public String kerbonautCost = "0";

        public bool changed = false;

        public int difficulty = 1;

        public int kerbonautCostAsInt 
        {
            get 
            { 
                return int.Parse (kerbonautCost); 
            }
        }
        public string HireKerbalNautCost = "5000";
        public int KerbalNautCost
        {
            get
            {
                return int.Parse(HireKerbalNautCost);
            }
        }
        

    }

    public class SettingsManager 
    {

        private static SettingsManager manager = new SettingsManager();

        public static SettingsManager Manager 
        { 
            get
            { return manager; } 
        }

        private Settings settings = new Settings();

        public Settings getSettings()
        {
            return manager.settings;
        }

        private Parser parser;

        private SettingsManager()
        {
            parser = new Parser();
            loadSettings ();
        }

        public void loadSettings() 
        {
            try 
            {
                settings = (Settings) parser.readFile ("settings.cfg");
            } 
            catch 
            {
                settings = new Settings();
            }
            settings.changed = false;
            Difficulty.init (settings.difficulty);
        }

        public void saveSettings()
        {
            settings.changed = false;
            parser.writeObject (settings,"settings.cfg");
        }

    }
}

