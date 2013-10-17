
##Research Tech New To MCE
Recycling: You have to research the ability to recycle now.

Fuel Efficiency:  research a Top End 20% Reduction to how well You Produce Fuels.. (IE Cheaper Fuel Cost)

Construction 1:  First Level Of Construction Your Paying Out about 120% more Then Original MCE.. 
With This Upgrade you bring it back down to Normal MCE.

Construction 2:  2nd Reduction in Construction Cost with an additional %10 Cut to cost.

##Loans Bank Loans:

Now if you go into the red with mission controller the bank will be happy to lend you the money you need to 
continue your space program. But it will cost you interest. At 25% of Your Mission Payouts until you pay off 
the loan and get back into the Green.

##Budgets And Financing: New For .30 and KSP .22

New Finance Screen that tracks the following

Total Mission Payouts (lifetime)
Total Recycling Payouts (LifeTime)
Total Of Both Above (LifeTime)
Total Spent On Vessels (LifeTime)

Total Profit (LifeTime)  After All Expenses.

The mod takes care of your budget.. Subtracting Cost of vessels for you.. And putting
the payments from missions back into your Budget. Can You keep it in the green?

Vessel Cost and Mission Payouts: Design your vessels like normal.. But now when you launch you will be charged
for it.. Each vessel cost you Crones. You start the Game with 50,000 Crones.. Can you spread this money enough to get your budding Space Program off the ground? If you can't don't worry.. You can go into the red... and mission controller will let you borrow money.. But how far do you go before you decide you can't handle it? Only time will tell.


##Flight Testing Mode:

Why should testing your Crafts be free? Is it free in the real world? What about Test Pilots do they really die?
The answer is yes.. New In Mission Controller Extended is the Testing Mode... Now you can set the Mod to Flight 
Testing Mode and you will be charged to test those craft.... Based on the cost of the actual vessel.. But don't
worry its not the full cost.. Its based on 3% of the original Craft.. So ya its cheap.. But how many test do you 
need anyway? They can add up so beware.

In both Normal Flight Mode and Testing mode if you lose Kerbalnauts or Test Pilots you can suffer a Insurance
cost.. All insurance cost are charged as the flight takes off.. And if the Victoms surive the blast... I mean 
test the cost will be returned to you... And of course you can choose to set this cost to what you like.. The 
default is 0... But I suggest a nice round number like 55,000.. Its what I use in my youtube series.

##Recover And Recycle Your Vessels: 

Totally Changed in .16 and .17. Gone is the old way of Recycling your vessel.. 
Now you can go into Tracking Station and use the Recover button to Recover AND Recycle your vessels. Here you will 
get 0.85 * part cost if it is landed, and 0.65 if it is splashed down (landed on water).

##Auto Recycle:

recycles your spent stages. This is how it works.
(a) falls below 25km above Kerbin and is therefore destroyed by the game but
(b) has 70 drag per ton (0.14t of stock parachute per ton of dry vessel mass)
then it will be recycled at 0.60 * sum(cost of parts in it). Note that a Mk16 
parachute, which masses 0.1t and has drag coefficient of 500, yields 50 drag (0.1 * 500); the stock 
drogue yields less because its Cd is lower.
NOTE: Kerbals will NOT be recovered this way! This is only for spent stages, etc. You MUST ride down 
manually any craft containing crew or they will be lost.

##Disable Plugin:

Feel a little down today and don't want to be charged anything. Want to turn off the Plugin.. 
Well the option to turn off the plugin is in the settings.. And nothing will be charged.. And 
no missions can be completed.. But essentially the mod does not exist in this mode.

##Missions:

Everything from Orbits, to landing on other moons or planets to Docking with other 
crafts are available to you in the missions!!.. Have some knowledge in coding? Make your own missions 
with easy to edit .cfg files!! (future plan might be an actual mission editor everyone can use) And with 
my added missions bringing you two new packs.. The missions grow even more.. 2 Story Based missions fashioned 
in the kerbal style we are use too. These missions are designed to work along with your Sandbox mode.. Like to
make your own vessels and do what you want.. But in the scope of Mission Controllers budget? Well the 2 added 
Packs are made to pay you out enough to move you along in Mission Controller.. And fund your own missions....

##mod support: 

Support for Prices For the Mods IonCross Oxygen Cost.. And Modular Fuel Tanks Fuel Cost

### Mission Controller Extended

Please Read Important Mission Controller Extended is the Continuation of the Mission Controller 


### Mission Controller for Kerbal Space Program

This plugin keeps track of your accomplished mission and your available budget.
You can even create your own missions with a simple text editor and a small tutorial.


### How this works

In the bottom left corner is a new icon (appears only when you are at the space center, flying a vessel or
building a rocket or a plane): "MC" for mission controller.
Click on it and it gives you the mission window, that shows your budget and informations about
the currently selected mission.

But be careful with your rockets! Once you launch the space craft on the launchpad, it will cost you Kerbin krones (the currency on Kerbin, ₭). Even if you restart the
flight, the krones are *GONE*.

### VESSELS COST KRONES???

Yes, indeed. You pay for construction, fuel (solid fuel, liquid fuel, mono propellant and xenon),
and other materials (= mass without fuel). But don't worry, the Kerbal Space Program will borrow you money, if you need it.

### How to accomplish a mission

Open the mission package browser window and select the mission package to browse the missions.
A bigger window opens and you can browse through all missions in that mission package. Once you have decided to finish the selected
mission, press "select mission" and the mission package browser disappears. The previously selected mission appears in the smaller
window and you are able to finish the mission now.

Before you launch the space craft, you should read the mission description and all mission goals. Just to be clear
that you didn't forget something... because you will forget something.
Then launch your space craft and accomplish the *first* mission goal. Most missions require you to finish the mission
goals in the right order to get the reward for the mission.

Let's take a look at the "Sputnik III" mission:

Reward: 24000 + 2000

1. reach a stable orbit around kerbin, minimal periapsis: 70km (2000 krones reward)
2. Land back on Kerbin

You can't finish the second mission goal before you did not finish the first mission goal. Once you have achieved the
stable orbit, you get a small reward for reaching this goal (you can hide all finished goals by pressing the button). When you are out of
fuel and can't make it back to Kerbin, you will not get the 14k reward. Once you *land or splash* back on Kerbin, you get the full
14k + 2k reward. There are missions, where a splash is not enough. Crashes on the surface, that do not destroy the space craft,
are sufficient but reduce the recyclable value of the crashed ("landed") vessel.

Let's take another look at the "Mun I" mission:

Reward: 40000 + 5000

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

For more infos about mission goals, see [missions.md](missions.md).

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
With 0.4 you have to use mission packages, read [missions.md](missions.md) for more.

### Contributions

* maintainer and main developer: nobody44
* support and ideas: vaughner81
* support and ideas: tek\_604
* ideas: BaphClass
* images: BlazingAngel665


