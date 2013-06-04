# Mission gaols

A missions consists of one of multiple mission goals. The mission goals have to be finished in the
right order in order to finish the mission.

There are currently several mission goals:

### Common fields (available in *all* mission goals)

* description: A small description for this mission goal (field is optional)
* reward: reward in krones for finishing this mission goal (default: 0)
* crewCount: amount of kerbals needed to finish the mission goals (default: 0)
* optional: makes the mission goal optional (default: false)
* throttleDown: if true, the vessel needs to throttle down in order to finish the mission goal (default: true)


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


### SubMissionGoal

Contains multiple mission goals and combines them into one mission goal.

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
    `total reward = 2.5 * initial costs`.
4. Keep in mind that you shouldn't build a huge vessel just to finish a simple orbiting mission around
    Mun or something. Your vessel needs to be as small as possible.


