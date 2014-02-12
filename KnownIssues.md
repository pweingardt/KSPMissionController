#Known Problems

##MechJeb Cost To Dam Much!

The way to fix MechJeb if it costs too much is either:
(a) Give the part a reasonable mass, like at minimum 0.1 (then it'll only cost ~1800)
(b) remove MODULE { name = Module Command ..... } from the MJ part cfg.
(c) no we don't hate mechjeb. Its just the values it uses in its .cfg does not agree with MCE Part Cost Algorithms. ;)


##Science not paying out in crashgoal it seems!

If your having issues still getting science payout with CrashGoal Type Missions.. Read this!

Ok guys I have noticed that there is still a problem with science getting paid.. But its only in the Crash Type missions. And this is what I have figured out and how you can avoid it!

When you crash a vessel you get the old screen popup that shows you flight data and gives you option to revert or go to spacecenter or trackingstation! Its very very important that when you finish the mission for any Crash goal type mission that you exit this screen. For some reason this screen seems to set the persistence file back when you goto SpaceCenter scene using it and resets the science to old values before launching. If you exit out of this screen and exit the normal way.. (esc and exit to spacecenter) you will get paid for the science the right way.

I have tried fixing this.. But seems like there is nothing I can do (I even have a persistence save when science changes).. This screen is the problem, and not much I can do about how that pop up screeen works. 

It only happens with Crash type goals. All the other types of missions its not an issue because you never get the Exit screen that is causing the issue.

##Issues with adding parts to mission with underscores?

For Mission makers if your using a required part as a mission objective.. And it has an underscore in the name you have to replace the underscore with a period. So Large_Crewed_Lab would be Large.Crewed.Lab

Blizzy's Tool Bar. Noticed sometimes the ToolBar might jump to a different area. If it lands on say KSP buttons and you can no longer select Toolbars Menu System Delete the toolbar-settings.dat located in the GameData Folder to reset it to default.
