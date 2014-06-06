#Known Problems

### How MCE does its calculations, and how Other Mods Might break this.
MCE does most of its calculations during scenechanges, or when you go to a praticular scene.  There are some mods that
will allow you to skip certain scenes or to go to scene in an order that is out of sync with what a Regular Vanilla game will allow.  Let me tell you now that if you use these types of mods with MCE, I guaratee you that you will have very bad experience with Mission Controller, and other mods that use the same type of system to comuicate and excute programs in the mod.

Why does MCE use these changes to excute programs it uese?  Well its to keep Kerbal Space Program and MCE from completely bogging down your system.  If MCE only does 1 check for its whole program when a scene change takes place. Thats a lot better then doing 100 checks a second and bogging down your system.  It works out very well most of the time, unless of course you use a MOD that skips around KSP or completely skips certain scenes.  You been warned.

### EVA Kerbal is giving me double mission payouts.
This has been fixed in version .69.


### Apollo Syle Missions:

New Test .dll that fixes this issue is out, will be part of .70 when fully tested and implemented.
http://forum.kerbalspaceprogram.com/entries/1529-Mission-Controller-Developer-Test-Version-%28Apollo-Style-Mission-Support%21%29

People report having issues doing a mun mission apollo style.  Well the reason this happens is because MCE uses Vessel ID's to Record what Vessel has done what Mission Goal in each mission.  So when you say Land on Mun with Lander MCE records that landers ID and Finishes the Mission Goal.  When you take off from Mun, dock with CM and dump the lander your CM has a totally different Vessel ID.  So now when you land the CM back at kerbin, MCE doesn't think you did the Mun landing part of mission yet because the Landing Vessel at kerbin does not match the one that landed on Mun.  

If you must have Apollo style Missions then there is a work around.

Add this line to any landing goal in a mission you want to do Apollo Style.

vesselIndenpendent = true

This is what it should look like in a real mission type.



	LandingGoal
        {
            body = Mun
	    vesselIndenpendent = true
        }
	LandingGoal
        {
            body = Kerbin
	    vesselIndenpendent = true
        }

You can also use the Custom Contract Maker to make your own Apollo style missions.  The contract maker in game tool will automatically ask you if you want to use the vesselIndependent = true option when making the missions.  ;)

##MechJeb Cost To Dam Much! (fixed in .51)

The way to fix MechJeb if it costs too much is either:
(a) Give the part a reasonable mass, like at minimum 0.1 (then it'll only cost ~1800)
(b) remove MODULE { name = Module Command ..... } from the MJ part cfg.
(c) no we don't hate mechjeb. Its just the values it uses in its .cfg does not agree with MCE Part Cost Algorithms. ;)


##Mission Goals Not Being Completed!
Most of the time mission goals will be completed, but sometimes special issues might arise to make mission goals not be competed.  And it all matter how the mission was written.  One of the biggest problems I have found that can cause problems is this.
If you build a huge vessel in Orbit made of many different vessels.  IE a vessel that goes to duna and has an Interplanetary stage, a Landing Stage, and say another part that is the orbiter that brought the crew up to the main vessel.  All these vessels are considered different vessels and when they undock have different Vessel IDs.  So if you did a 3 part mission that was Orbit Duna, Land Duna, then land at Kerbin you’re going to have problems!

First the first part was done with The main Vessel and you orbit duna.. Everything is cool, goal complete.  Now it’s time for the landing part.  You undock the Landing vessel and go and land on duna.. But the mission goal is not complete, why?  Well because the Lander and the Main Vessel have separate Vessel IDs.  And MCE saves Mission Goals Via Vessel ID’s and checks these ID’s with other goals in the mission. If they don’t match then the goal can’t be completed!  There is a way around this though. But the mission maker has to put a variable into the mission to make this work!  It’s called Vessel Independent!  If Vessel Independent is placed on the Landing Goals, all goals will be completed without issues. Because both landing goals it does not matter what vessel actually lands.
This is a limitation of how MCE works and the saving process.  But you can add the Vessel Independent goal to Mission Goals if you wish.  But it’s the only way to combat this situation.
Also note any vessel that you launch that’s all in one.  Large vessel has both orbiter and lander on it, this is not an issue.  Because again they are the same vessel, your just breaking it apart and it will not cause this issue to happen.
A Vessel Independent Goal was added to the custom contract builder to help combat this issue with Custom Contracts. So use it if your vessel is going to be many vessels docked into one large vessel.


##Science not paying out in crashgoal it seems!

If your having issues still getting science payout with CrashGoal Type Missions.. Read this!

Ok guys I have noticed that there is still a problem with science getting paid.. But its only in the Crash Type missions. And this is what I have figured out and how you can avoid it!

When you crash a vessel you get the old screen popup that shows you flight data and gives you option to revert or go to spacecenter or trackingstation! Its very very important that when you finish the mission for any Crash goal type mission that you exit this screen. For some reason this screen seems to set the persistence file back when you goto SpaceCenter scene using it and resets the science to old values before launching. If you exit out of this screen and exit the normal way.. (esc and exit to spacecenter) you will get paid for the science the right way.

I have tried fixing this.. But seems like there is nothing I can do (I even have a persistence save when science changes).. This screen is the problem, and not much I can do about how that pop up screeen works. 

It only happens with Crash type goals. All the other types of missions its not an issue because you never get the Exit screen that is causing the issue.

####Updated: 
Vessel Destroyed - Mission Controller Help

Your vessel was destroyed during a Mission Controller mission. In order to gain credit for a Crash Goal, or to properly revert this flight, certain steps must be taken.

1) Click outside of this help window. A crash log window should appear.
2) Close the crash log window. DO NOT USE ANY OTHER BUTTONS ON THE CRASH LOG WINDOW, EXCEPT FOR THE CLOSE BUTTON.

If you wish to accept the mission failure and destruction of this vessel:

3) Click the "Erase Finished Goals" button below.
4) Hit the ESC key. The game menu should appear.
5) Click the "Space Center" button.

If you have completed your Crash Goal mission:

3) Click the "Exit" button on this window.
4) Hit the ESC key. The game menu should appear.
5) Click the "Space Center" button.

If you wish to revert this flight:

3) Click the "Exit" button on this window.
4) Hit the MCE "Revert Flight" button.

If you wish to quick load from a save:

3) Click the "Exit" button on this window.
4) Hold down the F9 key.
Written By. JeBuSBrian

##Issues with adding parts to mission with underscores?

For Mission makers if your using a required part as a mission objective.. And it has an underscore in the name you have to replace the underscore with a period. So Large_Crewed_Lab would be Large.Crewed.Lab

Blizzy's Tool Bar. Noticed sometimes the ToolBar might jump to a different area. If it lands on say KSP buttons and you can no longer select Toolbars Menu System Delete the toolbar-settings.dat located in the GameData Folder to reset it to default.
