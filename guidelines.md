# Mission packages

Mission packages were introduced so that the mission directory would not
be filled of mission files. The user can not browse the mission directory in
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


