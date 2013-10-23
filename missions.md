# Mission packages

Mission packages were introduced so that the mission directory would not
be filled with mission files. The user can not browse the mission directory in
a convenient way if there are more than 20 missions.

So I introduced mission packages. A mission package is nothing more than a
bundle of missions and some basic fields.

The missions have the same fields as before and they are handled in the same
way as before, but the user select the missions in a different way:

* First he selects the mission package
* A bigger mission package browser appears where he sees the mission package
    name, a description, and all available missions on the left side of the
    window. A mission belongs to exactly one category and the users sees the
    mission category in form of an icon. Further more he can see if all
    requirements are met to finish the mission. He can sort the missions by
    name, by reward, by category or by status (= "he can finish the mission"
    or "he can not finish the mission" because the requirement are not met,
    only the requiresMission field is being checked.)
* On the left side is the mission list in form of buttons. When he selects
    a mission, the default mission description and the mission goals
    appear on the right side of the window. The user can not finish
    a mission in this window!
* Once he has browsed through the available missions and found a mission he
    wants to finish, he presses the button "Select mission". The mission
    package browser disappears and the selected mission appears inside the
    smaller window. Only the currently selected mission can be finished!


And now the definition of a mission package (.mpkg file):

    MissionPackage
    {
        name = A *small* description of your mission package
        description = A long description of your missions, with newest changes etc.

        Mission
        {
            *normal mission definition, see below for details*
        }

        Mission
        {
            *normal mission definition, see below for details*
        }
    }


Do not worry about the order of your missions. The plugin will sort the missions by name by default,
but the user can also sort the missions by reward, and later on by category.
If you want to provide your own mission order, use the ownOrder field:

    MissionPackage
    {
        ...
        ownOrder = true
        ...

        Mission
        {
            ...
            packageOrder = 1
            ...
        }

        Mission
        {
            ...
            packageOrder = 2
            ...
        }
    }


I will write a script that converts multiple mission files (old \*.m files) into one mission package
in order to make maintaining a mission package easier. It is easier to maintain several files instead of one,
in my opinion.

## Missions

A mission has several fields

* name: unique name for this mission. Used to identify the mission throughout the space program. Do not use the name of
    real life missions like "Apollo 11". Those names are reserved for designated mission designers.
* description: The description for this mission.
* reward: The reward in krones for finishing this mission
* scienceReward: Reward The Player in Science Points (KSP) be careful.. Don't give to much, science already too easy.
* requiresMission: name of another mission, that needs to be finished in order to finish this mission
    (default: "", ignored)
* repeatable: if true, the mission is repeatable. Requires a different vessel. You can't finish the
    same repeatable mission with the same vessel more than once.
* inOrder: if true, the mission goals have to be finished in the defined order (default: true)
* vesselIndependent: if true, it does not matter with which vessel the mission goal has been finished. (default: false)
* packageOrder: the number of this mission. Used to sort the missions. (default: 0, ignored)
* passiveMission: if true, this mission is a passive mission that generates income over time (default: false).
    Use it in combination with `clientControlled = true`, if possible.
* passiveReward: reward per day (default: 0)
* clientControlled: if true the client takes over control when the user finishes the mission
* destroyPunishment: if the vessel is involved in an active mission and the user destroys the vessel, he will
    be fined (default: 100000)
* lifetime: lifetime of this mission in seconds, use TIME. (default: 0)
* category: a list of categories (please *do not* use more than two categories at the same time for one mission)
* repeatableSameVessel: if true, then this mission can be finished more than once with the same vessel. Only possible
    for clientControlled or passive missions. (default: false)
* special: new field added in .20 that both Docking And UnDocking Have set to true.. The default Value is False.  This Field alows you to save a submission goal without the vessel ID.. (don't mix other goals with this Goal if you use it That do save the vessel ID) The advantage of this Field  over Vesselindependent is that the goal is still saved.. Vesselindependent is NOT SAVED.

Possible categories and their corresponding ideas:

* ORBIT: orbit goal
* PROBE: a simple probe
* IMPACT: crash goal
* LANDING: landing goal
* DOCKING: docking goal
* SATELLITE: a complex satellite
* MINING: mining mission (no icon yet)
* EVA: eva goal
* TIME: mission goals with minSeconds
* SCIENCE: science missions (No Mission Goal For This Yet)
* AVIATION: aviation missions
* MANNED: manned missions
* ROVER: a rover mission
* COMMUNICATION: communication mission (no icon yet Or Mission Goal Yet)
* REPAIR For the New Repair Mission Has icon.

Combine two categories with a coma: MANNED, ROVER.


A mission has one or multiple mission goals. After you have finished the mission, you get the reward.

Every vessel has its own mission: You can't finish the 1st mission goal with vessel `A`
and then continue to finish the 2nd mission goal with vessel `B`. Exception: EVAGoals, because an EVA is strictly speaking a vessel.
Exception B is the New DockingGoal and UnDockingGoal.  They both save the MissionData Only.. But not the Vessel ID.  This
also causes a problem with other Goals.. Suggest to keep these 2 in Seperate single Goals.. Because if you mix them with Goals
That do save the Vessel ID.. Then by default they will have the vessel ID also for that MissionGoal.. And you won't be able to UnDock or 
Dock In another SubMission.  (be warned)
### Mission Goals

Currently there are 11 mission goals:

1. OrbitGoal to define an orbit
2. LandingGoal to define a landing site
3. ResourceGoal to define a required resource
4. PartGoal to define a required part
5. EVAGoal to define an EVA
6. CrashGoal to define an impact
7. DockingGoal to define a docking maneuver
8. SubMissionGoal to combine multiple goals into one goal.
9. UnDockingGoal to undock from a vessel.. For start Missions.. Or other ideas (Read Important Info Below)
10. RepairGoal Places a part on vessel.. That can be used to set RepairGoal
11. LaunchGoal launch from Runway or any Planet Body

### Common mission goal fields (available in *all* mission goals)

* description: A small description for this mission goal (field is optional), do not use it for huge texts
* reward: reward in krones for finishing this mission goal (default: 0)
* crewCount: amount of kerbals needed to finish the mission goals (default: 0)
* optional: makes the mission goal optional (default: false)
* throttleDown: if true, the vessel needs to throttle down in order to finish the mission goal (default: true)
* repeatable: if true, the mission goal is repeatable. Requires a different vessel. You can't finish the same repeatable mission
    with the same vessel more than once. (default: false) (do not use this field unless you have to and you know what you are doing)
* minSeconds: the minimal seconds the vessel needs to meet all requirements for this mission goal to be able to
    finish this mission goal. (default: -1, ignored) Use TIME() instruction to set high values (see below).
* minTotalMass: the minimal total mass of the vessel, in tons (default: 0, ignored)
* maxTotalMass: the maximal total mass of the vessel, in tons (default: 0, ignored)

### OrbitGoal

Defines a certain orbit around a certain body as mission goal. All fields are optional.
Should also be used for aviation mission goals.

Fields:

* body: orbit around body (default: Kerbin)
* minEccentricity: minimal eccentricity (default: 0, ignored)
* maxEccentricity: maximal eccentricity (default: 0, ignored)
* eccentricity: target eccentricity (do not combine it with min/maxEccentricity) (default: 0, ignored)
* eccentricityPrecision: defines the necessary precision for eccentricity. E.g. eccentricity = 0.001 and
    eccentricityPrecision is 0.1, that means that the vessels eccentricity needs to be smaller
    than 1.1 * eccentricity and bigger than 0.9 * eccentricity (default: 0.1)
* minPeA: minimal periapsis (default: 0, ignored)
* maxPeA: maximal periapsis (default: 0, ignored)
* minApA: minimal apoapsis (default: 0, ignored)
* maxApA: maximal apoapsis (default: 0, ignored)
* minLan: minimal longitude of ascending node (default: 0, ignored)
* maxLan: maximal longitude of ascending node (default: 0, ignored)
* minInclination: minimal inclination (default: 0, ignored)
* maxInclination: maximal inclination (default: 0, ignored)
* minOrbitalPeriod: minimal orbital period in seconds (use TIME instruction, default: 0, ignored)
* maxOrbitalPeriod: maximal orbital period in seconds (use TIME instruction, default: 0, ignored)
* minAltitude: minimal altitude for aviation (default: 0, ignored)
* maxAltitude: maximal altitude for aviation (default: 0, ignored)
* minSpeedOverGround: minimal horizontal speed (default: 0, ignored)
* maxSpeedOverGround: maximal horizontal speed (default: 0, ignored)
* minGForce: minimal G force (default: 0, ignored)
* maxGForce: maximal G force (default: 0, ignored)
* minVerticalSpeed: minimal vertical speed (default: 0, ignored)
* maxVerticalSpeed: maximal vertical speed (default: 0, ignored)

### LaunchGoal

Defines a launch location as a mission goal.

Overrides default field `throttleDown` to `false`.

Fields:

* launchZone: celestial body or named location (default: launch pad)
** Current named locations supported: launch pad, runway
** Celestial bodies are also supported (eg. Kerbin).

### LandingGoal

Defines a landing on a certain celestial body as mission goal.

Fields:

* body: celestial body on which the vessel needs to land (default: Kerbin)
* splashedValid: if true, then a "landing" in an ocean is fine (default: true)
* minLatitude: maximal latitude of landing spot (default: 0, ignored)
* maxLatitude: minimal latitude of landing spot (default: 0, ignored)
* minLongitude: minimal longitude of landing spot (default: 0, ignored)
* maxLongitude: maximal longitude of landing spot (default: 0, ignored)


### ResourceGoal

Defines a certain amount of a certain resource as mission goal.
Use it in combination with SubMissionGoal! See [SubMissionGoal](#submission) for more informations.

Fields:

* name: Resource name, (default: LiquidFuel)
* minAmount: minimal amount of defined resource (default: 0, ignored)
* maxAmount: maximal amount of defined resource (default: 0, ignored)


### PartGoal

Defines a mission goal that requires a certain amount of a certain part on your vessel.
Use it in combination with SubMissionGoal! See [SubMissionGoal](#submission) for more informations.

Overrides default field `throttleDown` to `false`.

Fields:

* partName: name of the part
* partCount: minimal amount of the defined part (default: 1)
* maxPartCount: maximal amount of the defined part (default: -1, ignored)

### EVAGoal

Defines an EVA.

Overrides default field `vesselIndependent` to `true`.

Fields:

* no extra fields

Usage:

    EVAGoal
    {
    }

Yes, those brackets *are necessary*!

### CrashGoal

Defines an impact. Don't use it inside a SubMissionGoal with Parts, because the parts might get destroyed
before the plugin checks for the crash. Use a suborbital OrbitGoal in combination with PartGoals (inside a
SubMissionGoal) and after that a CrashGoal.

Field:
* body: name of the celestial body


### DockingGoal

Defines a successful docking. Docking goal was fixed in .33.  Multiple docking ports on a vessel will not make the
mission not finishable.  Now Docking records the ship ID again.  When combined with UnDocking please note that a mission
that uses Undocking will suffer the same problem of multiple docking ports not working.  There is no way around
this problem.  The id conflict because of the Ship ID changes when you Dock cause to many problems.

Fields:

* no extra fields

### UnDockingGoal

* Defines a Successful Undock from a vessel
UnDockingGoal
{
}

All you need.. Do not mix UnDocking and Docking in the Same SubMissionGoal.. Bad things happen.. In fact I suggest 
you keep them in single Goals by themselves.. .You can mix 1 and only 1 with other goals in SubMissionGoals.. Ive 
tested it and it works.. AGAIN DON't MIX BOTH TOGETHER. The MIssion will not be able to be finsihed!!

###RepairGoal

* Alows you to have a repair mission to a vessel. Place the part on the vessel (you can have the player do this in a 
* previous mission with the partGoal.  Then in another mission have the player launch to the vessel and use the part to
* repair the vessel.. The player right clicks on the part and the condition for Repair is set to true.  NEW For
* VERSION .33 Repair Goal has its own countdown Timer.. Its takes about 1 min to coplete repairs now.




### <a id="#submission"></a>SubMissionGoal

Contains multiple mission goals and combines them into one mission goal. Keep in mind that *all* mission goals
have to be finished at the same time. Use with caution.

Use this to define a PartGoal or ResourceGoal in combination with another goal. For example you want a satellite exploring Eve in a stable
orbit using scientic instruments. You don't want to explore Kerbin, you want to explore *Eve*. So instead of

    Mission
    {
        ...

        PartGoal
        {
            ...
        }

        OrbitGoal
        {
            ...
        }
    }

you should use

    Mission
    {
        SubMissionGoal
        {
            description = small description

            PartGoal
            {
                ...
            }

            OrbitGoal
            {
                ...
            }
        }
    }

The first mission is wrong, because I could build a vessel with scientic instruments on the lifting stage.
The scientific instruments won't make their way to Eve, because they are on the lifting stage, but I can and I will be able to finish the
first mission (not the second mission). Strictly speaking it was never required to have the scientific instruments *while* orbiting Eve
in the first mission.


Those fields are ignored in all mission goals inside the SubMissionGoal (you can use them on the SubMissionGoal though):

* repeatable
* reward
* optional


Do not laugh about the name... I will change it in the future. SubMission like in suborbital...

Fields:

* no extra fields


### Example mission

Here is the example `Mun X.m` mission file:

    Mission
    {
        name = Mun X
        description = Bring a manned space craft onto the surface of the Mun and bring him back.
        reward = 100000

        OrbitGoal
        {
            crewCount = 1
            reward = 10000
            body = Kerbin
            minPeA = 70000
            maxEccentricity = 1
        }

        OrbitGoal
        {
            crewCount = 1
            reward = 15000
            body = Mun
            minPeA = 3000
            maxEccentriciy = 1
        }

        LandingGoal
        {
            crewCount = 1
            body = Mun
        }

        OrbitGoal
        {
            reward = 15000
            crewCount = 1
            body = Mun
            minPeA = 3000
            maxEccentriciy = 1
        }

        LandingGoal
        {
            crewCount = 1
            body = Kerbin
        }
    }
##OrMissionGoal 
works just like SubMission Goal only this is a Complete this.. Or Complete this for the submission parts.

##NorMissionGoal 
again just like SubMission Goal but all Submissions within this goal must NOT be complete for to be true.


## Mission file instructions

There are currently three instructions for double fields:

* RANDOM(MINMAL, MAXIMAL) generates a random double value
* ADD(fieldName, VALUE) calculates fieldName + VALUE and assignes it to the field
* TIME(aay bbd cch eem ffs) converts the time into seconds. All fields are optional. y = years, d = days, and so on.

Keep in mind that those are *instructions*. You can't combine them like `RANDOM(ADD(fieldValue, 5) ...`. This requires a
parser and it is not worth it.


## Random fields

Say you want to create a randomized mission, e.g. an orbiting mission around Kerbin. You can use the instructions `RANDOM` and `ADD`
to define your mission. Don't forget to add the `randomized = true` field, so that the users can discard the random mission and generate
another one. The mission will be generated everytime in another way if you don't set the `randomized` field.

Here is an example randomized mission:

    Mission
    {
        name = Randomized Example
        description = Bring a satellite into the defined orbit
        reward = 80000
        randomized = true

        OrbitGoal
        {
            body = Kerbin
            minApA = RANDOM(100000, 200000)
            maxApA = ADD(minApA, 20000)

            maxEccentricity = 0.01
        }
    }

The required orbit has a maximal eccentricity of 0.01 and its minimal apoapsis is somewhere between 100000m and 200000m,
and the maximal apoapsis is 20000m higher than the minimal apoapsis.


# File format

The fileformat for mission packages is restrictive:

1. One line per field
2. no quotation marks like in C or Java
3. use `{` and `}` seperately, in one line
4. case sensitive
5. '#' at the beginning of the line (spaces and tabs don't matter) marks a comment


