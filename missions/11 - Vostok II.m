Mission
{
    name = Vostok II
    description = The manned suborbital flight was a fantastic success, but our physicians require more data on the effects of long-term space flights on Kerbal physiology. Launch a second mission, this time achieving a stable orbit for at least 6 hours before returning safely to the surface.

    reward = 100000
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
