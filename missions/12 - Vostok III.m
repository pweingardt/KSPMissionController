Mission
{
    name = Vostok III
    description = Interplanetary exploration will require sophisticated docking expertise. Before we attempt such docking procedures in the depths of space, we require a test mission for our astronauts to practice a docking procedure in a standard circular orbit around Kerbal. Simply bring two manned craft together and dock them successfully, and then bring the craft safely back to the surface of the planet.

    reward = 195000
    category = ORBIT, MANNED

    requiresMission = Vostok II
    packageOrder = 12

    SubMissionGoal
    {
        description = Bring two manned probes into orbit and dock them.
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
