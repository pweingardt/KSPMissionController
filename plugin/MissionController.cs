using System;
using UnityEngine;
using MissionController;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using KSP.IO;

/// <summary>
/// Draws the GUI and handles the user input and events (like launch)
/// </summary>
namespace MissionController
{
//    [KSPAddon(KSPAddon.Startup.EveryScene, true)]
    public partial class MissionController : MonoBehaviour
    {
        public bool recycled = false;
        private bool drawLandingArea = false;

        private AssemblyName assemblyName;
        private String versionCode;
        private DateTime buildDateTime;
        private String mainWindowTitle;

        public static string root = KSPUtil.ApplicationRootPath.Replace ("\\", "/");
        public static string pluginFolder = root + "GameData/MissionController/";
        public static string missionFolder = pluginFolder + "Plugins/PluginData/MissionController";

        private WWW wwwIconFinished = new WWW("file://" + pluginFolder + "icons/flightavailible.png");
        private WWW wwwIconMenu = new WWW("file://" + pluginFolder + "icons/FlightSceneButtonT4.png");
        private WWW wwwIconProbe = new WWW("file://" + pluginFolder + "icons/sputnikmk2.png");
        private WWW wwwIconImpactor = new WWW("file://" + pluginFolder + "icons/impactormk2.png");
        private WWW wwwIconLander = new WWW("file://" + pluginFolder + "icons/landermk2.png");
        private WWW wwwIconOrbit = new WWW("file://" + pluginFolder + "icons/launchmk2.png");
        private WWW wwwIconDocking = new WWW("file://" + pluginFolder + "icons/rendezvousmk2.png");
        private WWW wwwIconSatellite = new WWW("file://" + pluginFolder + "icons/Stellitemk2.png");
        private WWW wwwIconEVA = new WWW("file://" + pluginFolder + "icons/EVAing.png");
        private WWW wwwIconClock = new WWW("file://" + pluginFolder + "icons/clockmk2.png");
        private WWW wwwIconManned = new WWW("file://" + pluginFolder + "icons/kerbedmk2.png");
        private WWW wwwIconAviation = new WWW("file://" + pluginFolder + "icons/planemk2.png");
        private WWW wwwIconScience = new WWW("file://" + pluginFolder + "icons/sciencemk2.png");
        private WWW wwwIconCommunication = new WWW("file://" + pluginFolder + "icons/sensormk2.png");
        private WWW wwwIconRover = new WWW("file://" + pluginFolder + "icons/rovermk2.png");

        private Texture2D iconFinished = null;
        private Texture2D iconMenu = null;
        private Texture2D iconTypeProbe = null;
        private Texture2D iconTypeImpactor = null;
        private Texture2D iconTypeLander = null;
        private Texture2D iconTypeOrbit = null;
        private Texture2D iconTypeDocking = null;
        private Texture2D iconTypeSatellite = null;
        private Texture2D iconTypeEVA = null;
        private Texture2D iconTypeClock = null;
        private Texture2D iconTypeManned = null;
        private Texture2D iconTypeAviation = null;
        private Texture2D iconTypeScience = null;
        private Texture2D iconTypeCommunication = null;
        private Texture2D iconTypeRover = null;

        /// <summary>
        /// True if the UI should be hidden (F2 button)
        /// </summary>
        private bool hideAll = false;

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
        private Rect settingsWindowPosition = new Rect (700, 70, 300, 250);
        private Rect packageWindowPosition = new Rect (50, 50, 1000, 700);

        private bool showMainWindow = false;
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
        private GUIStyle styleButtonWordWrap;

        private EventFlags eventFlags = EventFlags.NONE;

        private double lastPassiveReward = 0.0;

        private void loadIcons () {
            if (iconMenu == null) {
                iconMenu = new Texture2D (35, 50, TextureFormat.ARGB32, false);
                iconFinished = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeProbe = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeImpactor = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeLander = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeOrbit = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeDocking = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeSatellite = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeAviation = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeClock = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeCommunication = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeEVA = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeManned = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeRover = new Texture2D (0, 0, TextureFormat.ARGB32, false);
                iconTypeScience = new Texture2D (0, 0, TextureFormat.ARGB32, false);

                wwwIconFinished.LoadImageIntoTexture(iconFinished);
                wwwIconMenu.LoadImageIntoTexture(iconMenu);
                wwwIconDocking.LoadImageIntoTexture(iconTypeDocking);
                wwwIconOrbit.LoadImageIntoTexture(iconTypeOrbit);
                wwwIconImpactor.LoadImageIntoTexture(iconTypeImpactor);
                wwwIconSatellite.LoadImageIntoTexture(iconTypeSatellite);
                wwwIconLander.LoadImageIntoTexture(iconTypeLander);
                wwwIconProbe.LoadImageIntoTexture(iconTypeProbe);
                wwwIconAviation.LoadImageIntoTexture(iconTypeAviation);
                wwwIconClock.LoadImageIntoTexture(iconTypeClock);
                wwwIconEVA.LoadImageIntoTexture(iconTypeEVA);
                wwwIconManned.LoadImageIntoTexture(iconTypeManned);
                wwwIconRover.LoadImageIntoTexture(iconTypeRover);
                wwwIconScience.LoadImageIntoTexture(iconTypeScience);
                wwwIconCommunication.LoadImageIntoTexture(iconTypeCommunication);

                iconDictionary.Add (Mission.Category.DOCKING, iconTypeDocking);
                iconDictionary.Add (Mission.Category.ORBIT, iconTypeOrbit);
                iconDictionary.Add (Mission.Category.PROBE, iconTypeProbe);
                iconDictionary.Add (Mission.Category.LANDING, iconTypeLander);
                iconDictionary.Add (Mission.Category.SATELLITE, iconTypeSatellite);
                iconDictionary.Add (Mission.Category.IMPACT, iconTypeImpactor);
                iconDictionary.Add (Mission.Category.AVIATION, iconTypeAviation);
                iconDictionary.Add (Mission.Category.COMMUNICATION, iconTypeCommunication);
                iconDictionary.Add (Mission.Category.EVA, iconTypeEVA);
                iconDictionary.Add (Mission.Category.MANNED, iconTypeManned);
                iconDictionary.Add (Mission.Category.ROVER, iconTypeRover);
                iconDictionary.Add (Mission.Category.SCIENCE, iconTypeScience);
                iconDictionary.Add (Mission.Category.TIME, iconTypeClock);
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

            styleButtonWordWrap = new GUIStyle (HighLogic.Skin.button);
            styleButtonWordWrap.normal.textColor = Color.white;
            styleButtonWordWrap.fontStyle = FontStyle.Bold;
            styleButtonWordWrap.alignment = TextAnchor.MiddleCenter;
            styleButtonWordWrap.wordWrap = true;
            
            styleValueName = new GUIStyle (GUI.skin.label);
            styleValueName.normal.textColor = Color.white;
            styleValueName.fontStyle = FontStyle.Normal;
            styleValueName.alignment = TextAnchor.MiddleLeft;

            styleWarning = new GUIStyle (GUI.skin.label);
            styleWarning.normal.textColor = Color.red;
            styleWarning.fontStyle = FontStyle.Bold;
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

            loadIcons ();

            Debug.LogError("Using factors: " + String.Join(", ", Difficulty.Factors.Select(p => p.ToString()).ToArray()));
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

        /// <summary>
        /// We check for passive missions and client controlled missions every day.
        /// </summary>
        public void Update() {
            if (Input.GetKeyDown (KeyCode.F2)) {
                hideAll = !hideAll;
            }

            try {
                if(!isValidScene()) {
                    return;
                }

                // Every game day we check the passive missions and reward the player
                if((lastPassiveReward == 0.0 || Planetarium.GetUniversalTime() - lastPassiveReward >= 60.0 * 60.0 * 24.0) 
                        && Planetarium.GetUniversalTime() != 0.0) {

                    lastPassiveReward = Planetarium.GetUniversalTime();

                    // we better make sure that lastPassiveReward is non zero:
                    if(lastPassiveReward == 0.0) {
                        lastPassiveReward = 1.0;
                    }

                    // Now we check the all passive missions, that are currently active
                    List<MissionStatus> passives = manager.getActivePassiveMissions();
                    double time = Planetarium.GetUniversalTime();
                    foreach(MissionStatus s in passives) {
                        int daysDiff = (int)((time - s.lastPassiveRewardTime) / (60.0 * 60.0 * 24.0));
                        // If the last time we gave reward is longer than one day ago,
                        // we reward the player now
                        if(daysDiff > 0) {
                            // Now we check if the vessel is still active. If it is not, we will punish the player
                            // and remove the old mission status

                            ProtoVessel pv = HighLogic.CurrentGame.flightState.protoVessels
                                .Find(p => p.vesselID.ToString().Equals(s.vesselGuid) && p.vesselType != VesselType.Debris);

                            if(pv != null) {
                                s.lastPassiveRewardTime = time;
                                manager.reward(daysDiff * s.passiveReward);
                            } else {
                                manager.removeMission(s);
                                manager.costs(s.punishment);
                            }
                        }
                    }

                    // After that we check for client controlled missions. If those vessels get destroyed, we will punish the player
                    passives = manager.getClientControlledMissions();
                    foreach(MissionStatus s in passives) {
                        ProtoVessel pv = HighLogic.CurrentGame.flightState.protoVessels
                            .Find(p => p.vesselType != VesselType.Debris && p.vesselID.ToString().Equals(s.vesselGuid));

                        if(pv == null) {
                            manager.removeMission(s);
                            manager.costs(s.punishment);
                        }
                    }
                }
            } catch {
            }
        }

        public void OnGUI () {
            if (!isValidScene()) {
                return;
            }

            if(drawLandingArea) {
                CelestialBody kerbin = FlightGlobals.Bodies.Find (b => b.bodyName.Equals("Kerbin"));
                if (kerbin != null) {
//                    GLUtils.drawLandingArea (kerbin, 80, 90, -170.0, 170.0, new Color(1.0f, 0.0f, 0.0f, 0.5f));
                }
            }

            loadIcons ();
            loadStyles ();

            GUI.skin = HighLogic.Skin;

            // We need to calculate the status in case the windows are not visible
            calculateStatus (currentMission, true, activeVessel);

            if (hideAll) {
                return;
            }

            if (GUI.Button (new Rect (Screen.width / 6 - 44, Screen.height - 38, 45, 40), iconMenu, styleIcon)) {
                toggleWindow ();
            }
            
            if (showMainWindow) {
                mainWindowPosition = GUILayout.Window (98765, mainWindowPosition, drawMainWindow, mainWindowTitle);
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
        /// Draws the main mission window.
        /// Do not use currentMission.isDone or missionGoal.isDone(), use status instead!!!
        /// </summary>
        /// <param name="id">Identifier.</param>
        private void drawMainWindow (int id) {
            Status status = calculateStatus (currentMission, true, activeVessel);

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
                showCostValue("Liquid fuel costs:", res.liquid (), styleValueGreen);
                showCostValue("Oxidizer costs:", res.oxidizer (), styleValueGreen);
                showCostValue("Monopropellant costs:", res.mono (), styleValueGreen);
                showCostValue("Solid fuel costs:", res.solid (), styleValueGreen);
                showCostValue("Xenon gas costs:", res.xenon (), styleValueGreen);
                showCostValue("Liquid engines: ", res.engine (), styleValueGreen);
                showCostValue("Crew insurance: ", res.crew (), styleValueGreen);
                showCostValue("Other resource costs:", res.materials (), styleValueGreen);
                showCostValue("Sum:", res.sum(), (res.sum () > manager.budget ? styleValueRed : styleValueGreen));
            }

            if (status.isClientControlled) {
                MissionStatus s = manager.getClientControlledMission (activeVessel);
                GUILayout.Label ("This vessel is controlled by a client. Do not destroy this vessel! Fine: " + s.punishment + CurrencySuffix, styleWarning);
                GUILayout.Label ("End of life in " + MathTools.formatTime(s.endOfLife - Planetarium.GetUniversalTime()));
            } else if (status.isOnPassiveMission) {
                MissionStatus s = manager.getPassiveMission (activeVessel);
                GUILayout.Label ("This vessel is involved in a passive mission. Do not destroy this vessel! Fine: " + s.punishment + CurrencySuffix, styleWarning);
                GUILayout.Label ("End of life in " + MathTools.formatTime(s.endOfLife - Planetarium.GetUniversalTime()));
            }

            GUILayout.Space (30);

            if (currentMission != null) {
                drawMission (currentMission, status);
            } else {
                drawPassiveMissions (manager.getActivePassiveMissions());

                if (GUILayout.Button ("Configure")) {
                    settingsWindow (!showSettingsWindow);
                    resetCount = 0;
                }
            }

//            if (GUILayout.Button ("Draw landing area!", styleButton)) {
//                drawLandingArea = !drawLandingArea;
//            }
            
            GUILayout.EndVertical ();
            GUILayout.EndScrollView ();

            if (GUILayout.Button ("Select mission package")) {
                createFileBrowser ("Select mission from package", selectMissionPackage);
            }

            if(currentPackage != null) {
                if (GUILayout.Button ("Open browser window")) {
                    packageWindow (true);
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
                    hiddenGoals = new List<MissionGoal> ();
                    currentMission = null;
                }
            } else {
                if (status.recyclable) {
                    VesselResources res = vesselResources;
                    showCostValue("Recyclable value: ", res.recyclable(activeVessel.Landed), styleCaption);
                    if (GUILayout.Button ("Recycle and end flight!")) {
                        manager.recycleVessel (activeVessel, res.recyclable(activeVessel.Landed));
                        FlightDriver.TerminateCurrentFlight ();
                        FlightResultsDialog.showExitControls = true;
                        FlightResultsDialog.Display ("Vessel has been recycled!");
                        recycled = true;
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
            if (currentPackage != null) {
                packageWindow (true);
            }
            currentSort = (currentPackage.ownOrder ? SortBy.PACKAGE_ORDER : SortBy.NAME);
        }

        /// <summary>
        /// Draws the mission parameters
        /// </summary>
        private void drawMission (Mission mission, Status s) {
            if (s.missionAlreadyFinished) {
                GUILayout.Label ("Mission already finished!", styleWarning);
            }

            GUILayout.Label ("Mission: ", styleCaption);
            GUILayout.Label (mission.name, styleText);
            GUILayout.Label ("Description: ", styleCaption);
            GUILayout.Label (mission.description, styleText);
            
            GUILayout.BeginHorizontal ();
            GUILayout.Label ("Reward: ", styleValueName);
            GUILayout.Label (mission.reward + CurrencySuffix, styleValueGreen);
            GUILayout.EndHorizontal ();

            if (mission.passiveMission) {
                GUILayout.BeginHorizontal ();
                GUILayout.Label ("Reward every day: ", styleValueName);
                GUILayout.Label (mission.passiveReward + CurrencySuffix, styleValueGreen);
                GUILayout.EndHorizontal ();
            }

            if (mission.lifetime != 0.0) {
                GUILayout.BeginHorizontal ();
                GUILayout.Label ("Lifetime: ", styleValueName);
                GUILayout.Label (MathTools.formatTime(mission.lifetime), styleValueGreen);
                GUILayout.EndHorizontal ();
            }

            if (mission.clientControlled) {
                GUILayout.Label ("The client will get the control over this vessel once the mission is finished!", styleWarning);
            }

            if (mission.repeatable) {
                GUILayout.Label ("Mission is repeatable!", styleCaption);
            }

            if (s.requiresAnotherMission) {
                GUILayout.Label ("This mission requires the mission \"" + mission.requiresMission + "\". Finish mission \"" + 
                                 mission.requiresMission + "\" first, before you proceed.", styleWarning);
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

                if (c is SubMissionGoal) {
                    GUILayout.Label ((index++) + ". Mission goal: " + (c.optional ? " (optional)" : ""), styleCaption);
                } else {
                    GUILayout.Label ((index++) + ". Mission goal: " + c.getType () + (c.optional ? " (optional)" : ""), styleCaption);
                }
                
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

        /// <summary>
        /// Draws the currently active passive missions
        /// </summary>
        /// <param name="missions">Missions.</param>
        private void drawPassiveMissions(List<MissionStatus> missions) {
            if (missions.Count > 0) {
                GUILayout.BeginHorizontal ();
                GUILayout.Label ("Next payment in", styleValueName);
                GUILayout.Label (MathTools.formatTime(60.0 * 60.0 * 24.0 - (Planetarium.GetUniversalTime() - lastPassiveReward)), styleValueGreen);
                GUILayout.EndHorizontal ();

                int total = 0;
                foreach (MissionStatus m in missions) {
                    GUILayout.BeginHorizontal ();
                    GUILayout.Label (m.missionName, styleValueName);
                    GUILayout.Label (m.passiveReward + CurrencySuffix, styleValueGreen);
                    GUILayout.EndHorizontal ();
                    total += m.passiveReward;
                }

                GUILayout.Space (20);

                GUILayout.BeginHorizontal ();
                GUILayout.Label ("Total", styleValueName);
                GUILayout.Label (total + CurrencySuffix, styleValueGreen);
                GUILayout.EndHorizontal ();
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

        /// <summary>
        /// Sets the visibility of the settings window
        /// </summary>
        /// <param name="visibility">If set to <c>true</c> visibility.</param>
        private void settingsWindow(bool visibility) {
            showSettingsWindow = visibility;
            lockOrUnlockEditor (visibility);
        }

        /// <summary>
        /// Sets the visibility of the package browser
        /// </summary>
        /// <param name="visibility">If set to <c>true</c> visibility.</param>
        private void packageWindow(bool visibility) {
            showMissionPackageBrowser = visibility;
            lockOrUnlockEditor (visibility);
        }

        /// <summary>
        /// Locks or unlocks the editor, if it is available
        /// </summary>
        /// <param name="visiblity">If set to <c>true</c> visiblity.</param>
        private void lockOrUnlockEditor(bool visiblity) {
            if (EditorLogic.fetch != null) {
                if (visiblity) {
                    EditorLogic.fetch.Lock (true, true, true);
                } else {
                    EditorLogic.fetch.Unlock ();
                }
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

        private bool isValidScene() {
            if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor
                || HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER)) {
                return true;
            }
            return false;
        }

        private const String CurrencySuffix = " ₭";
    }


    /// <summary>
    /// This code is necessary, so the user doesn't have to add a part to his vessel 
    /// </summary>
    public class MissionControllerTest : KSP.Testing.UnitTest
    {
        public MissionControllerTest () : base()
        {
            I.AddI<MissionController> ("MISSION_CONTROLLER");
        }
    }

    static class I
    {
        private static GameObject _gameObject;
    
        public static T AddI<T> (string name) where T : Component
        {
            if (_gameObject == null) {
                _gameObject = new GameObject (name, typeof(T));
                GameObject.DontDestroyOnLoad (_gameObject);
            
                return _gameObject.GetComponent<T> ();
            } else {
                if (_gameObject.GetComponent<T> () != null)
                    return _gameObject.GetComponent<T> ();
                else
                    return _gameObject.AddComponent<T> ();
            }
        }
    }
}

