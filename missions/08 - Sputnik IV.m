Mission
{
    name = Sputnik IV
    description = A special mission has been requested by a climate research facility on Kerbin. Limited data is available on the north pole of Kerbin itself, but a space-based probe would provide us a unique opportunity to fully map and analyze its topology. Construct such a probe and place it into a circular polar orbit, maintaining stability for at least 2 hours in order to capture sufficient data.

    reward = 65000
    category = ORBIT, PROBE

    requiresMission = Mun I
    packageOrder = 8

    SubMissionGoal
    {
        description = Bring a sensor package into a high circular polar orbit around Kerbin.

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
