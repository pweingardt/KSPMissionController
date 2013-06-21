Mission
{
    name = Vostok III
    description = If we want to explore other planets we need to know how to rendezvous and how to dock. That is what our scientists say, but I don't trust them! But orders are orders and I just gave you one! Bring two manned space crafts into orbit and dock them together!

    reward = 195000
    category = ORBIT, MANNED

    requiresMission = Vostok II
    packageOrder = 12

    SubMissionGoal
    {
        description = Bring two manned probes into orbit and dock them. We suggest a circular orbit.
        crewCount = 1

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

    DockingGoal
    {
    }

    LandingGoal
    {
        body = Kerbin
    }
}
