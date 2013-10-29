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
    [KSPAddonFixed(KSPAddon.Startup.MainMenu, true, typeof(MissionController))]
    
    public partial class MissionController : MonoBehaviour
    {
        public bool recycled = false;
        private bool drawLandingArea = false;
        private bool FileBrowserBool = false;
        private bool packageWindowtoggle = false;
        private bool currentMissiontoggle = false;
        private bool finishmissiontoggle = true;
        private bool ExpandCost = false;
        private bool ShowMissionGoals = true;

        private AssemblyName assemblyName;
        private String versionCode;
        private DateTime buildDateTime;
        private String mainWindowTitle;

        public static string root = KSPUtil.ApplicationRootPath.Replace("\\", "/");
        public static string pluginFolder = root + "GameData/MissionController/";
        public static string missionFolder = pluginFolder + "Plugins/PluginData/MissionController";

        private WWW wwwIconFinished = new WWW("file://" + pluginFolder + "icons/flightavailible.png");
        private WWW wwwIconMenu = new WWW("file://" + pluginFolder + "icons/FlightSceneButtonT4.png");
        private WWW wwwIconProbe = new WWW("file://" + pluginFolder + "icons/sputnikmk2.png");
        private WWW wwwIconImpactor = new WWW("file://" + pluginFolder + "icons/impactormk2.png");
        private WWW wwwIconLander = new WWW("file://" + pluginFolder + "icons/landermk2.png");
        private WWW wwwIconOrbit = new WWW("file://" + pluginFolder + "icons/launchmk2.png");
        private WWW wwwIconDocking = new WWW("file://" + pluginFolder + "icons/docking.png");
        private WWW wwwIconSatellite = new WWW("file://" + pluginFolder + "icons/Stellitemk2.png");
        private WWW wwwIconEVA = new WWW("file://" + pluginFolder + "icons/EVAing.png");
        private WWW wwwIconClock = new WWW("file://" + pluginFolder + "icons/clockmk2.png");
        private WWW wwwIconManned = new WWW("file://" + pluginFolder + "icons/kerbedmk2.png");
        private WWW wwwIconAviation = new WWW("file://" + pluginFolder + "icons/planemk2.png");
        private WWW wwwIconScience = new WWW("file://" + pluginFolder + "icons/sciencemk2.png");
        private WWW wwwIconCommunication = new WWW("file://" + pluginFolder + "icons/sensormk2.png");
        private WWW wwwIconRover = new WWW("file://" + pluginFolder + "icons/rovermk2.png");
        private WWW wwwIconRepair = new WWW("file://" + pluginFolder + "icons/repair.png");

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
        private Texture2D iconTypeRepair = null;

        /// <summary>
        /// True if the UI should be hidden (F2 button)
        /// </summary>
        private bool hideAll = false;

        private Manager manager
        {
            get
            {
                return Manager.instance;
            }
        }

        private Settings settings
        {
            get
            {
                return SettingsManager.Manager.getSettings();
            }
        }

        private List<MissionGoal> hiddenGoals = new List<MissionGoal>();

        public Rect mainWindowPosition;
        private Rect settingsWindowPosition;
        private Rect packageWindowPosition;
        private Rect financeWindowPosition;
        private Rect researchtreewinpostion;


        private bool showMainWindow = false;
        private bool showSettingsWindow = false;
        private bool showMissionPackageBrowser = false;
        private bool showFinanceWindow = false;
        private bool showRecycleWindow = false;
        private bool showResearchTreeWindow = false;
        public bool showRandomWindow = false;

        public string recycledName = "";
        public string recycledDesc = "";
        public int recycledCost = 0;
        public int recycledCrewCost = 0;

        private FileBrowser fileBrowser = null;
        private Mission currentMission = null;
        private MissionPackage currentPackage = null;

        private Vector2 scrollPosition = new Vector2(0, 0);
        private GUIStyle styleCaption;
        private GUIStyle styleText;
        private GUIStyle styleValueGreen;
        private GUIStyle styleValueGreenBold;
        private GUIStyle styleValueYellow;
        private GUIStyle styleValueRed;
        private GUIStyle styleValueRedBold;
        private GUIStyle styleButton;
        private GUIStyle styleButtonYellow;
        private GUIStyle styleGreenButton, styleRedButton;
        private GUIStyle styleValueName;
        private GUIStyle styleWarning;
        private GUIStyle styleIcon;
        private GUIStyle styleButtonWordWrap;

        private EventFlags eventFlags = EventFlags.NONE;

        private double lastPassiveReward = 0.0;

        private void loadIcons()
        {
            if (iconMenu == null)
            {

                iconMenu = new Texture2D(125, 30, TextureFormat.ARGB32, false);
                iconFinished = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeProbe = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeImpactor = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeLander = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeOrbit = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeDocking = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeSatellite = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeAviation = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeClock = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeCommunication = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeEVA = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeManned = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeRover = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeScience = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                iconTypeRepair = new Texture2D(0, 0, TextureFormat.ARGB32, false);

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
                wwwIconRepair.LoadImageIntoTexture(iconTypeRepair);
                wwwIconScience.LoadImageIntoTexture(iconTypeScience);
                wwwIconCommunication.LoadImageIntoTexture(iconTypeCommunication);

                iconDictionary.Add(Mission.Category.DOCKING, iconTypeDocking);
                iconDictionary.Add(Mission.Category.ORBIT, iconTypeOrbit);
                iconDictionary.Add(Mission.Category.PROBE, iconTypeProbe);
                iconDictionary.Add(Mission.Category.LANDING, iconTypeLander);
                iconDictionary.Add(Mission.Category.SATELLITE, iconTypeSatellite);
                iconDictionary.Add(Mission.Category.IMPACT, iconTypeImpactor);
                iconDictionary.Add(Mission.Category.AVIATION, iconTypeAviation);
                iconDictionary.Add(Mission.Category.COMMUNICATION, iconTypeCommunication);
                iconDictionary.Add(Mission.Category.EVA, iconTypeEVA);
                iconDictionary.Add(Mission.Category.MANNED, iconTypeManned);
                iconDictionary.Add(Mission.Category.ROVER, iconTypeRover);
                iconDictionary.Add(Mission.Category.REPAIR, iconTypeRepair);
                iconDictionary.Add(Mission.Category.SCIENCE, iconTypeScience);
                iconDictionary.Add(Mission.Category.TIME, iconTypeClock);
            }
        }

        private void loadStyles()
        {
            styleCaption = new GUIStyle(GUI.skin.label);
            styleCaption.normal.textColor = Color.green;
            styleCaption.fontStyle = FontStyle.Normal;
            styleCaption.alignment = TextAnchor.MiddleLeft;

            styleText = new GUIStyle(GUI.skin.label);
            styleText.normal.textColor = Color.white;
            styleText.fontStyle = FontStyle.Normal;
            styleCaption.alignment = TextAnchor.UpperLeft;

            styleValueGreen = new GUIStyle(GUI.skin.label);
            styleValueGreen.normal.textColor = Color.green;
            styleValueGreen.fontStyle = FontStyle.Normal;
            styleValueGreen.alignment = TextAnchor.MiddleRight;

            styleValueGreenBold = new GUIStyle(GUI.skin.label);
            styleValueGreenBold.normal.textColor = Color.green;
            styleValueGreenBold.fontStyle = FontStyle.Bold;
            styleValueGreenBold.alignment = TextAnchor.MiddleLeft;

            styleValueYellow = new GUIStyle(GUI.skin.label);
            styleValueYellow.normal.textColor = Color.yellow;
            styleValueYellow.fontStyle = FontStyle.Bold;
            styleValueYellow.alignment = TextAnchor.MiddleRight;

            styleValueRed = new GUIStyle(GUI.skin.label);
            styleValueRed.normal.textColor = Color.red;
            styleValueRed.fontStyle = FontStyle.Normal;
            styleValueRed.alignment = TextAnchor.MiddleRight;

            styleValueRedBold = new GUIStyle(GUI.skin.label);
            styleValueRedBold.normal.textColor = Color.red;
            styleValueRedBold.fontStyle = FontStyle.Bold;
            styleValueRedBold.alignment = TextAnchor.MiddleRight;

            styleButton = new GUIStyle(HighLogic.Skin.button);
            styleButton.normal.textColor = Color.white;
            styleButton.fontStyle = FontStyle.Bold;
            styleButton.alignment = TextAnchor.MiddleCenter;

            styleButtonYellow = new GUIStyle(HighLogic.Skin.button);
            styleButtonYellow.normal.textColor = Color.yellow;
            styleButtonYellow.fontStyle = FontStyle.Bold;
            styleButtonYellow.alignment = TextAnchor.MiddleCenter;

            styleButtonWordWrap = new GUIStyle(HighLogic.Skin.button);
            styleButtonWordWrap.normal.textColor = Color.white;
            styleButtonWordWrap.fontStyle = FontStyle.Bold;
            styleButtonWordWrap.alignment = TextAnchor.MiddleCenter;
            styleButtonWordWrap.wordWrap = true;

            styleValueName = new GUIStyle(GUI.skin.label);
            styleValueName.normal.textColor = Color.white;
            styleValueName.fontStyle = FontStyle.Normal;
            styleValueName.alignment = TextAnchor.MiddleLeft;

            styleWarning = new GUIStyle(GUI.skin.label);
            styleWarning.normal.textColor = Color.red;
            styleWarning.fontStyle = FontStyle.Bold;
            styleWarning.alignment = TextAnchor.MiddleLeft;

            styleGreenButton = new GUIStyle(HighLogic.Skin.button);
            styleGreenButton.normal.textColor = Color.green;
            styleGreenButton.alignment = TextAnchor.MiddleCenter;
            styleGreenButton.wordWrap = true;

            styleRedButton = new GUIStyle(HighLogic.Skin.button);
            styleRedButton.normal.textColor = Color.red;
            styleRedButton.alignment = TextAnchor.MiddleCenter;
            styleRedButton.wordWrap = true;

            styleIcon = new GUIStyle();

        }

        public void toggleWindow()
        {
            showMainWindow = !showMainWindow;
        }

        public void Start()
        {
            GUILoad();
            print("Mission Controller Loaded");
        }

        void OnLevelWasLoaded()
        {
            GUISave();
            repairStation.repair = false; // we have to reset the RepairGoal for it can be used again.           
            repairStation.myTime = 5.0f; // Also Reset the Time For RepairGoal For it can be used again in same session.
            SettingsManager.Manager.saveSettings();
            FuelMode.fuelinit(manager.GetFuels);
            ConstructionMode.constructinit(manager.GetConstruction);
            FinanceMode fn = new FinanceMode();
            fn.checkloans();
        }

        public void Awake()
        {
            DontDestroyOnLoad(this);

            GameEvents.onLaunch.Add(this.onLaunch);
            GameEvents.onVesselChange.Add(this.onVesselChange);
            GameEvents.onCrewKilled.Add(this.onCrewKilled);
            GameEvents.onVesselDestroy.Add(this.onVesselDestroy);
            GameEvents.onGameSceneLoadRequested.Add(this.onGameSceneLoadRequested);
            GameEvents.onCrash.Add(this.onCrash);
            GameEvents.onCollision.Add(this.onCollision);
            GameEvents.onPartCouple.Add(this.onPartCouple);
            GameEvents.onVesselRecovered.Add(this.onRecovered);
            GameEvents.onPlanetariumTargetChanged.Add(this.onTargeted);
            GameEvents.onVesselCreate.Add(this.onCreate);
            GameEvents.onPartUndock.Add(this.onUndock);

            assemblyName = Assembly.GetExecutingAssembly().GetName();
            versionCode = "" + assemblyName.Version.Major + "." + assemblyName.Version.Minor + "." + assemblyName.Version.Build;
            buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
                TimeSpan.TicksPerDay * assemblyName.Version.Build + // days since 1 January 2000
                TimeSpan.TicksPerSecond * 2 * assemblyName.Version.Revision));

            mainWindowTitle = "Mission Controller Extended " +
                versionCode + " (" + buildDateTime.ToShortDateString() + ")";

            loadIcons();

            //Debug.LogError("Using factors: " + String.Join(", ", Difficulty.Factors.Select(p => p.ToString()).ToArray()));
        }

        private void Reset(GameScenes gameScenes)
        {
            GameEvents.onLaunch.Remove(this.onLaunch);
            GameEvents.onVesselChange.Remove(this.onVesselChange);
            GameEvents.onCrewKilled.Remove(this.onCrewKilled);
            GameEvents.onVesselDestroy.Remove(this.onVesselDestroy);
            GameEvents.onGameSceneLoadRequested.Remove(this.onGameSceneLoadRequested);
            GameEvents.onCrash.Remove(this.onCrash);
            GameEvents.onCollision.Remove(this.onCollision);
            GameEvents.onVesselRecovered.Remove(this.onRecovered);
            GameEvents.onPlanetariumTargetChanged.Remove(this.onTargeted);
            GameEvents.onVesselCreate.Remove(this.onCreate);
            GameEvents.onPartUndock.Remove(this.onUndock);
        }

        public void GUILoad()
        {
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<MissionController>();
            configfile.load();

            mainWindowPosition = configfile.GetValue<Rect>("maineWindowPostion");
            settingsWindowPosition = configfile.GetValue<Rect>("settingsWindowPostion");
            financeWindowPosition = configfile.GetValue<Rect>("finanaceWindowPostion");
            researchtreewinpostion = configfile.GetValue<Rect>("kerbalnautWindowPostion");
            packageWindowPosition = configfile.GetValue<Rect>("packageWindowPostion");
        }

        public void GUISave()
        {
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<MissionController>();

            configfile.SetValue("maineWindowPostion", mainWindowPosition);
            configfile.SetValue("settingsWindowPostion", settingsWindowPosition);
            configfile.SetValue("finanaceWindowPostion", financeWindowPosition);
            configfile.SetValue("kerbalnautWindowPostion", researchtreewinpostion);
            configfile.SetValue("packageWindowPostion", packageWindowPosition);

            configfile.save();
        }

        

        /// <summary>
        /// Returns the active vessel if there is one, null otherwise
        /// </summary>
        /// <value>The active vessel.</value>
        private Vessel activeVessel
        {
            get
            {
                // We need this try-catch-block, because FlightGlobals.ActiveVessel might throw
                // an exception
                try
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
                    {
                        return FlightGlobals.ActiveVessel;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        private ProtoVessel pVessel; // NK for new recyce on recover

        /// <summary>
        /// We check for passive missions and client controlled missions every day.
        /// </summary>
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                hideAll = !hideAll;
            }

            try
            {
                if (!isValidScene())
                {
                    return;
                }

                // Every game day we check the passive missions and reward the player
                if ((lastPassiveReward == 0.0 || Planetarium.GetUniversalTime() - lastPassiveReward >= 60.0 * 60.0 * 24.0)
                        && Planetarium.GetUniversalTime() != 0.0)
                {
                    
                    lastPassiveReward = Planetarium.GetUniversalTime();

                    // we better make sure that lastPassiveReward is non zero:
                    if (lastPassiveReward == 0.0)
                    {
                        lastPassiveReward = 1.0;
                    }

                    // Now we check the all passive missions, that are currently active
                    List<MissionStatus> passives = manager.getActivePassiveMissions();
                    double time = Planetarium.GetUniversalTime();
                    foreach (MissionStatus s in passives)
                    {
                        int daysDiff = (int)((time - s.lastPassiveRewardTime) / (60.0 * 60.0 * 24.0));
                        // If the last time we gave reward is longer than one day ago,
                        // we reward the player now
                        if (daysDiff > 0)
                        {
                            // Now we check if the vessel is still active. If it is not, we will punish the player
                            // and remove the old mission status

                            ProtoVessel pv = HighLogic.CurrentGame.flightState.protoVessels
                                .Find(p => p.vesselID.ToString().Equals(s.vesselGuid) && p.vesselType != VesselType.Debris);

                            if (pv != null)
                            {
                                s.lastPassiveRewardTime = time;
                                manager.reward(daysDiff * s.passiveReward);
                            }
                            else
                            {
                                manager.removeMission(s);
                                manager.costs(s.punishment);
                            }
                        }
                    }

                    // After that we check for client controlled missions. If those vessels get destroyed, we will punish the player
                    passives = manager.getClientControlledMissions();
                    foreach (MissionStatus s in passives)
                    {
                        ProtoVessel pv = HighLogic.CurrentGame.flightState.protoVessels
                            .Find(p => p.vesselType != VesselType.Debris && p.vesselID.ToString().Equals(s.vesselGuid));

                        if (pv == null)
                        {
                            manager.removeMission(s);
                            manager.costs(s.punishment);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        bool partsCostCorrected = false; // NK for setting part costs on load
        public void OnGUI()
        {
            if (!isValidScene())
            {
                pVessel = null;
                return;
            }
            if (HighLogic.LoadedScene.Equals(GameScenes.MAINMENU) || HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER) || HighLogic.LoadedScene.Equals(GameScenes.EDITOR)
                || HighLogic.LoadedScene.Equals(GameScenes.SPH) || HighLogic.LoadedScene.Equals(GameScenes.TRACKSTATION))
                manager.resetLatest();

            if (!HighLogic.LoadedSceneIsFlight)
                canRecycle = true; // reenable once exit flight
            // NK
            if (!partsCostCorrected)
            {
                Tools.FindMCSettings();

                pVessel = null;
                partsCostCorrected = true;
                print("*MC* Calculating part costs!");
                foreach (AvailablePart ap in PartLoader.LoadedPartsList)
                {
                    try
                    {
                        int cst = PartCost.cost(ap);
                        print("For part " + ap.name + ": " + ap.title + ", cost = " + cst);
                        ap.cost = cst;
                    }
                    catch
                    {
                    }
                }
                EditorPartList.Instance.Refresh();

            }

            if (drawLandingArea)
            {
                CelestialBody kerbin = FlightGlobals.Bodies.Find(b => b.bodyName.Equals("Kerbin"));
                if (kerbin != null)
                {
                    //                    GLUtils.drawLandingArea (kerbin, 80, 90, -170.0, 170.0, new Color(1.0f, 0.0f, 0.0f, 0.5f));
                }
            }

            loadIcons();
            loadStyles();

            GUI.skin = HighLogic.Skin;

            // We need to calculate the status in case the windows are not visible
            calculateStatus(currentMission, true, activeVessel);

            if (hideAll)
            {
                return;
            }

            if (GUI.Button(new Rect(Screen.width / 3 - 44, Screen.height - 29, 125, 35), iconMenu, styleIcon))
            { //3-15
                toggleWindow();
            }

            if (showMainWindow)
            {
                mainWindowPosition = GUILayout.Window(98765, mainWindowPosition, drawMainWindow, mainWindowTitle, GUILayout.MinHeight(700), GUILayout.MaxHeight(700), GUILayout.MinWidth(375), GUILayout.MaxWidth(375));
            }

            if (showSettingsWindow)
            {
                settingsWindowPosition = GUILayout.Window(98763, settingsWindowPosition, drawSettingsWindow, "Settings", GUILayout.MinHeight(300), GUILayout.MinWidth(300));
            }

            if (showMissionPackageBrowser)
            {
                packageWindowPosition = GUILayout.Window(98762, packageWindowPosition, drawPackageWindow, currentPackage.name, GUILayout.MinHeight(750), GUILayout.MinWidth(1000));
            }

            if (showFinanceWindow)
            {
                financeWindowPosition = GUILayout.Window(98761, financeWindowPosition, drawFinaceWindow, "Finance Window", GUILayout.MinHeight(350), GUILayout.MinWidth(300));
            }

            if (showRecycleWindow)
            {
                GUILayout.Window(98766, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 100), drawRecycleWindow, "Recycle Window");
            }
            if (showRandomWindow)
            {
                GUILayout.Window(98866, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 100), drawRandomWindow, "Event Window");
            }

            if (showResearchTreeWindow)
            {
                researchtreewinpostion = GUILayout.Window(98760, researchtreewinpostion, drawResearchTree, "Research Window", GUILayout.MinHeight(350), GUILayout.MinWidth(500));
            }

            if (fileBrowser != null)
            {
                GUI.skin = HighLogic.Skin;
                GUIStyle list = GUI.skin.FindStyle("List Item");
                list.normal.textColor = XKCDColors.DarkYellow;
                list.contentOffset = new Vector2(1, 0);
                list.fontSize = 20;
                list.alignment = TextAnchor.MiddleLeft;

                fileBrowser.OnGUI();

                // Reset Skin
                list.normal.textColor = new Color(0.739f, 0.739f, 0.739f);
                list.contentOffset = new Vector2(1, 42.4f);
                list.fontSize = 10;
            }
        }

        private void drawRecycleWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            if (settings.gameMode != 0 && (manager.ResearchRecycle || HighLogic.CurrentGame.Mode != Game.Modes.CAREER))
            {
                showCostValue("Vessel " + recycledName + " recyled: ", recycledCost, styleCaption);
                GUILayout.BeginHorizontal();
                GUILayout.Label("  (" + recycledDesc + ")", styleValueName);
                GUILayout.EndHorizontal();

            }
            if (recycledCrewCost > 0)
            {
                GUILayout.BeginHorizontal();
                showCostValue("Crew Insurance Returned: ", recycledCrewCost, styleCaption);
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("OK", styleButtonWordWrap))
            {
                showRecycleWindow = false;
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void drawRandomWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            Status status = calculateStatus(currentMission, true, activeVessel);
            Mission mission = new Mission();

            
            GUILayout.Label("Current Mission: " +mission.name, styleText);
            if (manager.budget < 0)
            {
                if (settings.gameMode == 1)
                {
                    GUILayout.Label("All goals accomplished. Deducted For Loans!", styleCaption);
                    showCostValue("Total Mission Payout:", currentMission.reward * FinanceMode.currentloan, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
                if (settings.gameMode == 2)
                {
                    GUILayout.Label("All Goals accomplished. Hardcore and Deducted Loans"); // .75 * .6 = .45
                    showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan) * .60, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
            }
            else
            {
                if (settings.gameMode == 1)
                {
                    GUILayout.Label("All goals accomplished. you can finish the mission now!", styleCaption);
                    showCostValue("Total Mission Payout:", currentMission.reward, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
                if (settings.gameMode == 2)
                {
                    GUILayout.Label("All goals accomplished. you can finish the mission now: HardCore Mode 40 % Reduction!", styleCaption);
                    showCostValue("Total Mission Payout:", currentMission.reward * 60 / 100, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
            }

            if (GUILayout.Button("Finish The Mission", styleButtonWordWrap))
            {
                manager.finishMission(currentMission, activeVessel, status.events);
                hiddenGoals = new List<MissionGoal>();
                currentMission = null;
                finishmissiontoggle = true;
                showRandomWindow = false;
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>
        /// Draws the main mission window.
        /// Do not use currentMission.isDone or missionGoal.isDone(), use status instead!!!
        /// </summary>
        /// <param name="id">Identifier.</param>

        private void drawMainWindow(int id)
        {

            Status status = calculateStatus(currentMission, true, activeVessel);

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Current budget: ", GUILayout.Height(30));
            GUILayout.Box(manager.budget + CurrencySuffix, GUILayout.Height(30));           
            GUILayout.EndHorizontal();
            // Edits malkuth shows the modes that you have the plugin set to from settings .13 added the Borrowing Money mission deduction of %25
            GUILayout.Space(10);
            if (settings.disablePlugin == true)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("PLUGIN IS DISABLED ", GUILayout.Height(25));
                GUILayout.EndHorizontal();
            }
            if (settings.gameMode == 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Test Flight Mode ", GUILayout.Height(25));
                GUILayout.EndHorizontal();
            }
            if (settings.gameMode == 1)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Flight Mode ", GUILayout.Height(25));
                GUILayout.EndHorizontal();
            }
            if (manager.budget < 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("In Red, Borrowing Money", GUILayout.Height(25));
                GUILayout.EndHorizontal();
            }
            if (settings.gameMode == 2)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("HardCore Mode", GUILayout.Height(25));
                GUILayout.EndHorizontal();
            }
            // Show only when the loaded scene is an editor or a vessel is available and its situation is PRELAUNCH

            scrollPosition = GUILayout.BeginScrollView(scrollPosition,GUILayout.MaxWidth(365), GUILayout.MaxHeight(550));

            if (HighLogic.LoadedSceneIsEditor || status.onLaunchPad)
            {
                VesselResources res = new VesselResources(activeVessel);

                // .11 Edited malkuth shows only when in Testing Mode.  Plan to add things like Delta V stats and other Helpful testing info
                if (settings.gameMode == 0)
                {                    
                    showCostValue("Flight Testing Cost:", res.dry() * 6 / 100, (res.dry() * 6 / 100 > manager.budget ? styleValueRedBold : styleValueYellow));
                }
                else
                {
                    
                    showCostValue("Total Cost Vessel:", res.sum(), (res.sum() > manager.budget ? styleValueRedBold : styleValueYellow));                   
                    ExpandCost = GUILayout.Toggle(ExpandCost, "Expand Cost List");
                    if (ExpandCost != false)
                    {
                        showCostValue("Crew insurance (Launch Pad Only): ", res.crew(), styleValueGreen);
                        if (res.pod() > (0)) { showCostValue("Command Sections:", res.pod(), styleValueGreen); }
                        if (res.ctrl() > (0)) { showCostValue("Avionics and Control:", res.ctrl(), styleValueGreen); } // NOT control surfaces. Those are AERO parts. These are SAS etc
                        if (res.util() > (0)) { showCostValue("Utility Parts:", res.util(), styleValueGreen); }
                        if (res.sci() > (0)) { showCostValue("Science Parts:", res.sci(), styleValueGreen); }
                        if (res.engine() > (0)) { showCostValue("Engines And Cooling: ", res.engine(), styleValueGreen); }
                        if (res.tank() > (0)) { showCostValue("Fuel Tank Cost: ", res.tank(), styleValueGreen); }
                        if (res.stru() > (0)) { showCostValue("Structural Cost:", res.stru(), styleValueGreen); }
                        if (res.aero() > (0)) { showCostValue("Aerodynamic Cost:", res.aero(), styleValueGreen); }
                        // pull from resources
                        if(res.resources.Count > 0)
                        {
                            List<string> resInVessel = res.resources.Keys.ToList();
                            resInVessel.Sort();
                            foreach(string r in resInVessel)
                                showCostValue(r, Math.Round(res.resources[r],0), styleValueGreen);
                        }
                        if (res.wet() > (0)) { showCostValue("(Total Cost Of Fuels):", res.wet(), styleCaption); }
                        if (res.dry() > (0)) { showCostValue("(Total Cost Of Parts):", res.dry(), styleCaption); }  
                        GUILayout.Space(20);
                    }

                }
            }
            
            {

                if (status.isClientControlled)
                {
                    MissionStatus s = manager.getClientControlledMission(activeVessel);
                    GUILayout.Label("This vessel is controlled by a client. Do not destroy this vessel! Fine: " + s.punishment + CurrencySuffix, styleWarning);
                    GUILayout.Label("End of life in " + MathTools.formatTime(s.endOfLife - Planetarium.GetUniversalTime()));
                }
                else if (status.isOnPassiveMission)
                {
                    MissionStatus s = manager.getPassiveMission(activeVessel);
                    GUILayout.Label("This vessel is involved in a passive mission. Do not destroy this vessel! Fine: " + s.punishment + CurrencySuffix, styleWarning);
                    GUILayout.Label("End of life in " + MathTools.formatTime(s.endOfLife - Planetarium.GetUniversalTime()));
                }

                if (currentMission != null)
                {
                    ShowMissionGoals = GUILayout.Toggle(ShowMissionGoals, "Show Mission Info");
                    if (ShowMissionGoals != false)
                    {
                        drawMission(currentMission, status);
                    }
                }
            }
                GUILayout.Space(30);
                GUILayout.EndScrollView();

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(125));
                showSettingsWindow = GUILayout.Toggle(showSettingsWindow, "Settings");
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Width(125));
                showFinanceWindow = GUILayout.Toggle(showFinanceWindow, "Finances");
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                {
                    showResearchTreeWindow = GUILayout.Toggle(showResearchTreeWindow, "Research");
                }
                GUILayout.EndVertical();    
                GUILayout.EndHorizontal();


                //            if (GUILayout.Button ("Draw landing area!", styleButton)) {
                //                drawLandingArea = !drawLandingArea;
                //            }
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(125));
                FileBrowserBool = GUILayout.Toggle(FileBrowserBool, "Packages");

                if (FileBrowserBool == true)
                {
                    createFileBrowser("Select mission from package", selectMissionPackage);
                    FileBrowserBool = false;
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Width(125));

                if (currentPackage != null)
                {

                    packageWindowtoggle = GUILayout.Toggle(packageWindowtoggle, "Missions");
                    if (packageWindowtoggle == true)
                    {
                        packageWindow(true);
                        packageWindowtoggle = false;
                    }
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                if (currentMission != null)
                {
                    currentMissiontoggle = GUILayout.Toggle(currentMissiontoggle, "Deselect");
                    if (currentMissiontoggle == true)
                    {
                        currentMission = null;
                        currentMissiontoggle = false;
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                if (status.missionIsFinishable)
                {

                    showRandomWindow = true;
                    finishmissiontoggle = GUILayout.Toggle(finishmissiontoggle, "FINISH THE CURRENT MISSION");
                    if (finishmissiontoggle == false)
                    {
                        manager.finishMission(currentMission, activeVessel, status.events);
                        hiddenGoals = new List<MissionGoal>();
                        currentMission = null;
                        finishmissiontoggle = true;
                    }

                }
                // NK recycle from tracking station
                if (pVessel != null && settings.gameMode != 0 && (manager.ResearchRecycle || HighLogic.CurrentGame.Mode != Game.Modes.CAREER))
                {
                    //print("*MC* In TS, pVessel not null");
                    if (pVessel.situation.Equals(Vessel.Situations.LANDED) || pVessel.situation.Equals(Vessel.Situations.SPLASHED))
                    {
                        VesselResources res = new VesselResources(pVessel.vesselRef);
                        showCostValue("Recyclable value: ", res.recyclable(pVessel.situation.Equals(Vessel.Situations.LANDED) ? 1 : 0), styleCaption);
                    }
                }

                GUILayout.EndVertical();
                if (!Input.GetMouseButtonDown(1))
                {
                    GUI.DragWindow();
                }
            
        }

        /// <summary>
        /// Selects the mission in the file
        /// </summary>
        /// <param name="file">File.</param>
        private void selectMissionPackage(String file)
        {
            destroyFileBrowser();

            if (file == null)
            {
                return;
            }

            currentPackage = manager.loadMissionPackage(file);
            currentPreviewMission = null;
            if (currentPackage != null)
            {
                packageWindow(true);
            }
            currentSort = (currentPackage.ownOrder ? SortBy.PACKAGE_ORDER : SortBy.NAME);
        }

        /// <summary>
        /// Draws the mission parameters
        /// </summary>
        private void drawMission(Mission mission, Status s)
        {
            if (s.missionAlreadyFinished)
            {
                GUILayout.Label("Mission already finished!", styleWarning);
            }

            GUILayout.Label("Current Mission: ", styleValueGreenBold);
            GUILayout.Label(mission.name, styleText);
            GUILayout.Label("Description: ", styleCaption);
            GUILayout.Label(mission.description, styleText);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Reward: ", styleValueGreenBold);
            if (settings.gameMode == 1)
            { GUILayout.Label(mission.reward + CurrencySuffix, styleValueYellow); }
            if (settings.gameMode == 2)
            { GUILayout.Label(mission.reward * 60 / 100 + CurrencySuffix, styleValueYellow); }
            GUILayout.EndHorizontal();
            
            if (mission.scienceReward != 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Science Reward: ", styleValueGreenBold);
                GUILayout.Label(mission.scienceReward + " sp", styleValueYellow);
                GUILayout.EndHorizontal();
            }

            if (mission.passiveMission)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Reward every day: ", styleValueYellow);
                GUILayout.Label(mission.passiveReward + CurrencySuffix, styleValueGreen);
                GUILayout.EndHorizontal();
            }

            if (mission.lifetime != 0.0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Lifetime: ", styleValueYellow);
                GUILayout.Label(MathTools.formatTime(mission.lifetime), styleValueGreen);
                GUILayout.EndHorizontal();
            }

            if (mission.clientControlled)
            {
                GUILayout.Label("The client will get the control over this vessel once the mission is finished!", styleWarning);
            }

            if (mission.repeatable)
            {
                GUILayout.Label("Mission is repeatable!", styleCaption);
            }

            if (s.requiresAnotherMission)
            {
                GUILayout.Label("This mission requires the mission \"" + mission.requiresMission + "\". Finish mission \"" +
                                 mission.requiresMission + "\" first, before you proceed.", styleWarning);
            }

            drawMissionGoals(mission, s);

            if (s.missionIsFinishable)
            {
                if (manager.budget < 0)
                {
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All goals accomplished. You can finish the mission now! Deducted for loans!", styleCaption);
                        showCostValue("Total Mission Payout:", currentMission.reward * FinanceMode.currentloan, styleValueGreen);
                        showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                    }
                    if (settings.gameMode == 2)
                    {
                        GUILayout.Label("All Goals accomplished. Finish The Mission. Deducted for loans and HardCore mode"); // .75 * .6 = .45
                        showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan) * 60, styleValueGreen);
                        showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                    }
                }
                else
                {
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now!", styleCaption);
                        showCostValue("Total Mission Payout:", currentMission.reward, styleValueGreen);
                        showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                    }
                    if (settings.gameMode == 2)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now: HardCore Mode!", styleCaption);
                        showCostValue("Total Mission Payout:", currentMission.reward * 60 / 100, styleValueGreen);
                        showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the mission goals
        /// </summary>
        /// <param name="mission">Mission.</param>
        private void drawMissionGoals(Mission mission, Status s)
        {
            int index = 1;

            foreach (MissionGoal c in mission.goals)
            {                
                {
                    if (hiddenGoals.Contains(c))
                    {
                        index++;
                        continue;
                    }

                    if (c is SubMissionGoal)
                    {
                        GUILayout.Label((index++) + ". Mission goal (complete all):" + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }
                    else if (c is OrMissionGoal)
                    {
                        GUILayout.Label((index++) + ". Mission goal (complete any):" + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }
                    else if (c is OrMissionGoal)
                    {
                        GUILayout.Label((index++) + ". Mission goal (complete none):" + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }
                    else
                    {
                        GUILayout.Label((index++) + ". Mission goal: " + c.getType() + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }

                    if (c.description.Length != 0)
                    {
                        GUILayout.Label("Description: ", styleCaption);
                        GUILayout.Label(c.description, styleText);
                    }

                    if (c.nonPermanent && c.reward != 0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Reward:", styleValueGreenBold);
                        GUILayout.Label(c.reward + CurrencySuffix, styleValueYellow);
                        GUILayout.EndHorizontal();
                    }

                    List<Value> values = c.getValues(activeVessel, s.events);

                    foreach (Value v in values)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(v.name, styleValueName);
                        if (v.currentlyIs.Length == 0)
                        {
                            GUILayout.Label(v.shouldBe, styleValueGreen);
                        }
                        else
                        {
                            GUILayout.Label(v.shouldBe + " : " + v.currentlyIs, (v.done ? styleValueGreen : styleValueRed));
                        }
                        GUILayout.EndHorizontal();
                    }

                    if (activeVessel != null)
                    {
                        if (s.finishableGoals.ContainsKey(c.id) && s.finishableGoals[c.id])
                        {
                            if (GUILayout.Button("Hide finished goal"))
                            {
                                hiddenGoals.Add(c);
                            }
                        }
                        else
                        {
                            if (c.optional)
                            {
                                if (GUILayout.Button("Hide optional goal"))
                                {
                                    hiddenGoals.Add(c);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Create the file browser
        private void createFileBrowser(string title, FileBrowser.FinishedCallback callback)
        {
            fileBrowser = new FileBrowser(new Rect(Screen.width / 2, 100, 350, 500), title, callback, true);
            fileBrowser.BrowserType = FileBrowserType.File;
            fileBrowser.CurrentDirectory = missionFolder;
            fileBrowser.disallowDirectoryChange = true;
            fileBrowser.SelectionPattern = "*.mpkg";

            if (EditorLogic.fetch != null)
            {
                EditorLogic.fetch.Lock(true, true, true);
            }
        }

        private void destroyFileBrowser()
        {
            fileBrowser = null;

            if (EditorLogic.fetch != null)
            {
                EditorLogic.fetch.Unlock();
            }
        }

        /// <summary>
        /// Sets the visibility of the settings window
        /// </summary>
        /// <param name="visibility">If set to <c>true</c> visibility.</param>
        private void settingsWindow(bool visibility)
        {
            showSettingsWindow = visibility;
            lockOrUnlockEditor(visibility);
        }

        /// <summary>
        /// Sets the visibility of the Finace Window
        /// </summary>
        /// <param name="visibility">If set to <c>true</c> visibility.</param>
        private void financeWindow(bool visibility)
        {
            showFinanceWindow = visibility;
            lockOrUnlockEditor(visibility);
        }
        /// <summary>
        /// Sets The Visibility of the KerbalNauts Recruitment Window
        /// </summary>
        /// <param name="visibility"></param>
        private void ResearchTreeWindow(bool visibility)
        {
            showResearchTreeWindow = visibility;
            lockOrUnlockEditor(visibility);
        }

        /// <summary>
        /// Sets the visibility of the package browser
        /// </summary>
        /// <param name="visibility">If set to <c>true</c> visibility.</param>
        private void packageWindow(bool visibility)
        {
            showMissionPackageBrowser = visibility;
            lockOrUnlockEditor(visibility);
        }

        /// <summary>
        /// Locks or unlocks the editor, if it is available
        /// </summary>
        /// <param name="visiblity">If set to <c>true</c> visiblity.</param>
        private void lockOrUnlockEditor(bool visiblity)
        {
            if (EditorLogic.fetch != null)
            {
                if (visiblity)
                {
                    EditorLogic.fetch.Lock(true, true, true);
                }
                else
                {
                    EditorLogic.fetch.Unlock();
                }
            }
        }

        private void showCostValue(String name, double value, GUIStyle style)
        {
            showStringValue(name, String.Format("{0:N0}{1}", value, CurrencySuffix), style);
        }

        private void showDoubleValue(String name, double value, GUIStyle style)
        {
            showStringValue(name, String.Format("{0:N3}", value), style);
        }

        private void showIntValue(String name, int value, GUIStyle style)
        {
            showStringValue(name, String.Format("{0:N0}", value), style);
        }

        private void showStringValue(String name, String value, GUIStyle style)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name, styleValueName);
            GUILayout.Label(value, style);
            GUILayout.EndHorizontal();
        }

        private bool isValidScene()
        {
            if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor
                || HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER)
                || HighLogic.LoadedScene.Equals(GameScenes.TRACKSTATION)
                )
            {
                return true;
            }
            return false;
        }

        private const String CurrencySuffix = " ₭";
    }
    /// <summary>
    /// KSPAddon with equality checking using an additional type parameter. Fixes the issue where AddonLoader prevents multiple start-once addons with the same start scene.
    /// </summary>
    public class KSPAddonFixed : KSPAddon, IEquatable<KSPAddonFixed>
    {
        private readonly Type type;

        public KSPAddonFixed(KSPAddon.Startup startup, bool once, Type type)
            : base(startup, once)
        {
            this.type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) { return false; }
            return Equals((KSPAddonFixed)obj);
        }

        public bool Equals(KSPAddonFixed other)
        {
            if (this.once != other.once) { return false; }
            if (this.startup != other.startup) { return false; }
            if (this.type != other.type) { return false; }
            return true;
        }

        public override int GetHashCode()
        {
            return this.startup.GetHashCode() ^ this.once.GetHashCode() ^ this.type.GetHashCode();
        }
    }
}


