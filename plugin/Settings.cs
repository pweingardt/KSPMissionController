using System;

namespace MissionController
{
    public class Settings {
        public bool disablePlugin = false;

        public String kerbonautCost = "0";

        public bool changed = false;

        public int difficulty = 1;

        public int kerbonautCostAsInt {
            get { return int.Parse (kerbonautCost); }
        }
    }

    public class SettingsManager {

        private static SettingsManager manager = new SettingsManager();

        public static SettingsManager Manager { get { return manager; } }

        private Settings settings = new Settings();

        public Settings getSettings() {
            return manager.settings;
        }

        private Parser parser;

        private SettingsManager() {
            parser = new Parser();
            loadSettings ();
        }

        public void loadSettings() {
            try {
                settings = (Settings) parser.readFile ("settings.cfg");
            } catch {
                settings = new Settings();
            }
            settings.changed = false;
        }

        public void saveSettings() {
            settings.changed = false;
            parser.writeObject (settings, "settings.cfg");
        }

        public void saveSettingsIfChanged() {
            if (settings.changed) {
                saveSettings ();
            }
        }
    }
}

