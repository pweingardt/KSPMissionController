using System;
using UnityEngine;
using MissionController;
using System.Collections.Generic;
using System.Reflection;
using KSP.IO;

/// <summary>
/// Draws the GUI and handles the user input and events (like launch)
/// </summary>
namespace MissionController
{
    [KSPAddon(KSPAddon.Startup.EveryScene, true)]
    public partial class MissionController : MonoBehaviour
    {
        private AssemblyName assemblyName;
        private String versionCode;
        private DateTime buildDateTime;
        private String mainWindowTitle;

        public static string root = KSPUtil.ApplicationRootPath.Replace ("\\", "/");
        public static string pluginFolder = root + "GameData/MissionController/";
        public static string missionFolder = pluginFolder + "Plugins/PluginData/MissionController";
        private Texture2D menuIcon = null;

        private Manager manager {
            get {
                return Manager.instance;
            }
        }

        private Settings settings {
            get {
                return SettingsManager.Manager.getSettings();
            }
        }

        private List<MissionGoal> hiddenGoals = new List<MissionGoal> ();
    
        private Rect mainWindowPosition = new Rect (300, 70, 400, 700);
        private Rect testWindowPosition = new Rect (Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 150);
        private Rect settingsWindowPosition = new Rect (700, 70, 300, 250);
        private Rect packageWindowPosition = new Rect (50, 50, 1000, 700);

        private bool showMainWindow = false;
        private bool showTestVesselWindow = false;
        private bool showSettingsWindow = false;
        private bool showMissionPackageBrowser = false;

        private FileBrowser fileBrowser = null;
        private Mission currentMission = null;
        private MissionPackage currentPackage = null;

        private Vector2 scrollPosition = new Vector2 (0, 0);
        private GUIStyle styleCaption;
        private GUIStyle styleText;
        private GUIStyle styleValueGreen;
        private GUIStyle styleValueRed;
        private GUIStyle styleButton;
        private GUIStyle styleGreenButton, styleRedButton;
        private GUIStyle styleValueName;
        private GUIStyle styleWarning;
        private GUIStyle styleIcon;

        private EventFlags eventFlags = EventFlags.NONE;

        private void loadIcons () {
            if (menuIcon == null) {
                menuIcon = new Texture2D (30, 30, TextureFormat.ARGB32, false);
                menuIcon.LoadImage (KSP.IO.File.ReadAllBytes<MissionController> ("icon.png"));
            }
        }
        
        private void loadStyles ()
        {
            styleCaption = new GUIStyle (GUI.skin.label);
            styleCaption.normal.textColor = Color.green;
            styleCaption.fontStyle = FontStyle.Normal;
            styleCaption.alignment = TextAnchor.MiddleLeft;
            
            styleText = new GUIStyle (GUI.skin.label);
            styleText.normal.textColor = Color.white;
            styleText.fontStyle = FontStyle.Normal;
            styleCaption.alignment = TextAnchor.UpperLeft;
            
            styleValueGreen = new GUIStyle (GUI.skin.label);
            styleValueGreen.normal.textColor = Color.green;
            styleValueGreen.fontStyle = FontStyle.Normal;
            styleValueGreen.alignment = TextAnchor.MiddleRight;
            
            styleValueRed = new GUIStyle (GUI.skin.label);
            styleValueRed.normal.textColor = Color.red;
            styleValueRed.fontStyle = FontStyle.Normal;
            styleValueRed.alignment = TextAnchor.MiddleRight;

            styleButton = new GUIStyle (HighLogic.Skin.button);
            styleButton.normal.textColor = Color.white;
            styleButton.fontStyle = FontStyle.Bold;
            styleButton.alignment = TextAnchor.MiddleCenter;
            
            styleValueName = new GUIStyle (GUI.skin.label);
            styleValueName.normal.textColor = Color.white;
            styleValueName.fontStyle = FontStyle.Normal;
            styleValueName.alignment = TextAnchor.MiddleLeft;

            styleWarning = new GUIStyle (GUI.skin.label);
            styleWarning.normal.textColor = Color.red;
            styleWarning.fontStyle = FontStyle.Normal;
            styleWarning.alignment = TextAnchor.MiddleLeft;

            styleGreenButton = new GUIStyle (HighLogic.Skin.button);
            styleGreenButton.normal.textColor = Color.green;
            styleGreenButton.alignment = TextAnchor.MiddleCenter;
            styleGreenButton.wordWrap = true;

            styleRedButton = new GUIStyle (HighLogic.Skin.button);
            styleRedButton.normal.textColor = Color.red;
            styleRedButton.alignment = TextAnchor.MiddleCenter;
            styleRedButton.wordWrap = true;

            styleIcon = new GUIStyle ();
        }

        public void toggleWindow () {
            showMainWindow = !showMainWindow;
        }

        public void Awake () {
            DontDestroyOnLoad (this);

            GameEvents.onLaunch.Add (this.onLaunch);
            GameEvents.onVesselChange.Add (this.onVesselChange);
            GameEvents.onCrewKilled.Add (this.onCrewKilled);
            GameEvents.onVesselDestroy.Add(this.onVesselDestroy);
            GameEvents.onGameSceneLoadRequested.Add (this.onGameSceneLoadRequested);
            GameEvents.onCrash.Add (this.onCrash);
            GameEvents.onCollision.Add (this.onCollision);
            GameEvents.onPartCouple.Add (this.onPartCouple);

            assemblyName = Assembly.GetExecutingAssembly ().GetName ();
            versionCode = "" + assemblyName.Version.Major + "." + assemblyName.Version.Minor;
            buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
                TimeSpan.TicksPerDay * assemblyName.Version.Build + // days since 1 January 2000
                TimeSpan.TicksPerSecond * 2 * assemblyName.Version.Revision));

            mainWindowTitle = "Mission Controller " + 
                versionCode + " (" + buildDateTime.ToShortDateString () + ")";
        }

        private void Reset(GameScenes gameScenes) {
            GameEvents.onLaunch.Remove (this.onLaunch);
            GameEvents.onVesselChange.Remove (this.onVesselChange);
            GameEvents.onCrewKilled.Remove (this.onCrewKilled);
            GameEvents.onVesselDestroy.Remove(this.onVesselDestroy);
            GameEvents.onGameSceneLoadRequested.Remove (this.onGameSceneLoadRequested);
            GameEvents.onCrash.Remove (this.onCrash);
            GameEvents.onCollision.Remove (this.onCollision);
        }

        /// <summary>
        /// Returns the active vessel if there is one, null otherwise
        /// </summary>
        /// <value>The active vessel.</value>
        private Vessel activeVessel {
            get {
                // We need this try-catch-block, because FlightGlobals.ActiveVessel might throw
                // an exception
                try {
                    if(HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null) {
                        return FlightGlobals.ActiveVessel;
                    } else {
                        return null;
                    }
                } catch {
                    return null;
                }
            }
        }

        public void Update() {
            try {
                Debug.LogError ("Vessel size: " + FlightGlobals.Vessels.Count);
            } catch {
                Debug.LogError ("Exception !");
            }
        }

        public void OnGUI () {
            if (!HighLogic.LoadedSceneIsFlight && !HighLogic.LoadedSceneIsEditor
                    && !HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER)) {
                return;
            }

            loadIcons ();
            loadStyles ();

            GUI.skin = HighLogic.Skin;

            if (GUI.Button (new Rect (Screen.width / 6 - 34, Screen.height - 34, 32, 32), menuIcon, styleIcon)) {
                toggleWindow ();
            }
            
            if (showMainWindow) {
                mainWindowPosition = GUILayout.Window (98765, mainWindowPosition, drawMainWindow, mainWindowTitle);
            }
            
            if (showTestVesselWindow) {
                testWindowPosition = GUILayout.Window (98764, testWindowPosition, drawTestWindow, "Are you sure?");
            }

            if (showSettingsWindow) {
                settingsWindowPosition = GUILayout.Window (98763, settingsWindowPosition, drawSettingsWindow, "Settings");
            }

            if (showMissionPackageBrowser) {
                packageWindowPosition = GUILayout.Window(98762, packageWindowPosition, drawPackageWindow, currentPackage.name);
            }
            
            if (fileBrowser != null) {
                GUI.skin = HighLogic.Skin;
                GUIStyle list = GUI.skin.FindStyle ("List Item");
                list.normal.textColor = XKCDColors.DarkYellow; 
                list.contentOffset = new Vector2 (1, 0);
                list.fontSize = 20;
                list.alignment = TextAnchor.MiddleLeft;
                
                fileBrowser.OnGUI ();
                
                // Reset Skin
                list.normal.textColor = new Color (0.739f, 0.739f, 0.739f);
                list.contentOffset = new Vector2 (1, 42.4f);
                list.fontSize = 10;
            }
        }

        /// <summary>
        /// Draws the window, that asks the user if he really wants to mark the active vessel as test vessel.
        /// </summary>
        /// <param name="id">Identifier.</param>
        private void drawTestWindow (int id) {
            GUI.skin = HighLogic.Skin;
            
            GUILayout.BeginVertical ();
            GUILayout.Label ("Do you want to mark this vessel as test vessel? Test vessels *CAN NOT* finish missions, but cost only half the price!", styleText);
            
            GUILayout.BeginHorizontal ();
            if (GUILayout.Button ("Yes")) {
                showTestVesselWindow = false;
//                isTestVessel = true;
            }
            if (GUILayout.Button ("NO!!!")) {
                showTestVesselWindow = false;
            }
            GUILayout.EndHorizontal ();
            GUILayout.EndVertical ();
            GUI.DragWindow ();
        }

        /// <summary>
        /// Draws the main mission window.
        /// Do not use currentMission.isDone or missionGoal.isDone(), use status instead!!!
        /// </summary>
        /// <param name="id">Identifier.</param>
        private void drawMainWindow (int id) {
            Status status = calculateStatus (currentMission, true);

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical ();
            
            scrollPosition = GUILayout.BeginScrollView (scrollPosition);
            GUILayout.BeginVertical ();
            
            GUILayout.BeginHorizontal ();
            GUILayout.Label ("Current budget: ", styleValueName);
            GUILayout.Label (manager.budget + CurrencySuffix, (manager.budget < 0 ? styleValueRed : styleValueGreen));
            GUILayout.EndHorizontal ();

            // Show only when the loaded scene is an editor or a vessel is available and its situation is PRELAUNCH
            if (HighLogic.LoadedSceneIsEditor || status.onLaunchPad) {
                VesselResources res = vesselResources;
                showCostValue("Construction costs:", res.construction, styleValueGreen);
                showCostValue("Liquid fuel costs:", res.liquid (), styleValueGreen);
                showCostValue("Oxidizer costs:", res.oxidizer (), styleValueGreen);
                showCostValue("Monopropellant costs:", res.mono (), styleValueGreen);
                showCostValue("Solid fuel costs:", res.solid (), styleValueGreen);
                showCostValue("Xenon gas costs:", res.xenon (), styleValueGreen);
                showCostValue("Other resource costs:", res.other (), styleValueGreen);
                showCostValue("Sum:", res.sum(), (res.sum () > manager.budget ? styleValueRed : styleValueGreen));
            }

            if (currentMission != null) {
                drawMission (currentMission, status);
            } else {
                if (GUILayout.Button ("Configure")) {
                    showSettingsWindow = !showSettingsWindow;
                    resetCount = 0;
                }
            }
            
            GUILayout.EndVertical ();
            GUILayout.EndScrollView ();

            if (GUILayout.Button ("Select mission package")) {
                createFileBrowser ("Select mission from package", selectMissionPackage);
            }

            if(currentPackage != null) {
                if (GUILayout.Button ("Open browser window")) {
                    showMissionPackageBrowser = true;
                }
            }

            if(currentMission != null) {
                if (GUILayout.Button ("Deselect mission")) {
                    currentMission = null;
                }
            }

            if (status.missionIsFinishable) {
                if (GUILayout.Button ("Finish the mission!")) {
                    manager.finishMission (currentMission, activeVessel, status.events);
                }
            } else {
                if (status.recyclable) {
                    VesselResources res = vesselResources;
                    showCostValue("Recyclable value: ", res.recyclable(), styleCaption);
                    if (GUILayout.Button ("Recycle this vessel!")) {
                        manager.recycleVessel (activeVessel, (int)(res.recyclable()));
                    }
                } else {
                    if (status.recycledVessel) {
                        GUILayout.Label ("This is a recycled vessel. You can't finish any missions with this vessel!", styleWarning);
                    }
                }
            }

            GUILayout.EndVertical ();
            GUI.DragWindow ();
        }

        /// <summary>
        /// Selects the mission in the file
        /// </summary>
        /// <param name="file">File.</param>
        private void selectMissionPackage (String file) {
            destroyFileBrowser ();
            
            if (file == null) {
                return;
            }

            currentPackage = manager.loadMissionPackage (file);
            currentPreviewMission = null;
            showMissionPackageBrowser = (currentPackage != null);
            currentSort = (currentPackage.ownOrder ? SortBy.PACKAGE_ORDER : SortBy.NAME);
//            hiddenGoals = new List<MissionGoal> ();
        }

        /// <summary>
        /// Draws the mission parameters
        /// </summary>
        private void drawMission (Mission mission, Status s) {
            GUILayout.Label ("Mission: ", styleCaption);
            GUILayout.Label (mission.name, styleText);
            GUILayout.Label ("Description: ", styleCaption);
            GUILayout.Label (mission.description, styleText);
            
            GUILayout.BeginHorizontal ();
            GUILayout.Label ("Reward: ", styleValueName);
            GUILayout.Label (mission.reward + CurrencySuffix, styleValueGreen);
            GUILayout.EndHorizontal ();

            if (mission.repeatable) {
                GUILayout.Label ("Mission is repeatable!", styleCaption);
            }

            if (s.requiresAnotherMission) {
                GUILayout.Label ("This mission requires the mission \"" + mission.requiresMission + "\". Finish mission \"" + 
                                 mission.requiresMission + "\" first, before you proceed.", styleWarning);
            }

            if (s.missionAlreadyFinished) {
                GUILayout.Label ("Mission already finished!", styleCaption);
            } 

            drawMissionGoals (mission, s);

            if(s.missionIsFinishable) {
                GUILayout.Label("All goals accomplished. You can finish the mission now!", styleCaption);
            }
        }

        /// <summary>
        /// Draws the mission goals
        /// </summary>
        /// <param name="mission">Mission.</param>
        private void drawMissionGoals (Mission mission, Status s) {
            int index = 1;

            foreach (MissionGoal c in mission.goals) {
                if (hiddenGoals.Contains(c)) {
                    index++;
                    continue;
                }

                GUILayout.Label ((index++) + ". Mission goal: " + c.getType () + (c.optional ? " (optional)" : ""), styleCaption);
                
                if (c.description.Length != 0) {
                    GUILayout.Label ("Description: ", styleCaption);
                    GUILayout.Label (c.description, styleText);
                }
                
                if (c.nonPermanent && c.reward != 0) {
                    GUILayout.BeginHorizontal ();
                    GUILayout.Label ("Reward:", styleValueName);
                    GUILayout.Label (c.reward + CurrencySuffix, styleValueGreen);
                    GUILayout.EndHorizontal ();
                }

                List<Value> values = c.getValues (activeVessel, s.events);

                foreach (Value v in values) {
                    GUILayout.BeginHorizontal ();
                    GUILayout.Label (v.name, styleValueName);
                    if(v.currentlyIs.Length == 0) {
                        GUILayout.Label(v.shouldBe, styleValueGreen);
                    } else {
                        GUILayout.Label (v.shouldBe + " : " + v.currentlyIs, (v.done ? styleValueGreen : styleValueRed));
                    }
                    GUILayout.EndHorizontal ();
                }

                if (activeVessel != null) {
                    if (s.finishableGoals.ContainsKey(c.id) && s.finishableGoals[c.id]) {
                        if (GUILayout.Button ("Hide finished goal")) {
                            hiddenGoals.Add (c);
                        }
                    } else {
                        if (c.optional) {
                            if (GUILayout.Button ("Hide optional goal")) {
                                hiddenGoals.Add (c);
                            }
                        }
                    }
                }
            }
        }
        
        // Create the file browser
        private void createFileBrowser (string title, FileBrowser.FinishedCallback callback) {
            fileBrowser = new FileBrowser (new Rect (Screen.width / 2, 100, 350, 500), title, callback, true);
            fileBrowser.BrowserType = FileBrowserType.File;
            fileBrowser.CurrentDirectory = missionFolder;
            fileBrowser.disallowDirectoryChange = true;
            fileBrowser.SelectionPattern = "*.mpkg";

            if (EditorLogic.fetch != null) {
                EditorLogic.fetch.Lock (true, true, true);
            }
        }

        private void destroyFileBrowser() {
            fileBrowser = null;

            if (EditorLogic.fetch != null) {
                EditorLogic.fetch.Unlock ();
            }
        }



        private void showCostValue(String name, double value, GUIStyle style) {
            showStringValue (name, String.Format ("{0:0.##}{1}", value, CurrencySuffix), style);
        }

        private void showDoubleValue(String name, double value, GUIStyle style) {
            showStringValue (name, String.Format ("{0:0.##}", value), style);
        }

        private void showIntValue(String name, int value, GUIStyle style) {
            showStringValue (name, String.Format ("{0}", value), style);
        }

        private void showStringValue(String name, String value, GUIStyle style) {
            GUILayout.BeginHorizontal ();
            GUILayout.Label (name, styleValueName);
            GUILayout.Label (value, style);
            GUILayout.EndHorizontal ();
        }

        private const String CurrencySuffix = " ₭";
    }


    /// <summary>
    /// This code is necessary, so the user doesn't have to add a part to his vessel 
    /// </summary>
//    public class MissionControllerTest : KSP.Testing.UnitTest
//    {
//        public MissionControllerTest () : base()
//        {
//            I.AddI<MissionController> ("MISSION_CONTROLLER");
//        }
//    }
//
//    static class I
//    {
//        private static GameObject _gameObject;
//    
//        public static T AddI<T> (string name) where T : Component
//        {
//            if (_gameObject == null) {
//                _gameObject = new GameObject (name, typeof(T));
//                GameObject.DontDestroyOnLoad (_gameObject);
//            
//                return _gameObject.GetComponent<T> ();
//            } else {
//                if (_gameObject.GetComponent<T> () != null)
//                    return _gameObject.GetComponent<T> ();
//                else
//                    return _gameObject.AddComponent<T> ();
//            }
//        }
//    }
}

