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
