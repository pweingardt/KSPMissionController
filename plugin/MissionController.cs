using System;
using UnityEngine;
using MissionController;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using KSP.IO;
using Toolbar;

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

        private Rect settingsWindowPosition;
        private Rect packageWindowPosition;
        private Rect financeWindowPosition;
        private Rect researchtreewinpostion;
               
        Rect VabBudgetWin;
        Rect VabBudgetWin1;
        Rect VabBudgetWin2;
        Rect VabBudgetWin3;
        Rect VabShipBuildList;
        Rect VabShipBuildList1;
        Rect VabShipBuildList2;
        Rect VabShipWindow;
        Rect MissionWindowStatus;

        private IButton button;
        private IButton BudgetDisplay;
        private IButton MissionSelect;
        private IButton VabShipSelect;
        private IButton RevertSelect;
        private IButton ScienceResearch;
        private IButton Hidetoolbars;

        private bool showSettingsWindow = false;
        private bool showMissionPackageBrowser = false;
        private bool showFinanceWindow = false;
        private bool showRecycleWindow = false;
        private bool showResearchTreeWindow = false;
        private bool showRandomWindow = false;
        private bool showRevertWindow = false;
        private bool showconstructionwindow = false;
        private bool showbudgetamountwindow = false;
        private bool hidetoolbarsviews = true;
        private bool showVabShipWindow = false;
        private bool showMissionStatusWindow = false;
               
        public string recycledName = "";
        public string recycledDesc = "";
        public int recycledCost = 0;
        public int recycledCrewCost = 0;

        private FileBrowser fileBrowser = null;
        private Mission currentMission = null;
        private MissionPackage currentPackage = null;

        private Vector2 scrollPosition = new Vector2(0, 0);
        private Vector2 scrollPositionship = new Vector2(0, 0);
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
        private GUIStyle styleGreenButtonCenter, styleRedButtonCenter;
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
            styleButtonYellow.alignment = TextAnchor.MiddleLeft;

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

            styleGreenButtonCenter = new GUIStyle(HighLogic.Skin.button);
            styleGreenButtonCenter.normal.textColor = Color.green;
            styleGreenButtonCenter.alignment = TextAnchor.MiddleLeft;
            styleGreenButtonCenter.wordWrap = true;

            styleRedButton = new GUIStyle(HighLogic.Skin.button);
            styleRedButton.normal.textColor = Color.red;
            styleRedButton.alignment = TextAnchor.MiddleCenter;
            styleRedButton.wordWrap = true;

            styleRedButtonCenter = new GUIStyle(HighLogic.Skin.button);
            styleRedButtonCenter.normal.textColor = Color.red;
            styleRedButtonCenter.alignment = TextAnchor.MiddleLeft;
            styleRedButtonCenter.wordWrap = true;

            styleIcon = new GUIStyle();

        }        

        public void Start()
        {            
            print("Mission Controller Loaded");
            
            button = ToolbarManager.Instance.add("MC1", "Settings1");
            button.TexturePath = "MissionController/icons/settings";
            button.ToolTip = "Mission Controller Settings";
            button.Visibility = new GameScenesVisibility(GameScenes.SPACECENTER,GameScenes.EDITOR,GameScenes.FLIGHT);
            button.OnClick += (e) =>
            {
                showSettingsWindow = !showSettingsWindow;
            };

            BudgetDisplay = ToolbarManager.Instance.add("MC1", "money1");
            BudgetDisplay.TexturePath = "MissionController/icons/money";
            BudgetDisplay.ToolTip = "Current Budget";
            BudgetDisplay.Visibility = new GameScenesVisibility(GameScenes.SPACECENTER,GameScenes.EDITOR,GameScenes.SPH);
            BudgetDisplay.OnClick += (e) =>
            {
                showFinanceWindow = !showFinanceWindow;
            };

            MissionSelect = ToolbarManager.Instance.add("MC1", "missionsel1");
            MissionSelect.TexturePath = "MissionController/icons/mission";
            MissionSelect.ToolTip = "Select Current Mission";
            MissionSelect.Visibility = new GameScenesVisibility(GameScenes.SPACECENTER,GameScenes.FLIGHT);
            MissionSelect.OnClick += (e) =>
             {
                 showMissionStatusWindow = !showMissionStatusWindow;
             };

            VabShipSelect = ToolbarManager.Instance.add("MC1", "ship1");
            VabShipSelect.TexturePath = "MissionController/icons/blueprints";
            VabShipSelect.ToolTip = "Ship Value BreakDown";
            VabShipSelect.Visibility = new GameScenesVisibility(GameScenes.EDITOR,GameScenes.SPH);
            VabShipSelect.OnClick += (e) =>
            {
                showVabShipWindow = !showVabShipWindow;
            };

            ScienceResearch = ToolbarManager.Instance.add("MC1", "ship3");
            ScienceResearch.TexturePath = "MissionController/icons/research";
            ScienceResearch.ToolTip = "Mission Controller Research Window";
            ScienceResearch.Visibility = new GameScenesVisibility(GameScenes.SPACECENTER,GameScenes.EDITOR,GameScenes.SPH);
            ScienceResearch.OnClick += (e) =>
            {
                showResearchTreeWindow = !showResearchTreeWindow;
            };

            RevertSelect = ToolbarManager.Instance.add("MC1", "ship2");
            RevertSelect.TexturePath = "MissionController/icons/revert";
            RevertSelect.ToolTip = "Revert To VAB Cost 1000 Credits";
            RevertSelect.Visibility = new GameScenesVisibility(GameScenes.FLIGHT);
            RevertSelect.OnClick += (e) =>
              {
                 if (FlightDriver.CanRevertToPrelaunch)
                  {
                     showRevertWindow = !showRevertWindow;
                  }
              };

            Hidetoolbars = ToolbarManager.Instance.add("MC1", "Hide");
            Hidetoolbars.TexturePath = "MissionController/icons/hide";
            Hidetoolbars.ToolTip = "Hide Mission Controller Extra Windows";
            Hidetoolbars.Visibility = new GameScenesVisibility(GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.SPH,GameScenes.FLIGHT);
            Hidetoolbars.OnClick += (e) =>
            {
                hidetoolbarsviews = !hidetoolbarsviews;
            };

            GUILoad();

        }

        void OnLevelWasLoaded()
        {   
            repairStation.repair = false; // we have to reset the RepairGoal for it can be used again.           
            repairStation.myTime = 5.0f; // Also Reset the Time For RepairGoal For it can be used again in same session.
            SettingsManager.Manager.saveSettings();
            FuelMode.fuelinit(manager.GetFuels);
            ConstructionMode.constructinit(manager.GetConstruction);
            PayoutLeveles.payoutlevels(manager.GetCurrentPayoutLevel);
            FinanceMode fn = new FinanceMode();
            fn.checkloans();
            GUISave();
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
            GameEvents.onVesselCreate.Add(this.onVesselCreate);
            
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
            GameEvents.onVesselCreate.Remove(this.onVesselCreate);
        }

        public void GUILoad()
        {
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<MissionController>();
            configfile.load();

            settingsWindowPosition = configfile.GetValue<Rect>("settingsWindowPostion");
            financeWindowPosition = configfile.GetValue<Rect>("finanaceWindowPostion");
            researchtreewinpostion = configfile.GetValue<Rect>("kerbalnautWindowPostion");
            packageWindowPosition = configfile.GetValue<Rect>("packageWindowPostion");
            
            VabBudgetWin = configfile.GetValue<Rect>("vabBudgetWin");
            VabBudgetWin1 = configfile.GetValue<Rect>("vabBudgetWin1");
            VabBudgetWin2 = configfile.GetValue<Rect>("vabBudgetWin2");
            VabBudgetWin3 = configfile.GetValue<Rect>("vabBudgetWin3");
            VabShipBuildList = configfile.GetValue<Rect>("VabShipBuildList");
            VabShipBuildList1 = configfile.GetValue<Rect>("VabShipBuildList1");
            VabShipBuildList2 = configfile.GetValue<Rect>("VabShipBuildList2");
            VabShipWindow = configfile.GetValue<Rect>("VabShipWindow");
            MissionWindowStatus = configfile.GetValue<Rect>("MissionWindowStatus");
        }

         public void GUISave()
        {
             KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<MissionController>();
 
             
            configfile.SetValue("settingsWindowPostion", settingsWindowPosition);
            configfile.SetValue("finanaceWindowPostion", financeWindowPosition);
            configfile.SetValue("kerbalnautWindowPostion", researchtreewinpostion);
            configfile.SetValue("packageWindowPostion", packageWindowPosition);
            configfile.SetValue("vabBudgetWin", VabBudgetWin);
            configfile.SetValue("vabBudgetWin1", VabBudgetWin1);
            configfile.SetValue("vabBudgetWin2", VabBudgetWin2);
            configfile.SetValue("vabBudgetWin3", VabBudgetWin3);
            configfile.SetValue("VabShipBuildList", VabShipBuildList);
            configfile.SetValue("VabShipBuildList1", VabShipBuildList1);
            configfile.SetValue("VabShipBuildList2", VabShipBuildList2);
            configfile.SetValue("VabShipWindow", VabShipWindow);
            configfile.SetValue("MissionWindowStatus", MissionWindowStatus);
 
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

        public void OnDestroy()
        {            
            button.Destroy();
        }

       
        /// <summary>
        /// Loads GUIWindows
        /// </summary>
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
            
            // Shows Mission Finsihed Window when done
            Status status = calculateStatus(currentMission, true, activeVessel);
            if (status.missionIsFinishable)
            {
                showRandomWindow = true;
            }

            // We need to calculate the status in case the windows are not visible
            calculateStatus(currentMission, true, activeVessel);

            if (hideAll)
            {
                return;
            }

            if (HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER))
            {
                showbudgetamountwindow = true;
            }

            if (HighLogic.LoadedScene.Equals(GameScenes.EDITOR))
            {                
                showconstructionwindow = true;
            }

            if (showconstructionwindow && hidetoolbarsviews)
            {
                VesselResources res = new VesselResources(activeVessel);
                if (HighLogic.LoadedScene.Equals(GameScenes.EDITOR))
                {               
                    VabShipBuildList = GUILayout.Window(898992, VabShipBuildList, drawconstructioncostwindow, "Ship Value: " + res.sum(), (res.sum() > manager.budget ? styleRedButtonCenter : styleGreenButtonCenter), GUILayout.MinHeight(20), GUILayout.MinWidth(175));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.FLIGHT))
                {
                    VabShipBuildList1 = GUILayout.Window(898992, VabShipBuildList1, drawconstructioncostwindow, "SV : " + res.sum(), (res.sum() > manager.budget ? styleRedButtonCenter : styleGreenButtonCenter), GUILayout.MinHeight(20), GUILayout.MinWidth(100));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.SPH))
                {
                    VabShipBuildList2 = GUILayout.Window(898992, VabShipBuildList2, drawconstructioncostwindow, "Ship Value: " + res.sum(), (res.sum() > manager.budget ? styleRedButtonCenter : styleGreenButtonCenter), GUILayout.MinHeight(20), GUILayout.MinWidth(175));
                }
            }

            if (showbudgetamountwindow && hidetoolbarsviews)
            {
                if (HighLogic.LoadedScene.Equals(GameScenes.EDITOR))
                {
                    VabBudgetWin = GUILayout.Window(898993, VabBudgetWin, drawbudgetwindow, "Current Budget: " + manager.budget + CurrencySuffix, styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(175));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER))
                {
                    VabBudgetWin1 = GUILayout.Window(898993, VabBudgetWin1, drawbudgetwindow, "Current Budget: " + manager.budget + CurrencySuffix, styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(175));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.FLIGHT))
                {
                    VabBudgetWin2 = GUILayout.Window(898993, VabBudgetWin2, drawbudgetwindow, "CB " + manager.budget + CurrencySuffix, styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(100));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.SPH))
                {
                    VabBudgetWin3 = GUILayout.Window(898993, VabBudgetWin3, drawbudgetwindow, "Current Budget: " + manager.budget + CurrencySuffix, styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(175));
                }
            }


            if (showVabShipWindow)
            {
                VabShipWindow = GUILayout.Window(898989, VabShipWindow, drawVabShipWindow, "Ship Breakdown List",GUILayout.MinHeight(400), GUILayout.MinWidth(300));
            }

            if (showMissionStatusWindow)
            {
                MissionWindowStatus = GUILayout.Window(898990, MissionWindowStatus, drawMissionInfoWindow, "Current Mission Window",GUILayout.MinHeight(450), GUILayout.MinWidth(350));
            }
                      
            if (showSettingsWindow)
            {
                settingsWindowPosition = GUILayout.Window(98763, settingsWindowPosition, drawSettingsWindow, "Settings", GUILayout.MinHeight(225), GUILayout.MinWidth(150));
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
            if (showRevertWindow)
            {
                GUILayout.Window(99746, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 100), drawRevertWindow, "Revert Space Program And KSP Window");
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
            if ((manager.ResearchRecycle || HighLogic.CurrentGame.Mode != Game.Modes.CAREER))
            {
                showCostValue("Vessel " + recycledName + " recyled: ", recycledCost, styleCaption);
                GUILayout.BeginHorizontal();
                GUILayout.Label("  (" + recycledDesc + ")", styleValueName);
                GUILayout.EndHorizontal();

            }
            if (recycledCrewCost > 1)
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

        private void drawRevertWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.Label("Pressing Ok Will Revert Both Mission controller");
            GUILayout.Label("And KSP To PreLaunch Conditions In The Editor");
            GUILayout.Label("You will Be charged 1000 Credits For Doing this");
            GUILayout.Label("Do You Want To Continue?");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Revert To PreLaunch", styleButtonWordWrap, GUILayout.Width(200)))
            {
                manager.loadProgramBackup(HighLogic.CurrentGame.Title);
                FlightDriver.RevertToPrelaunch(GameScenes.EDITOR);                
                manager.costs(1000);
                manager.saveProgramBackup();

                showRevertWindow = false;
            }

            if (GUILayout.Button("Do Not Revert", styleButtonWordWrap, GUILayout.Width(200)))
            {
                showRevertWindow = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void drawRandomWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            Status status = calculateStatus(currentMission, true, activeVessel);
            Mission mission = new Mission();


            GUILayout.Label("Current Mission: " + mission.name, styleText);
            if (manager.budget < 0)
            {
                if (settings.gameMode == 0)
                {
                    GUILayout.Label("All goals accomplished. Deducted For Loans!", styleCaption);
                    showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan) * PayoutLeveles.TechPayout, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
                if (settings.gameMode == 1)
                {
                    GUILayout.Label("All Goals accomplished. Hardcore and Deducted Loans"); // .75 * .6 = .45
                    showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan * PayoutLeveles.TechPayout) * .60, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
            }
            else
            {
                if (settings.gameMode == 0)
                {
                    GUILayout.Label("All goals accomplished. you can finish the mission now!", styleCaption);
                    showCostValue("Total Mission Payout:", currentMission.reward * PayoutLeveles.TechPayout, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
                if (settings.gameMode == 1)
                {
                    GUILayout.Label("All goals accomplished. you can finish the mission now: HardCore Mode 40 % Reduction!", styleCaption);
                    showCostValue("Total Mission Payout:", (currentMission.reward * PayoutLeveles.TechPayout) * .60, styleValueGreen);
                    showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                }
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Finish And Save The Mission Results", styleButtonWordWrap, GUILayout.Width(275)))
            {
                manager.finishMission(currentMission, activeVessel, status.events);
                hiddenGoals = new List<MissionGoal>();
                currentMission = null;
                showRandomWindow = false;
            }
            if (GUILayout.Button("X", styleButtonWordWrap, GUILayout.Width(25)))
            {
                currentMission = null;
                showRandomWindow = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }       

        private void drawVabShipWindow(int id)
        {            
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPositionship = GUILayout.BeginScrollView(scrollPositionship, GUILayout.Width(300));
            GUILayout.Space(20);

            VesselResources res = new VesselResources(activeVessel);

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
            if (res.resources.Count > 0)
            {
                List<string> resInVessel = res.resources.Keys.ToList();
                resInVessel.Sort();
                foreach (string r in resInVessel)
                    showCostValue(r, Math.Round(res.resources[r], 0), styleValueGreen);
            }
            if (res.wet() > (0)) { showCostValue("(Total Cost Of Fuels):", res.wet(), styleValueYellow); }
            if (res.dry() > (0)) { showCostValue("(Total Cost Of Parts):", res.dry(), styleValueYellow); }
            showCostValue("Total Cost Vessel:", res.sum(), (res.sum() > manager.budget ? styleValueRedBold : styleValueYellow));
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Exit Window", styleButtonWordWrap, GUILayout.Width(75)))
            {
                showVabShipWindow = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void drawMissionInfoWindow(int id)
        {
            Status status = calculateStatus(currentMission, true, activeVessel);

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(350));
            GUILayout.Space(20);

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
            if (manager.isVesselFlagged(activeVessel))
            {
                GUILayout.Label("Warning Vessel Is Flaged and Can't Do Missions", styleValueRedBold);
                GUILayout.Label("Vessel Most Likely Launched In Disabled Mode", styleValueRedBold);
            }

            if (currentMission != null)
            {
                drawMission(currentMission, status);     
            }
            GUILayout.EndScrollView();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Change Package", styleButtonWordWrap, GUILayout.Width(150)))
            {
                createFileBrowser("Select Contract", selectMissionPackage);
            }

            if (currentPackage != null)
            {
                if (GUILayout.Button("Select New Mission", styleButtonWordWrap, GUILayout.Width(150)))
                {
                    packageWindow(true);
                }
            }
            if (GUILayout.Button("X", styleButtonWordWrap, GUILayout.Width(25)))
            {
                showMissionStatusWindow = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void drawbudgetwindow(int id)
        {
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        private void drawconstructioncostwindow(int id)
        {
            VesselResources res = new VesselResources(activeVessel);
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        // End Of WindowGUIs
        
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
            if (settings.gameMode == 0)
            { GUILayout.Label(mission.reward * PayoutLeveles.TechPayout + CurrencySuffix, styleValueYellow); }
            if (settings.gameMode == 1)
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
                showRandomWindow = true;
                if (manager.budget < 0)
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Label("All goals accomplished. You can finish the mission now! Deducted for loans!", styleCaption);
                        showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan) * PayoutLeveles.TechPayout, styleValueGreen);
                        showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All Goals accomplished. Finish The Mission. Deducted for loans and HardCore mode"); // .75 * .6 = .45
                        showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan * PayoutLeveles.TechPayout) * .60, styleValueGreen);
                        showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                    }
                }
                else
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now!", styleCaption);
                        showCostValue("Total Mission Payout:", currentMission.reward * PayoutLeveles.TechPayout, styleValueGreen);
                        showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now: HardCore Mode!", styleCaption);
                        showCostValue("Total Mission Payout:", (currentMission.reward * PayoutLeveles.TechPayout) * .60, styleValueGreen);
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
                EditorLogic.fetch.Lock(true, true, true, ".zz9812.");
            }
        }

        private void destroyFileBrowser()
        {
            fileBrowser = null;

            if (EditorLogic.fetch != null)
            {
                EditorLogic.fetch.Unlock(".zz9812.");
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
                    EditorLogic.fetch.Lock(true, true, true, ".zz9812.");
                }
                else
                {
                    EditorLogic.fetch.Unlock(".zz9812.");
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


