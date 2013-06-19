Mission
{
    name = Vostok I
    description = Bring a manned probe into a suborbital flight around Kerbin and bring it back.
    reward = 32000

    category = ORBIT, LANDING

    OrbitGoal
    {
        description = Achieve a suborbital flight around Kerbin.
        reward = 6000
        crewCount = 1

        body = Kerbin
        minApA = 70000
        maxPeA = 1
    }

    LandingGoal
    {
       description = Land safely back on Kerbin
       reward = 6000

       body = Kerbin
    }
}
