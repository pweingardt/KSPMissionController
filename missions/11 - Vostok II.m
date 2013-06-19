Mission
{
    name = Vostok II
    description = After our first suborbital flight we need to explore the effects of long-term space flights on kerbonauts. Stay at least 6 hours in an orbital flight around Kerbin.

    reward = 75000
    category = ORBIT, MANNED

    requiresMission = Vostok I
    packageOrder = 11

    SubMissionGoal
    {
        description = Bring the manned probe into an orbital flight around Kerbin.
        crewCount = 1

        minSeconds = TIME(6h)

        OrbitGoal
        {
            body = Kerbin
            minPeA = 80000
            maxApA = 500000
        }

        PartGoal
        {
            partName = longAntenna
            partCount = 3
        }
    }

    EVAGoal
    {
        minSeconds = TIME(5m)
    }

    LandingGoal
    {
        body = Kerbin
    }
}
