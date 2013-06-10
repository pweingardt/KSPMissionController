# current master branch

* one .sp file for each KSP save, using HighLogic.CurrentGame.Title. To use the old spaceProgram.sp file rename
    it to `TITLE (Sandbox).sp`, where TITLE is the game title.
* saves the space program every time the game scene changes. Change it to save the space program only if
    the game scene is the main menu?


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
