#.50 Changes

1.	Lots of little bug fixes.
2.	New Random Contract System 
3.	New Custom Contract System
4.	MCE now has companies that give you Contracts and will pick up your custom contract
5.	Majirs CompatibilityChecker added.
6.	Reset Button added to Settings For GUI Widows.
7.	Reset Button added to Settings for Contract Time Reset.
8.	Missions and contracts now payout for Science correct (broke in older versions)
9.	Can now specify a biome for landing goal for mission makers (thanks jamis)
10.	Can now hire kerbals and they cost money when you do hire them.


#Version .40

1. Complete new UI change.  Itegrated ToolBar 1.3.0 into Mission Controller Extended.
2. New Windows and Every Scene has its own savable windows, icons.  Mission Controller will rember were you put them!  And ToolBar does its thing too!
3. No more main window, everything has been changed to make Mission Controller feel more like its part of Kerbal Space Program!
4. My own new Custom Icons to use with ToolBar 1.3.0
5. No More Flight Testing.  Now the mission controller revert button is the new flight test button?  Messed up? No problem REVERT.. It will cost you 1000 Credits though.
6. Don't worry if you need to revert the normal way you still can, just remember that if you launched already the Mission Controller Revert is the only guarateed way to save you MC info correct.
7. Fixed the Research Payouts not working correct with payments at end of missons.
8. Other bug fixes I can't rember after 3 weeks of editing.

ToolBar 1.3.0
At this time MCE uses ToolBar, doesn't really matter what version you use if it updates, it should be fine to update the toolbar mod byitself.  Its included in this download.
you can go to the ToolBar Thread at this address: http://forum.kerbalspaceprogram.com/threads/60863-0-23-0-Toolbar-1-3-0-Common-API-for-draggable-resizable-buttons-toolbar.

If for some reason you toolbar does not show up in game for MCE delete the toolbar-settings.dat in the GameData Folder.  I have it set for pretty high resolution and wasn't sure if they would go off screen with 
lower res rigs.

And if you can't see the mission controller new windows (ie they off screen) then delete the settings.cfg located in \GameData\MissionController\Plugins\PluginData\MissionController

#Changes in .20
1. 2 new MissionGoals UndockingGoal And RepairGoal

UndockingGoal Alows you to start the mission from a space station or other vessel you are Docked too.  

RepairGoal Adds a new part to MissionController.  Right now the part is just a place holder copy of the Vanilla SP-L 1x6 Photovoltaic Panels.  But it works out pretty good 

because it looks just like a small hatch.. Great for a repair door. :)  With RepairGoal you can put a part on any vessel and have the RepairGoal for that Vessel Set by a mission 

Designer. (later there might actually be a random mission for it).  The player launches to the vessel and HAS TO GO EVA and approach the vessel with the part on it.. After 

reaching a certain distance to the part.. The player can Right Click on it and select Repair.  If you wish as a Mission Designer you can add the TIME fuction to this and have a 

countdown to simulate the time needed to repair the vessel.. not to long though. :)

2. New UI options.  Now when you select missions you can filter out missions you can't do yet.  And missions that are complete and can't be redone.
3. DockingGoal has changed.. It has the same fuctions now as UnDocking.. These new fuctions have come at a price.  Because of the way KSP handles Vessel IDs.. And the 

way Mission Controller records these vessel IDs to show a player has finished a missionGoal and save it to file.. I had to take out the Vessel ID for both Undocking and 

Docking.  No big deal.. The only catch is that do not put Undocking and Docking into the same SubMissionGoal.. Or bad things will happen. (mission won't be able to 

complete)  So mission designers beware.. In fact I highly suggest you keep both Docking And Undocking in Seperate SINGLE GOALs... I have tested it and you can mix 1 with 

any other goals.. But you can't mix BOTH of them in same GOALS.
4. Nathan also fixed (he did the UI changes too) the cost of jets. 
5.New UI Icon For Docking and Undocking.. The Docking One was always there.. (not used?) and I made the new Undocking one. (don't laugh) ;)
6.Bunch of Code Changes and fixes..  And other stuff like that.  (mission controller now uses KSPADDON to start up.. Instead of the old way Nobody had set up.. Was a 

hacked way before KSPADDON)


#Changes in .19
1. Finally Mission Controller Has Saved Windows Postion.  Mission controller will finally remember your windows postions even after a restart.
2. Azadam was nice enough to update the Stock Missions Descriptions and other fixes to the Mission File.
3. Aontas added a new missions Goal LaunchGoal. Now using LaunchGoal you can launch from any Body. And have it a mission Goal.
4. New KerbalNauts Window. This is a work in progress but now you have a way of charging your space program for the Kerbals you hire.  At this time it is not connected with the HireButton in the AstroNaut complex (can’t seem to find a refrence to it). But you do have the choice to use the HireButton to charge your Space Program.  The price of HireKerbals is Editable from the New KerbalNauts Window.
5. Kerbal Insurance has been moved to KerbalNaut window.
6. Updated the DOCKING GOAL.  I did not change much but the change I did use now uses the actual in game DOCKING to tell when you do dock.  It still uses most of the legacy code though for the The Rest of the code.  Seems to work with the testing I have done.  Not sure if its better though.
7. The silly long messages for Reset Space Program And Rewind have been removed.
8. New Mission Controller InGame Icon has been added.
9. New Random Missions.  A new Pack has been added that takes advantage of the Random Missions Option in Mission Controller.  Every Mission in Random Missions has Random Mission Goals.. After you play the mission you can Use the NEW RANDOM MISSION Button in Mission Packages to get a new set of values for the random mission.  All are repeatable.

#Changes In .18.1
1. Fixed Kerbal Insurance not being Charged on launch. (thanks Nathan)
2. Fixed Some Decimal Issues and Rounded off the Orbits. (orbits Only)
3. Fixed the Civilian Space Program Missions Governments Contracts. Medium, heavy.  Passive Missions should now show up.
4. Increased the Payouts in a few of the Civilian Space Program Missions including the Government Ones.

#Changes in .18
1. Fixed the Recover and Recycle while in Testing Mode.. Should be all better now.. Now in Testing mode you can't Recover Recycle a whole vessel.. (can't recycle at all)


#Changes In .17

1. Hardcore Mode added. Its back.. Hard Mode for Mission controller. But instead of Making Parts more expensive Hardcore mode Reduces mission payouts by 40%
2. More Auto Recycle fixes.  You now get little messages that tells you how much you received and what was just Recycled. Same rules apply for Auto Recycle.. 
(a) falls below 25km above Kerbin and is therefore destroyed by the game but
(b) has 70 drag per ton (0.14t of stock parachute per ton of dry vessel mass)
then it will be recycled at 0.60 * sum(cost of parts in it). Note that a Mk16 parachute, which masses 0.1t and has drag coefficient of 500, yields 50 drag (0.1 * 500); the stock drogue yields less because its Cd is lower.
NOTE: Kerbals will NOT be recovered this way! This is only for spent stages, etc. You MUST ride down manually any craft containing crew or they will be lost.
3. Recycle-on-recover. The traditional way of recycling is gone and we're now using stock KSP recover functionality. When you go to the tracking station and select a vessel, the amount for which you can recycle it will appear in the MC main window. When you click recover, it will be recycled. Here you will get 0.85 * part cost if it is landed, and 0.65 if it is splashed down (landed on water).
4. Some more UI changes And added messages about what’s going on in the editor.


#Changes in .16
1. Finally Think I fixed all the bugs that the Auto Recycle Introduced to Test Flight Mode.. Now Test Flight Mode Should charge you the correct amount of money.
2. Fixed.. Also Fixed Auto Recycle working in Test Flight Mode.  You can no longer recycle vessels while in test flight mode.. The New System.. Or the old system
3. As a result of no more recycle in Test Flight mode. The Insurance Cost charge has been removed from test flight mode (without recycle you would never get this cost back)
4. New Window Introduced.  Starting to introduce a new window called the Finance Window.  Right now it holds your bank loans when you go in the red.  And a new.. But old Option called Passive Mission. Passive missions have been around for some time now.. But not many mission designers use the option.. A passive mission will pay you per day for (Place amount of Time) until that time runs out.  The finance window is now the major holder of this information.  Check the Civilian Space Program File to see how to do passive missions.
5. New missions introduced to the Civilian space Pack. Passive mission.  Many of the missions in the pack were converted to Passive missions. And I also Added 3 brand new missions for you to try out that use the passive mission option.
6. My continued pursuit of making the UI look better.. Some more changes I’m sure you might notice.. 


#.13 Changes
1.    Bank Loans.. Go into the Red and the bank will charge you .25% from your mission payouts until your back in the Green
2.	Many UI changes.  Been adjusting the Colors to help you more focus on what’s going on in the UI screen.
3.	You can now see the total Cost Of Fuels.. And Total Cost Of Parts Before the Total Cost Of Vessel.  To help you decide on fuel vs part cost more easy.
4.	Now when your mission ends you will see a total amount paid for the mission.  Also if you’re In the Red you will see the total amount paid after the bank takes its 25% from your Mission payout.

#.12 Changes
1.	Added support for Iron Cross Mod Oxygen Cost
2.	Added support for Modular fuel Tanks cost.. IE LiquidO2 and LiquidH2
3.	Some More UI changes.. Now resources will not show up until the have a value more the 0 in the Cost List.  Some things were left in to fill the area though.  Like cost and construction and insurance always show.

#.11 Changes 

1. Made Mission Controller work in .21
2. Got rid of the old difficulties options
3. Added 2 new modes.. Test Flight Mode and Flight Mode
4. Edited the Cost of Vessels to take into account Types... IE command, utility,engines,tanks, etc. 
5. Shrunk the UI to make it a little more visual friendly on your screen 
6. Other UI Changes to tell you what mode your in.[/CODE]


# version 0.9:

* fixed difficulty reset after restarting the game
* new icons from bac9
* added new mission field: repeatableSameVessel for client controlled and passive missions
* reworked costs (no construction costs because they are too different for different addons)
* new Duna mission

# version 0.8:

* added kerbonaut insurance costs which is configurable in the settings gui. The users gets those expenses back when he landed on Kerbin and recycles the manned vessel.
* added three difficulities, which define the costs for vessels
* fixed recycle option on distant planets
* fixed recycle option: ends the flight now
* reworked stock mission package


# version 0.7:

* fixed F2 bug
* added rewind option to get the latest expenses back. See config window
* added longitude and latitude for OrbitGoal (used for stationary orbits)
* fixed absurd longitude values
* added recycle option for splash downs
* added more icons, rescaled them in the make.sh script
* added high precision value representation for eccentricity values
* added overview for passive missions (only visible when no other mission is currently selected, and only
    in flight)

# version 0.6:

* added client controlled missions and passive missions which generate
    income over time
* moved icons into a separate folder
* added mission category icons from blazing angel 665

# version 0.5:

* implemented DockingGoal
* added CrashGoal
* resized mission package browser
* fixed some bugs with the mission package browser
* added sort button
* added new mission goal fields: min/maxTotalMass
* added new orbit goal fields: min/maxGForce, min/maxVerticalSpeed
* one .sp file for each KSP save. To use your old spaceProgram.sp file rename
    it to `TITLE (Sandbox).sp`, where TITLE is the game title, e.g. `KSP (Sandbox).sp`. It is located in
    `GameData/MissionController/Plugins/PluginData/MissionController/`
* added the possibility for mission designers to sort their missions with a new field: packageOrder and ownOrder.
    See documentation on github for more.

# Changelog

version 0.4:

* added a mission package browser
* if you want to create your own missions, read [missions.md](missions.md) , because you have
    to create a mission package instead of a single mission. Don't worry, it's easy.

* new mission field: category. This field will declare the icons that will be shown (does not work for now).
    There are several categories so far: DEFAULT, ORBIT, LANDING, DOCKING, EVA, MINING, SATELLITE. Do not
    use more than two categories at the same time for one mission!!! And use the icons to declare the
    *main* mission goal, not minor mission goals. Usage:

    Code:
    Mission
    {
            ...
            category = ORBIT, LANDING
            ...
    }

* added a new mission goal, EVAGoal, try Vostok II
* fixed minor bugs
