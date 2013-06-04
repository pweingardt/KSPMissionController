### Mission Controller for Kerbal Space Program

This plugin keeps track of your accomplished mission and your available budget.
You can even create your own missions with a simple text editor and a small tutorial.


### How this works

Once you have an active vessel (the plugin does not work in the editor), in the bottm left part of your screen appears a new icon: "MC"
for Mission Control. Click on it and it gives you the mission window, that shows your budget and informations about
the currently selected mission.

But be careful with your rockets! Once you launch the space craft on the launchpad, it will cost you Kerbin krones (the currency on Kerbin, ₭). Even if you restart the
flight, the krones are *GONE*.

### VESSELS COST KRONES???

Yes, indeed. You pay for construction, fuel (solid fuel, liquid fuel, mono propellant and xenon),
and other materials (= mass without fuel). But don't worry, the Kerbal Space Program will borrow you money, if you need it.

### How to accomplish a mission

Open the mission window and select the mission you want to accomplish.
Before you launch the space craft, you should read the mission description and all mission goals. Just to be clear
that you didn't forget something... because you will forget something.
Then launch your space craft and accomplish the *first* mission goal. Most missions require you to finish the mission
goals in the right order to get the reward for the mission.

Let's take a look at the "Sputnik III" mission:

Reward: 14000 + 2000

1. reach a stable orbit around kerbin, minimal periapsis: 70km (2000 krones reward)
2. Land back on Kerbin

You can't finish the second mission goal before you did not finish the first mission goal. Once you have achieved the
stable orbit, you get a small reward for reaching this goal (you can hide all finished goals by pressing the button). When you are out of
fuel and can't make it back to Kerbin, you will not get the 14k reward. Once you *land or splash* back on Kerbin, you get the full
14k + 2k reward. There are missions, where a splash is not enough. Crashes on the surface, that do not destroy the space craft, are sufficient.

Let's take another look at the "Mun I" mission:

Reward: 20000 + 5000

1. Get into an escape trajectory (a flyby) around Mun. The periapsis needs to
be between 4km and 6km.
2. Bring your probe back to Kerbin. Reward 5000, but optional

You don't need to finish the 2nd mission goal, because it is optional. But it will give you 5000
krones.


### How do I create a new mission?

This part is a bit more complicated and you need advanced knowledge about orbital mechanics.
Lets take a look at "Sputnik I.m"

    Mission
    {
        name = Sputnik III
        description = Bring a small satellite into a stable orbit around Kerbin and return safely back to Kerbins surface.
        reward = 14000

        OrbitGoal
        {
            reward = 2000

            body = Kerbin
            minPeA = 70000
            maxEccentricity = 1
        }

        LandingGoal
        {
            reward = 2000

            body = Kerbin
        }
    }

A mission consists of several (or one) mission goals. Those mission goals can be:

1. a certain orbit
2. a successful landing on a planet (splashing is enough)
3. special parts on your space craft, that you need to accomplish the mission
4. a minimal crew count to accomplish the mission
5. another mission

With combinations of these goals you can create a complicated mission, consisting of several mission goals:

1. stable orbit around Kerbin
2. stable orbit around Mun
3. landing on Mun (manned!)
4. bring it back into a stable orbit around Mun
5. landing on Kerbin (manned!)

Here it is:

    Mission
    {
        name = Mun X
        description = Bring a manned space craft onto the surface of the Mun and bring it back.
        reward = 30000

        OrbitGoal
        {
            crewCount = 1
            reward = 4000
            body = Kerbin
            minPeA = 70000
            maxEccentricity = 1
        }

        OrbitGoal
        {
            crewCount = 1
            reward = 10000
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
            reward = 10000
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

Create your missions and share them!
