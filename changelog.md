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
