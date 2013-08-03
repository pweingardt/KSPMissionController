### New In Version Mission Controller Extended

#New in .14 Auto Recycle.. 

Place some Parachutes on those spent stages.. And autorecycle will try to recycle the vessel for you


Changes In .14:  More updates Yay.  This time From NathanKell Thank him! he did some pretty neat things in this version.

1.    Cost in VAB Displays are now in Line with MC.  Pretty big change now is that you can see exactly how much that part is going to cost you in the parts description.  MC Brings all part cost to its formula and rewrites them to The Description of that part.
2.	Added a comma for those big numbers.. IE MAX AP MINPE and other mission goal numbers now should read with a comma.. And also prices in the MC window.
3.	All Mission Times in My Mission Packs have been edited to more reasonable times… I think the largest one is now only 4 hours. Which is only minutes in time acceleration.
4.	Part names from Description Now Show Up in The Mission Goals.. Before it was Part .cfg file names.. This should help you with what part it is you actually need.  ##For Mission Editors## you still need to use the Part .cfg name! Don’t forget to replace Underscore with a period (Example Part_Coolname_Awsome.. Will be written like this in mission Part.Coolname.Awsome))
###5.	Last Update and what we would like you guys to test out for us.  The Auto Recycle of Spent Stages has been implemented.  These are some of the rules for this new AutoRecycle.
A.	Notes on auto-recycling: This is a test Version Of AutoRecycle. 
B.	You need 70 drag per ton (approx .15 parachute mass per ton, more if using drogues)
C.	You only get 60% of dry cost (and nothing else) back. Crew is still dead, for example.



New In.13 Bank Loans:  Now if you go into the red with mission controller the bank will be happy to lend you the money you need to continue your space program.  But it will cost you interest.  At 25% of Your Mission Payouts until you pay off the loan and get back into the Green.

.11 Test Flight:  now you can test your flights instead of just turning the plugin off.. At a cost of 3% of Dry cost… including Kerbal Cost if you have test pilots...If you have insurance cost at a price higher then 0. When you return you can recycle what’s left of your test craft… And get your insurance cost back… 

Good Luck Pilot.  Malkuth…

###.13 Changes
1.    Bank Loans.. Go into the Red and the bank will charge you .25% from your mission payouts until your back in the Green
2.	Many UI changes.  Been adjusting the Colors to help you more focus on what’s going on in the UI screen.
3.	You can now see the total Cost Of Fuels.. And Total Cost Of Parts Before the Total Cost Of Vessel.  To help you decide on fuel vs part cost more easy.
4.	Now when your mission ends you will see a total amount paid for the mission.  Also if you’re In the Red you will see the total amount paid after the bank takes its 25% from your Mission payout.

###.12 Changes
1.	Added support for Iron Cross Mod Oxygen Cost
2.	Added support for Modular fuel Tanks cost.. IE LiquidO2 and LiquidH2
3.	Some More UI changes.. Now resources will not show up until the have a value more the 0 in the Cost List.  Some things were left in to fill the area though.  Like cost and construction and insurance always show.

###.11 Changes 

1. Made Mission Controller work in .21
2. Got rid of the old difficulties options
3. Added 2 new modes.. Test Flight Mode and Flight Mode
4. Edited the Cost of Vessels to take into account Types... IE command, utility,engines,tanks, etc. 
5. Shrunk the UI to make it a little more visual friendly on your screen 
6. Other UI Changes to tell you what mode your in.[/CODE]

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


