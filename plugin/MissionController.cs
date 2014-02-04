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
    [KSPAddonFixed(KSPAddon.Startup.SpaceCentre, true, typeof(MissionController))]

    public partial class MissionController : MonoBehaviour
    {
        public bool recycled = false;
        //private bool drawLandingArea = false;             

        private AssemblyName assemblyName;
        private String versionCode;
        private DateTime buildDateTime;
        private String mainWindowTitle;

        public static string root = KSPUtil.ApplicationRootPath.Replace("\\", "/");
        public static string pluginFolder = root + "GameData/MissionController/";
        public static string missionFolder = pluginFolder + "Plugins/PluginData/MissionController/";

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
            get{return Manager.instance;}
        }

        private Settings settings
        {
            get{return SettingsManager.Manager.getSettings();}
        }
        

        private List<MissionGoal> hiddenGoals = new List<MissionGoal>();

        private Rect settingsWindowPosition;
        private Rect packageWindowPosition;
        private Rect contractWindowPosition;
        private Rect financeWindowPosition;
        private Rect researchtreewinpostion;
        private Rect missionLogBookPostion;
        private Rect kerbalLogBookHirePostion;
        private Rect shipLogBook;
        private Rect ContractWindowStatus;
        private Rect userContractWindowStatus;
               
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
        private IButton ContractSelect;

        private bool showSettingsWindow = false;
        private bool showMissionPackageBrowser = false;
        private bool showContractSelection = false;
        private bool showFinanceWindow = false;
        private bool showRecycleWindow = false;
        private bool showResearchTreeWindow = false;
        private bool showRandomWindow = false;
        private bool showRevertWindow = false;
        private bool showconstructionwindow = false;
        private bool showbudgetamountwindow = false;
        private bool hideMCtoolbarsviews = true;
        private bool showVabShipWindow = false;
        private bool showMissionStatusWindow = false;
        private bool showContractStatusWindow = false;
        private bool showKerbalLogbookHire = false;
        private bool showMissionLogbookWindow = false;
        private bool showShipLogBookWindow = false;
        private bool showUserContractWindowStatus = false;
               
        public string recycledName = "";
        public string recycledDesc = "";
        public int recycledCost = 0;
       
        private FileBrowser fileBrowser = null;
        private Mission currentMission = null;
        private MissionPackage currentPackage = null;

        private Vector2 scrollPosition = new Vector2(0, 0);
        private Vector2 scrollPositionship = new Vector2(0, 0);
        private Vector2 scrollPositionMission = new Vector2(0, 0);
        private Vector2 scrollPositionShip = new Vector2(0, 0);
        private Vector2 scrollPositionHire = new Vector2(0, 0);
        private Vector2 scrollPosition22 = new Vector2(0, 0);
        private GUIStyle styleValueRedBold, styleValueRed, styleValueYellow, styleValueGreen, styleText, styleCaption, styleValueGreenBold;       
        private GUIStyle styleButton, styleButtonYellow, styleGreenButton, styleRedButton, styleGreenButtonCenter, styleRedButtonCenter;
        public GUIStyle StyleBoxGreen, StyleBoxYellow, StyleBoxWhite;
        private GUIStyle styleValueName, styleWarning,styleIcon,styleButtonWordWrap;        

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

            StyleBoxGreen = new GUIStyle(HighLogic.Skin.box);
            StyleBoxGreen.normal.textColor = Color.green;
            StyleBoxGreen.fontStyle = FontStyle.Bold;
            StyleBoxGreen.alignment = TextAnchor.MiddleCenter;
            StyleBoxGreen.wordWrap = true;

            StyleBoxYellow = new GUIStyle(HighLogic.Skin.box);
            StyleBoxYellow.normal.textColor = Color.yellow;
            StyleBoxYellow.fontStyle = FontStyle.Bold;
            StyleBoxYellow.alignment = TextAnchor.MiddleCenter;
            StyleBoxYellow.wordWrap = true;

            StyleBoxWhite = new GUIStyle(HighLogic.Skin.box);
            StyleBoxWhite.normal.textColor = Color.white;
            StyleBoxWhite.fontStyle = FontStyle.Bold;
            StyleBoxWhite.alignment = TextAnchor.MiddleLeft;
            StyleBoxWhite.wordWrap = true;

            styleIcon = new GUIStyle();

        }        

        public void Start()
        {
            manager.findVeselWithRepairPart();
            print("Mission Controller Loaded");
            manager.loadProgram(HighLogic.CurrentGame.Title);
            LoadDictionary();
            
            
            
            button = ToolbarManager.Instance.add("MC1", "Settings1");
            button.TexturePath = "MissionController/icons/settings";
            button.ToolTip = "MCE Settings";
            button.OnClick += (e) =>
            {
                showSettingsWindow = !showSettingsWindow;
            };

            BudgetDisplay = ToolbarManager.Instance.add("MC1", "money1");
            BudgetDisplay.TexturePath = "MissionController/icons/money";
            BudgetDisplay.ToolTip = "MCE Current Budget";
            BudgetDisplay.OnClick += (e) =>
            {
                showFinanceWindow = !showFinanceWindow;
            };

            MissionSelect = ToolbarManager.Instance.add("MC1", "missionsel1");
            MissionSelect.TexturePath = "MissionController/icons/mission";
            MissionSelect.ToolTip = "MCE Select Current Mission";
            MissionSelect.OnClick += (e) =>
             {
                 showMissionStatusWindow = !showMissionStatusWindow;
             };

            ContractSelect = ToolbarManager.Instance.add("MC1", "contractsel1");
            ContractSelect.TexturePath = "MissionController/icons/contract";
            ContractSelect.ToolTip = "Takes You To Contract Selection Screen";
            ContractSelect.OnClick += (e) =>
            {
                showContractStatusWindow = !showContractStatusWindow;
            };

            VabShipSelect = ToolbarManager.Instance.add("MC1", "ship1");
            VabShipSelect.TexturePath = "MissionController/icons/blueprints";
            VabShipSelect.ToolTip = "MCE Ship Value BreakDown";
            VabShipSelect.Visibility = new GameScenesVisibility(GameScenes.EDITOR,GameScenes.SPH,GameScenes.FLIGHT);
            VabShipSelect.OnClick += (e) =>
            {
                showVabShipWindow = !showVabShipWindow;
            };

            ScienceResearch = ToolbarManager.Instance.add("MC1", "ship3");
            ScienceResearch.TexturePath = "MissionController/icons/research";
            ScienceResearch.ToolTip = "MCE Research Window";
            ScienceResearch.OnClick += (e) =>
            {
                showResearchTreeWindow = !showResearchTreeWindow;
            };

            RevertSelect = ToolbarManager.Instance.add("MC1", "ship2");
            RevertSelect.TexturePath = "MissionController/icons/revert";
            RevertSelect.ToolTip = "MCE Revert To VAB Cost 1000 Credits";
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
            Hidetoolbars.ToolTip = "Hide MCE Display Windows";
            Hidetoolbars.OnClick += (e) =>
            {
                hideMCtoolbarsviews = !hideMCtoolbarsviews;
            };

            GUILoad();
            manager.SetClockCountdown();

        }

        void OnLevelWasLoaded()
        {   
            SettingsManager.Manager.saveSettings();
            FuelMode.fuelinit(manager.GetFuels);
            ConstructionMode.constructinit(manager.GetConstruction);
            PayoutLeveles.payoutlevels(manager.GetCurrentPayoutLevel);
            FinanceMode fn = new FinanceMode();
            fn.checkloans();
            manager.isKerbalHired();
            GUISave();
            manager.checkClockTiime();
            manager.findVeselWithRepairPart();
            repairStation.repair = false;
            repairStation.dooropen = false;
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

        public void OnDestroy()
        {
            button.Destroy();
            BudgetDisplay.Destroy();
            MissionSelect.Destroy();
            VabShipSelect.Destroy();
            RevertSelect.Destroy();
            ScienceResearch.Destroy();
            Hidetoolbars.Destroy();
        }
        

        public void GUILoad()
        {
            KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<MissionController>();
            configfile.load();

            settingsWindowPosition = configfile.GetValue<Rect>("settingsWindowPostion");
            financeWindowPosition = configfile.GetValue<Rect>("finanaceWindowPostion");
            researchtreewinpostion = configfile.GetValue<Rect>("researchtreewinpostion");
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
            missionLogBookPostion = configfile.GetValue<Rect>("missionLogBookPostion");
            kerbalLogBookHirePostion = configfile.GetValue<Rect>("kerbalLogBookHirePostion");
            shipLogBook = configfile.GetValue<Rect>("shipLogBook");
        }

         public void GUISave()
        {
             KSP.IO.PluginConfiguration configfile = KSP.IO.PluginConfiguration.CreateForType<MissionController>();
 
             
            configfile.SetValue("settingsWindowPostion", settingsWindowPosition);
            configfile.SetValue("finanaceWindowPostion", financeWindowPosition);
            configfile.SetValue("researchtreewinpostion", researchtreewinpostion);
            configfile.SetValue("packageWindowPostion", packageWindowPosition);
            configfile.SetValue("vabBudgetWin", VabBudgetWin);
            configfile.SetValue("vabBudgetWin1", VabBudgetWin1);
            configfile.SetValue("vabBudgetWin2", VabBudgetWin2);
            configfile.SetValue("vabBudgetWin3", VabBudgetWin3);
            configfile.SetValue("VabShipBuildList", VabShipBuildList);
            configfile.SetValue("VabShipBuildList1", VabShipBuildList1);
            configfile.SetValue("VabShipBuildList2", VabShipBuildList2);
            configfile.SetValue("VabShipWindow", VabShipWindow);
            configfile.SetValue("missionLogBookPostion", missionLogBookPostion);
            configfile.SetValue("MissionWindowStatus", MissionWindowStatus);
            configfile.SetValue("kerbalLogBookHirePostion", kerbalLogBookHirePostion);
            configfile.SetValue("shipLogBook", shipLogBook);
 
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
                                manager.totalReward(daysDiff * s.passiveReward);
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
            {
                canRecycle = true; // reenable once exit flight               
            }           
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

            //if (drawLandingArea)
            //{
            //    CelestialBody kerbin = FlightGlobals.Bodies.Find(b => b.bodyName.Equals("Kerbin"));
            //    if (kerbin != null)
            //    {
            //        //                    GLUtils.drawLandingArea (kerbin, 80, 90, -170.0, 170.0, new Color(1.0f, 0.0f, 0.0f, 0.5f));
            //    }
            //}

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

            if (HighLogic.LoadedScene.Equals(GameScenes.EDITOR) || HighLogic.LoadedScene.Equals(GameScenes.SPH))
            {
                showconstructionwindow = true;
            }

            if (showconstructionwindow && hideMCtoolbarsviews)
            {
                VesselResources res = new VesselResources(activeVessel);
                if (HighLogic.LoadedScene.Equals(GameScenes.EDITOR))
                {
                    VabShipBuildList = GUILayout.Window(898992, VabShipBuildList, drawconstructioncostwindow, "Ship Value: " + CurrencySuffix + res.sum().ToString("N2"), (res.sum() > manager.budget ? styleRedButtonCenter : styleGreenButtonCenter), GUILayout.MinHeight(20), GUILayout.MinWidth(175));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.FLIGHT))
                {
                    VabShipBuildList1 = GUILayout.Window(898991, VabShipBuildList1, drawconstructioncostwindow, "SV : " + CurrencySuffix + res.sum().ToString("N2"), (res.sum() > manager.budget ? styleRedButtonCenter : styleGreenButtonCenter), GUILayout.MinHeight(20), GUILayout.MinWidth(125));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.SPH))
                {
                    VabShipBuildList2 = GUILayout.Window(898990, VabShipBuildList2, drawconstructioncostwindow, "Ship Value: " + CurrencySuffix + res.sum().ToString("N2"), (res.sum() > manager.budget ? styleRedButtonCenter : styleGreenButtonCenter), GUILayout.MinHeight(20), GUILayout.MinWidth(175));
                }
            }

            if (showbudgetamountwindow && hideMCtoolbarsviews)
            {
                if (HighLogic.LoadedScene.Equals(GameScenes.EDITOR))
                {
                    VabBudgetWin = GUILayout.Window(898993, VabBudgetWin, drawbudgetwindow, "Current Budget: " + CurrencySuffix + manager.budget.ToString("N2"), styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(200));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER))
                {
                    VabBudgetWin1 = GUILayout.Window(898994, VabBudgetWin1, drawbudgetwindow, "Current Budget: " + CurrencySuffix + manager.budget.ToString("N2"), styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(200));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.FLIGHT))
                {
                    VabBudgetWin2 = GUILayout.Window(898995, VabBudgetWin2, drawbudgetwindow, "CB " + CurrencySuffix + manager.budget.ToString("N2"), styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(125));
                }
                if (HighLogic.LoadedScene.Equals(GameScenes.SPH))
                {
                    VabBudgetWin3 = GUILayout.Window(898996, VabBudgetWin3, drawbudgetwindow, "Current Budget: " + CurrencySuffix + manager.budget.ToString("N2"), styleGreenButtonCenter, GUILayout.MinHeight(20), GUILayout.MinWidth(200));
                }
            }

            if (showUserContractWindowStatus && hideMCtoolbarsviews)
            {
                userContractWindowStatus = GUILayout.Window(912311, userContractWindowStatus, drawUserContractWindow, "Player Custom Contracts", GUILayout.MinHeight(900), GUILayout.MinWidth(450));
            }

            if (showVabShipWindow && hideMCtoolbarsviews)
            {
                VabShipWindow = GUILayout.Window(898989, VabShipWindow, drawVabShipWindow, "Ship Breakdown List",GUILayout.MinHeight(400), GUILayout.MinWidth(300));
            }

            if (showMissionStatusWindow && hideMCtoolbarsviews)
            {
                MissionWindowStatus = GUILayout.Window(898990, MissionWindowStatus, drawMissionInfoWindow, "Current Mission Window",GUILayout.MinHeight(450), GUILayout.MinWidth(350));
            }

            if (showContractStatusWindow && hideMCtoolbarsviews)
            {
                ContractWindowStatus = GUILayout.Window(888991, ContractWindowStatus, drawContractInfoWindow, "Available Contracts", GUILayout.MinHeight(450), GUILayout.MinWidth(350));
            }

            if (showSettingsWindow && hideMCtoolbarsviews)
            {
                settingsWindowPosition = GUILayout.Window(98763, settingsWindowPosition, drawSettingsWindow, "Settings", GUILayout.MinHeight(225), GUILayout.MinWidth(150));
            }

            if (showMissionPackageBrowser && hideMCtoolbarsviews)
            {
                packageWindowPosition = GUILayout.Window(98762, packageWindowPosition, drawPackageWindow, currentPackage.name, GUILayout.MinHeight(750), GUILayout.MinWidth(1000));
            }

            if (showContractSelection && hideMCtoolbarsviews)
            {
                contractWindowPosition = GUILayout.Window(234321, contractWindowPosition, drawContractsWindow, currentPackage.name, GUILayout.MinHeight(700), GUILayout.MinWidth(520));
            }

            if (showFinanceWindow && hideMCtoolbarsviews)
            {
                financeWindowPosition = GUILayout.Window(98761, financeWindowPosition, drawFinaceWindow, "Finance Window", GUILayout.MinHeight(350), GUILayout.MinWidth(300));
            }           

            if (showRecycleWindow && hideMCtoolbarsviews)
            {
                GUILayout.Window(98766, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 600, 400), drawRecycleWindow, "Recycle Window");
            }

            if (manager.showKerbalHireWindow && hideMCtoolbarsviews)
            {
                GUILayout.Window(987667, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 425, 150), drawKerbalHireWindow, "Recently Hired Kerbals");
            }

            if (showRandomWindow && hideMCtoolbarsviews)
            {
                GUILayout.Window(98866, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 100), drawRandomWindow, "Event Window");
            }
            if (showRevertWindow && hideMCtoolbarsviews)
            {
                GUILayout.Window(99746, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 100), drawRevertWindow, "Revert Space Program And KSP Window");
            }

            if (showResearchTreeWindow && hideMCtoolbarsviews)
            {
                researchtreewinpostion = GUILayout.Window(98760, researchtreewinpostion, drawResearchTree, "Research Window", GUILayout.MinHeight(350), GUILayout.MinWidth(500));
            }

            if (showKerbalLogbookHire && hideMCtoolbarsviews)
            {
                kerbalLogBookHirePostion = GUILayout.Window(98888, kerbalLogBookHirePostion, drawKerbalLogBookHire, "Kerbal Hired Log Book", GUILayout.MinHeight(350), GUILayout.MinWidth(540));
            }

            if (showMissionLogbookWindow && hideMCtoolbarsviews)
            {
                missionLogBookPostion = GUILayout.Window(988889, missionLogBookPostion, drawmMissionLogBook, "Mission Log Book", GUILayout.MinHeight(500), GUILayout.MinWidth(1045));
            }

            if (showShipLogBookWindow && hideMCtoolbarsviews)
            {
                shipLogBook = GUILayout.Window(988889, shipLogBook, drawShipLogBook, "Ship Log Book ", GUILayout.MinHeight(500), GUILayout.MinWidth(960));
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

        private void drawmMissionLogBook(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPositionMission = GUILayout.BeginScrollView(scrollPositionMission, GUILayout.Width(1035));

            GUILayout.BeginHorizontal();
            GUILayout.Box("Mission Name", StyleBoxYellow, GUILayout.Width(425));
            GUILayout.Box("Date Finished", StyleBoxYellow, GUILayout.Width(160));
            GUILayout.Box("Vessel Name", StyleBoxYellow, GUILayout.Width(275));
            GUILayout.Box("Mission Payout", StyleBoxYellow,GUILayout.Width(140));
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            manager.displayEndedMissionList();           
            
            GUILayout.EndScrollView();
            if (GUILayout.Button("Exit Mission Log Book"))
            {
                showMissionLogbookWindow = false;
            }
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }

        private void drawShipLogBook(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPositionShip = GUILayout.BeginScrollView(scrollPositionShip, GUILayout.Width(940));

            GUILayout.BeginHorizontal();
            GUILayout.Box("Vessel Name", StyleBoxYellow, GUILayout.Width(250));
            GUILayout.Box("Vessels Mission", StyleBoxYellow, GUILayout.Width(400));
            GUILayout.Box("Number Of Crew", StyleBoxYellow, GUILayout.Width(125));
            GUILayout.Box("Vessel Cost", StyleBoxYellow, GUILayout.Width(125));
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            manager.displayShipList();


            GUILayout.EndScrollView();
            if (GUILayout.Button("Exit Ship Log Book"))
            {
                showShipLogBookWindow = false;
            }
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        private void drawKerbalLogBookHire(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPositionHire = GUILayout.BeginScrollView(scrollPositionHire, GUILayout.Width(520));
            
            GUILayout.BeginHorizontal();
            GUILayout.Box("Hired Name",StyleBoxYellow, GUILayout.Width(200));
            GUILayout.Box("Date Hired",StyleBoxYellow, GUILayout.Width(150));
            GUILayout.Box("Status", StyleBoxYellow, GUILayout.Width(125));
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            manager.displayKerbalList();            
            
            GUILayout.EndScrollView();
            if (GUILayout.Button("Exit Kerbal Hire Log"))
            {
                showKerbalLogbookHire = false;
            }
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        
        private void drawKerbalHireWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Hired Name",StyleBoxYellow, GUILayout.Width(200));
            GUILayout.Box("Hired Cost", StyleBoxYellow, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.Space(15);

            manager.displayCurrentHiredList();
            
            if (GUILayout.Button("OK", styleButtonWordWrap))
            {
                manager.showKerbalHireWindow = false;
                manager.currentHires.Clear();
                manager.saveProgramBackup();
            }

            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        
        private void drawRecycleWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            if ((manager.ResearchRecycle || HighLogic.CurrentGame.Mode != Game.Modes.CAREER))
            {

                GUILayout.BeginHorizontal();
                GUILayout.Box("Vessel Name", StyleBoxYellow, GUILayout.Width(350), GUILayout.Height(30));
                GUILayout.Box("Cost Returned", StyleBoxYellow, GUILayout.Width(100), GUILayout.Height(30));
                GUILayout.Box("Description", StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Box(recycledName, GUILayout.Width(350), GUILayout.Height(30));
                GUILayout.Box("$ " + recycledCost.ToString("N2"), GUILayout.Width(100), GUILayout.Height(30));
                GUILayout.Box("  (" + recycledDesc + ")", GUILayout.Width(300), GUILayout.Height(30));
                GUILayout.EndHorizontal();
            }           

            if (GUILayout.Button("OK", styleButtonWordWrap))
            {
                showRecycleWindow = false;
            }

            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }

        private void drawRevertWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            if (FlightDriver.CanRevertToPrelaunch && activeVessel.situation != Vessel.Situations.PRELAUNCH)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Pressing Ok Will Revert Both Mission controller", styleCaption);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("And KSP To PreLaunch Conditions In The Editor", styleCaption);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("You will Be charged 1000 Credits For Doing this", styleValueGreenBold);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Do You Want To Continue?", styleCaption);
                GUILayout.EndHorizontal();
               
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Revert To VAB", styleButtonWordWrap, GUILayout.Width(100)))
                {
                    LaunchGoal LG = new LaunchGoal();
                    manager.loadProgramBackup(HighLogic.CurrentGame.Title);
                    FlightDriver.RevertToPrelaunch(GameScenes.EDITOR);                    
                    manager.costs(1000);
                    manager.saveProgramBackup();

                    showRevertWindow = false;
                }
                if (GUILayout.Button("Revert To SPH", styleButtonWordWrap, GUILayout.Width(100)))
                {
                    LaunchGoal LG = new LaunchGoal();
                    manager.loadProgramBackup(HighLogic.CurrentGame.Title);
                    FlightDriver.RevertToPrelaunch(GameScenes.SPH);
                    manager.costs(1000);
                    manager.saveProgramBackup();

                    showRevertWindow = false;
                }

                if (GUILayout.Button("Do Not Revert", styleButtonWordWrap, GUILayout.Width(200)))
                {
                    showRevertWindow = false;
                }
                GUILayout.EndHorizontal();
            }
            if (activeVessel.situation == Vessel.Situations.PRELAUNCH)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Label("You are in PreLaunch Conditions you",styleCaption);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Can Revert For Free", styleValueGreenBold);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Do You Want To Continue?", styleCaption);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Revert To VAB", styleButtonWordWrap, GUILayout.Width(100)))
                {
                    manager.loadProgramBackup(HighLogic.CurrentGame.Title);
                    FlightDriver.RevertToPrelaunch(GameScenes.EDITOR);
                    showRevertWindow = false;
                }
                if (GUILayout.Button("Revert To SPH", styleButtonWordWrap, GUILayout.Width(100)))
                {
                    manager.loadProgramBackup(HighLogic.CurrentGame.Title);
                    FlightDriver.RevertToPrelaunch(GameScenes.SPH);
                    showRevertWindow = false;
                }

                if (GUILayout.Button("Do Not Revert", styleButtonWordWrap, GUILayout.Width(200)))
                {
                    showRevertWindow = false;
                }
                GUILayout.EndHorizontal();
            }
            if (!FlightDriver.CanRevertToPrelaunch && activeVessel.situation != Vessel.Situations.PRELAUNCH)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("You Can No Longer Revert With Vessel", styleCaption);
                GUILayout.EndHorizontal();
              
                if (GUILayout.Button("Do Not Revert", styleButtonWordWrap, GUILayout.Width(200)))
                {
                    showRevertWindow = false;
                }


            }

            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }

        private void drawRandomWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            Status status = calculateStatus(currentMission, true, activeVessel);
            Mission mission = new Mission();
            

            if (currentMission.IsContract != true)
            {
                double rewardFinanced = (currentMission.reward * FinanceMode.currentloan) * PayoutLeveles.TechPayout;
                double rewardFinancedHard = (currentMission.reward * FinanceMode.currentloan * PayoutLeveles.TechPayout) * .60;
                double rewardnormal = currentMission.reward * PayoutLeveles.TechPayout;
                double rewardHard = (currentMission.reward * PayoutLeveles.TechPayout) * .60;

                GUILayout.Box("Current Mission: " + mission.name, StyleBoxYellow, GUILayout.Width(600),GUILayout.Height(30));
                if (manager.budget < 0)
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Box("All goals accomplished. Deducted For Loans!", StyleBoxWhite, GUILayout.Width(600),GUILayout.Height(30));
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Total Mission Payout: " + rewardFinanced.ToString("N2"), StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            GUILayout.Box("Total Science Paid: " + currentMission.scienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Box("All Goals accomplished. Hardcore and Deducted Loans", StyleBoxWhite, GUILayout.Width(600),GUILayout.Height(30)); // .75 * .6 = .45
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Total Mission Payout: " + rewardFinancedHard.ToString("N2"), StyleBoxYellow, GUILayout.Width(300),GUILayout.Height(30));
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            GUILayout.Box("Total Science Paid: " + currentMission.scienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                else
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Box("All goals accomplished. you can finish the mission now!", StyleBoxWhite, GUILayout.Width(600),GUILayout.Height(30));
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Total Mission Payout: " + rewardnormal.ToString("N2"), StyleBoxYellow, GUILayout.Width(300),GUILayout.Height(30));
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            GUILayout.Box("Total Science Paid: " + currentMission.scienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Box("All goals accomplished. you can finish the mission now: HardCore Mode 40 % Reduction!", StyleBoxWhite, GUILayout.Width(600));
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Total Mission Payout: " + rewardHard.ToString("N2"), StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        {
                            GUILayout.Box("Total Science Paid: " + currentMission.scienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
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
            }
            else
            {
                
                double comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString), "payout", 1.0);
                double compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString), "science", 1.0);
               
                if (currentMission.CompanyOrder == 2)
                {
                    comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString2), "payout", 1.0);
                    compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString2), "science", 1.0);
                }
                if (currentMission.CompanyOrder == 3)
                {
                    comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString3), "payout", 1.0);
                    compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString3), "science", 1.0);
                }
                if (currentMission.CompanyOrder == 4)
                {
                    comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString4), "payout", 1.0);
                    compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString4), "science", 1.0);
                }                               

                double rewardFinanced = currentMission.reward * FinanceMode.currentloan * comppayout * PayoutLeveles.TechPayout;
                double rewardFinancedHard = currentMission.reward * FinanceMode.currentloan * comppayout * PayoutLeveles.TechPayout * .60;
                double rewardnormal = currentMission.reward * PayoutLeveles.TechPayout * comppayout;
                double rewardHard = currentMission.reward * PayoutLeveles.TechPayout * comppayout * .60;
                double ScienceReward = currentMission.scienceReward * compscience;

                GUILayout.Box("Current Contract: " + mission.name, StyleBoxYellow, GUILayout.Width(600),GUILayout.Height(30));
                if (manager.budget < 0)
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Box("Contract accomplished. Deducted For Loans!", StyleBoxWhite, GUILayout.Width(600),GUILayout.Height(30));
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Contract Payout: " + rewardFinanced.ToString("N2"), StyleBoxYellow, GUILayout.Width(300),GUILayout.Height(30));
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            GUILayout.Box("Science Learned: " + ScienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Box("Contract accomplished. Hardcore and Deducted Loans", StyleBoxWhite, GUILayout.Width(600),GUILayout.Height(30)); // .75 * .6 = .45
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Contract Payout: " + rewardFinancedHard.ToString("N2"), StyleBoxYellow, GUILayout.Width(300),GUILayout.Height(30));
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            GUILayout.Box("Science Learned: " + ScienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                else
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Box("Contract accomplished. you can finish the mission now!", StyleBoxWhite, GUILayout.Width(600),GUILayout.Height(30));
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Contract Payout: " + rewardnormal.ToString("N2"), StyleBoxYellow, GUILayout.Width(300),GUILayout.Height(30));
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            GUILayout.Box("Science Learned: " + ScienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Box("Contract accomplished. you can finish the mission now: HardCore Mode 40 % Reduction!", StyleBoxWhite, GUILayout.Width(600),GUILayout.Height(30));
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Contract Payout:" + rewardHard.ToString("N2"), StyleBoxYellow, GUILayout.Width(300),GUILayout.Height(30));
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            GUILayout.Box("Scinece Learned: " + ScienceReward, StyleBoxYellow, GUILayout.Width(300), GUILayout.Height(30));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Finish And Save The Mission Results", styleButtonWordWrap, GUILayout.Width(275)))
                {
                    manager.finishMission(currentMission, activeVessel, status.events);
                    hiddenGoals = new List<MissionGoal>();
                    currentMission = null;
                    showRandomWindow = false;
                    manager.SetCurrentContract(0);                    
                }
                if (GUILayout.Button("X", styleButtonWordWrap, GUILayout.Width(25)))
                {
                    currentMission = null;
                    showRandomWindow = false;
                }
                GUILayout.EndHorizontal();
            }                                          
            
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }       

        private void drawVabShipWindow(int id)
        {            
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPositionship = GUILayout.BeginScrollView(scrollPositionship, GUILayout.Width(300));
            GUILayout.Space(10);

            VesselResources res = new VesselResources(activeVessel);

            GUILayout.BeginHorizontal();
            GUILayout.Box("Parts", StyleBoxYellow, GUILayout.Width(150));
            GUILayout.Box("Cost", StyleBoxYellow, GUILayout.Width(100));
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            
            if (res.pod() > (0)) 
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Command Sections", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.pod().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            if (res.ctrl() > (0))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Control & SAS", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.ctrl().ToString("N2"), GUILayout.Width(100)); 
                GUILayout.EndHorizontal();
            } 
            if (res.util() > (0))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Utility Parts", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.util().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            if (res.sci() > (0))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Science Parts", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.sci().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            if (res.engine() > (0)) 
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Engines", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.engine().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            if (res.tank() > (0))
            { 
                GUILayout.BeginHorizontal();
                GUILayout.Box("Fuel Tank", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.tank().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            if (res.stru() > (0))
            { 
                GUILayout.BeginHorizontal();
                GUILayout.Box("Structural", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.stru().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            if (res.aero() > (0))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("Aerodynamic", StyleBoxWhite, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.aero().ToString("N2"), GUILayout.Width(100)); 
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
            }
            // pull from resources
            if (res.resources.Count > 0)
            {
                List<string> resInVessel = res.resources.Keys.ToList();
                resInVessel.Sort();
                foreach (string r in resInVessel)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Box(r, StyleBoxWhite, GUILayout.Width(150));
                    GUILayout.Box(CurrencySuffix + Math.Round(res.resources[r], 0), GUILayout.Width(100));
                    GUILayout.EndHorizontal();
                }
            }
            if (res.wet() > (0))
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Box("(Total Cost Of Fuels)", StyleBoxYellow, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.wet().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
            if (res.dry() > (0)) 
            { 
                GUILayout.BeginHorizontal();
                GUILayout.Box("(Total Cost Of Parts)", StyleBoxYellow, GUILayout.Width(150));
                GUILayout.Box(CurrencySuffix + res.dry().ToString("N2"), GUILayout.Width(100));
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
            }           
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Cost Vessel", StyleBoxYellow, GUILayout.Width(150));
            GUILayout.Box(CurrencySuffix + res.sum().ToString("N2"), StyleBoxGreen, GUILayout.Width(100));
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Exit Window", styleButtonWordWrap, GUILayout.Height(20)))
            {
                showVabShipWindow = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }

        private void drawMissionInfoWindow(int id)
        {
            if (showMissionStatusWindow == true)
                showContractStatusWindow = false;

            Status status = calculateStatus(currentMission, true, activeVessel);
          
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(350));
            GUILayout.Space(20);

            if (status.isClientControlled)
            {
                MissionStatus s = manager.getClientControlledMission(activeVessel);
                GUILayout.Label("This vessel is controlled by a client. Do not destroy this vessel! Fine: " + CurrencySuffix + s.punishment, styleWarning);
                GUILayout.Label("End of life in " + MathTools.formatTime(s.endOfLife - Planetarium.GetUniversalTime()));
            }
            else if (status.isOnPassiveMission)
            {
                MissionStatus s = manager.getPassiveMission(activeVessel);
                GUILayout.Label("This vessel is involved in a passive mission. Do not destroy this vessel! Fine: " + CurrencySuffix + s.punishment, styleWarning);
                GUILayout.Label("End of life in " + MathTools.formatTime(s.endOfLife - Planetarium.GetUniversalTime()));
            }
            if (manager.isVesselFlagged(activeVessel))
            {
                GUILayout.Label("Warning Vessel Is Flaged and Can't Do Missions", styleValueRedBold);
                GUILayout.Label("Vessel Most Likely Launched In Disabled Mode", styleValueRedBold);
            }

            if (settings.disablePlugin == true)
            {
                GUILayout.Label("Plugin Disabled", styleValueRed);
            }           

            if (currentMission != null && currentMission.IsContract != true)
            {
                drawMission(currentMission, status);     
            }
            GUILayout.EndScrollView();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Package", styleButtonWordWrap, GUILayout.Width(100)))
            {
                createFileBrowser("Contract", selectMissionPackage);
            }
            
            if (GUILayout.Button("X", styleButtonWordWrap, GUILayout.Width(25)))
            {
                showMissionStatusWindow = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }

        private void drawContractInfoWindow(int id)
        {
            if (showContractStatusWindow == true)
                showMissionStatusWindow = false;
            
            Status status = calculateStatus(currentMission, true, activeVessel);

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            scrollPosition22 = GUILayout.BeginScrollView(scrollPosition22, GUILayout.Width(350));
            GUILayout.Space(20);          

            if (settings.disablePlugin == true)
            {
                GUILayout.Label("Plugin Disabled", styleValueRed);
            }

            if (currentMission != null && currentMission.IsContract != false)
            {
                drawContracts(currentMission, status);
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();          

            if (GUILayout.Button("Contracts", styleButtonWordWrap, GUILayout.Width(100)))
            {
                selectContracts("MCContracts.cfg");
            }

            if (GUILayout.Button("User Contrancts", styleButtonWordWrap, GUILayout.Width(150)))
            {
                showUserContractWindowStatus = !showUserContractWindowStatus;
                currentPreviewMission3 = null;
            }

            if (GUILayout.Button("X", styleButtonWordWrap, GUILayout.Width(25)))
            {
                showContractStatusWindow = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }

        private void drawbudgetwindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        private void drawconstructioncostwindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
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

        private void selectContracts(String file)
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
                showContractSelection = !showContractSelection;
            }
            currentSort = (currentPackage.ownOrder ? SortBy.PACKAGE_ORDER : SortBy.NAME);
        }

        private void selectUserContract(string file)
        {
            if (file == null)
            {
                return;
            }
            currentMission = manager.loadContractMission(file);
            currentPreviewMission3 = null;
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
            { GUILayout.Label(CurrencySuffix + mission.reward * PayoutLeveles.TechPayout, styleValueYellow); }
            if (settings.gameMode == 1)
            { GUILayout.Label(CurrencySuffix + mission.reward * PayoutLeveles.TechPayout * .60, styleValueYellow); }
            GUILayout.EndHorizontal();

            if (mission.scienceReward != 0 && HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
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
                GUILayout.Label(CurrencySuffix + (mission.passiveReward), styleValueGreen);
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
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All Goals accomplished. Finish The Mission. Deducted for loans and HardCore mode"); // .75 * .6 = .45
                        showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan * PayoutLeveles.TechPayout) * .60, styleValueGreen);
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
                    }
                }
                else
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now!", styleCaption);
                        showCostValue("Total Mission Payout:", currentMission.reward * PayoutLeveles.TechPayout, styleValueGreen);
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now: HardCore Mode!", styleCaption);
                        showCostValue("Total Mission Payout:", (currentMission.reward * PayoutLeveles.TechPayout) * .60, styleValueGreen);
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
                    }
                }
            }
        }

        private void drawContracts(Mission mission, Status s)
        {

            string compName = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString), "name", "Default");
            double comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString), "payout", 1.0);
            double compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString), "science", 1.0);

            if (mission.CompanyOrder == 2)
            {
                compName = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString2), "name", "Default");
                comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString2), "payout", 1.0);
                compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString2), "science", 1.0);
            }
            if (mission.CompanyOrder == 3)
            {
                compName = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString3), "name", "Default");
                comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString3), "payout", 1.0);
                compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString3), "science", 1.0);
            }
            if (mission.CompanyOrder == 4)
            {
                compName = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString4), "name", "Default");
                comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString4), "payout", 1.0);
                compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(manager.GetCompanyInfoString4), "science", 1.0);
            }

            //GUILayout.Label("pay" + comppayout);
            //GUILayout.Label("sci" + compscience);
            GUILayout.Label("Current Contract: ", styleValueGreenBold);
            GUILayout.Label(mission.name, styleText);
            if (mission.IsUserContract  != true)
            {
                GUILayout.Label("Company Name", styleValueGreenBold);
                GUILayout.Label(compName, styleText);
            }
            GUILayout.Label("Description: ", styleValueGreenBold);
            GUILayout.Label(mission.description, styleText);
            if (mission.IsUserContract != true)
            {
                GUILayout.Label("Contract Binding Terms If Fail", styleValueGreenBold);
                GUILayout.Label("Contract Payout + 10% (Note Not Yet Implemented - Malkuth)", styleText);
            }
            if (mission.vesselName != false)
            {
                GUILayout.Label("Vessel Name To Repair", styleValueGreenBold);
                GUILayout.Label(manager.GetShowVesselRepairName, styleText);
            }
            
            if (mission.IsUserContract != true)
            {
                GUILayout.BeginHorizontal();           
                GUILayout.Label(" Contract Payout: ", styleValueGreenBold);
                if (settings.gameMode == 0)
                { GUILayout.Label(CurrencySuffix + mission.reward * comppayout * PayoutLeveles.TechPayout, styleValueYellow); }
                if (settings.gameMode == 1)
                { GUILayout.Label(CurrencySuffix + mission.reward * comppayout * PayoutLeveles.TechPayout * .60, styleValueYellow); }
                GUILayout.EndHorizontal();
            }

            if (mission.scienceReward != 0 && HighLogic.CurrentGame.Mode == Game.Modes.CAREER && mission.IsUserContract != true)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Science Reward: ", styleValueGreenBold);
                GUILayout.Label(mission.scienceReward * compscience + " sp", styleValueYellow);
                GUILayout.EndHorizontal();
            }


            if (mission.clientControlled)
            {
                GUILayout.Label("At The End Of Contract All Company Assets Will Revert To Company Control", styleWarning);
            }

            drawContractsGoals(mission, s);

            if (s.missionIsFinishable)
            {
                showRandomWindow = true;
                if (manager.budget < 0)
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Label("All goals accomplished. You can finish the mission now! Deducted for loans!", styleCaption);
                        showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan) * PayoutLeveles.TechPayout, styleValueGreen);
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All Goals accomplished. Finish The Mission. Deducted for loans and HardCore mode"); // .75 * .6 = .45
                        showCostValue("Total Mission Payout:", (currentMission.reward * FinanceMode.currentloan * PayoutLeveles.TechPayout) * .60, styleValueGreen);
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
                    }
                }
                else
                {
                    if (settings.gameMode == 0)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now!", styleCaption);
                        showCostValue("Total Mission Payout:", currentMission.reward * PayoutLeveles.TechPayout, styleValueGreen);
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
                    }
                    if (settings.gameMode == 1)
                    {
                        GUILayout.Label("All goals accomplished. you can finish the mission now: HardCore Mode!", styleCaption);
                        showCostValue("Total Mission Payout:", (currentMission.reward * PayoutLeveles.TechPayout) * .60, styleValueGreen);
                        if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                        {
                            showCostValue("Total Science Paid: ", currentMission.scienceReward, styleValueGreen);
                        }
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

        private void drawContractsGoals(Mission mission, Status s)
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
                        GUILayout.Label((index++) + ". Contract goal (complete all):" + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }
                    else if (c is OrMissionGoal)
                    {
                        GUILayout.Label((index++) + ". Contract goal (complete any):" + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }
                    else if (c is OrMissionGoal)
                    {
                        GUILayout.Label((index++) + ". Contract goal (complete none):" + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }
                    else
                    {
                        GUILayout.Label((index++) + ". Contract goal: " + c.getType() + (c.optional ? " (optional)" : ""), styleValueGreenBold);
                    }

                    if (c.description.Length != 0)
                    {
                        GUILayout.Label("Contract Description: ", styleCaption);
                        GUILayout.Label(c.description, styleText);
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
            fileBrowser.hideFileExtensions = true;

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
            showStringValue(name, String.Format("{0:N0}{1}", CurrencySuffix, value), style);
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

        private const String CurrencySuffix = "$";
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


