Mission
{
    name = Sputnik IV
    description = You have been around the Mun, but now it is time to explore Kerbin itself. We have been to the northpole, but we didn't see it from space. Bring a probe with some instruments into a circular polar orbit and leave it there for at least 2 hours.

    reward = 45000
    category = ORBIT, PROBE

    requiresMission = Mun I
    packageOrder = 8

    SubMissionGoal
    {
        description = Bring the probe into a high circular polar orbit around Kerbin.

        OrbitGoal
        {
            body = Kerbin

            minPeA = 300000
            maxApA = 350000
            maxEccentricity = 0.01

            minInclination = 87
            maxInclination = 93
        }

        PartGoal
        {
            partName = longAntenna
            partCount = 3
        }

        PartGoal
        {
            partName = sensorAccelerometer
            partCount = 1
        }

        PartGoal
        {
            partName = sensorBarometer
            partCount = 1
        }

        PartGoal
        {
            partName = sensorGravimeter
            partCount = 1
        }

        PartGoal
        {
            partName = sensorThermometer
            partCount = 1
        }
    }
}
