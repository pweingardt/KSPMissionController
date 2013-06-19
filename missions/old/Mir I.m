Mission
{
    name = Mir I
    description = Bring a small space station in an inclined low orbit around Kerbin. You will need at least 3 Hitchhiker Storage modules.

    reward = 450000
    category = ORBIT

    clientControlled = true
    lifetime = TIME(5y)

    SubMissionGoal
    {
        OrbitGoal
        {
            body = Kerbin

            minApA = 80000
            maxApA = 100000
            maxEccentricity = 0.001

            minInclination = 20
            maxInclination = 30
        }

        PartGoal
        {
            partName = crewCabin
            partCount = 3
        }
    }
}
