## Mission

A mission consists of one or multiple mission goals. You have to finish most mission goals in the given order in order to
finish the mission (this behaviour can vary). After you have finished the mission, you get the reward.

Mission fields:

* name: unique name for this mission. Used to identify the mission throughout the space program.
* description: The description for this mission.
* reward: The reward in krones for finishing this mission


## Mission goals

There are currently several mission goals, but they all have these fields in common:

### Common fields (available in *all* mission goals)

* description: A small description for this mission goal (field is optional)
* reward: reward in krones for finishing this mission goal (default: 0)
* crewCount: amount of kerbals needed to finish the mission goals (default: 0)
* optional: makes the mission goal optional (default: false)
* throttleDown: if true, the vessel needs to throttle down in order to finish the mission goal (default: true)
* repeatable: if true, the mission is repeatable. Requires a different vessel. You can't finish the same repeatable mission
    with the same vessel more than once. (default: false)
* minSeconds: the minimal seconds the vessel needs to meet all requirements for this mission goal to be able to
    finish this mission goal. (default: -1, ignored) Use TIME() instruction to set for high values (see below).

### OrbitGoal

Defines a certain orbit around a certain body as mission goal. All fields are optional.

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

Fields:

* name: Resource name, (default: LiquidFuel)
* minAmount: minimal amount of defined resource (default: 0, ignored)
* maxAmount: maximal amount of defined resource (default: 0, ignored)


### PartGoal

Defines a mission goal that requires a certain amount of a certain part on your vessel.

Fields:

* partName: name of the part
* partCount: minimal amount of the defined part (default: 1)
* maxPartCount: maximal amount of the defined part (default: -1, ignored)

### SubMissionGoal

Contains multiple mission goals and combines them into one mission goal. Keep in mind that *all* mission goals
have to be finished at the same time. Use with caution.

Do not laugh about the name... I will change it in the future. SubMission like in suborbital...

Fields:

* no extra fields


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

## Mission file instructions

There are currently three instructions for double fields:

* RANDOM(MINMAL, MAXIMAL) generates a random double value
* ADD(fieldName, VALUE) calculates ADD(fieldName, VALUE)
* TIME(aay bbd cch eem ffs) converts the time into seconds. All fields are optional. y = years, d = days, and so on.

Keep in mind that those are *instructions*. You can't combine them like `RANDOM(ADD(fieldValue, 5) ...`. This requires a
parser and it is not worth it.


## Random fields

Say you want to create a randomized mission, e.g. an orbiting mission around Kerbin. You can use the instructions `RANDOM` and `ADD`
to define your mission. Don't forget to add the `randomized = true` field, so that the users can discard the random mission and generate
another one. The mission will be generated everytime it loads otherwise.

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


## How to create your own mission

Unfortunately you need basic knowledge about orbital mechanics in order to write your own mission code.
For the start we want to write a basic exploration mission for Pol, one of Jools moons.

1. What do we want? We want the satellite to be in a stable orbit around Pol.
2. What does that mean in orbital numbers and stuff? The minimal periapsis is above Pols highest mountain and
    the orbit is not an escape trajectory. That means: minPeA = 5000 (I am not sure how high the highest
    mountain is, it is just a guess) and maxEccentricity = 1
3. That is all? No. You have to balance the numbers, especially the reward. Try to finish the
    mission: Build your vessel and write down the initial costs and once you accomplish the mission with
    the vessel you have built, you can adjust the rewards for your mission so that the
    `total reward = 2.5 * initial costs` (this is just an estimate equation).
4. Keep in mind that you shouldn't build a huge vessel just to finish a simple orbiting mission around
    Mun or something. Your vessel needs to be as small as possible.


