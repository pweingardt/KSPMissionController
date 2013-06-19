Mission
{
    name = Vostok II
    description = Bring a manned probe into an orbital flight around Kerbin, execute an EVA for 10 minutes and get back to Kerbin.
    reward = 42000

    category = ORBIT, LANDING

    OrbitGoal
    {
        description = Achieve a suborbital flight around Kerbin.
        reward = 6000
        crewCount = 1

        body = Kerbin
        minApA = 70000
        minPeA = 70000
    }

    EVAGoal
    {
        minSeconds = TIME(10m)
    }

    LandingGoal
    {
       description = Land safely back on Kerbin
       reward = 6000

       body = Kerbin
    }
}
