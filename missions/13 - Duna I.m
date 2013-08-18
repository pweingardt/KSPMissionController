Mission
{
    name = Duna I
    description = Remote observation has given us a view of the planet Duna for centuries, but we only now have the capability of actually reaching it. We'd like to send an initial probe to Duna with a full sensor package and a long range communications dish. Optional secondary missions will provide an extra bonus: achieving a high polar orbit around the planet, and crashing the probe into the surface to provide us with surface composition data.

    reward = 150000
    category = ORBIT, PROBE

    requiresMission = Kerbolo II
    packageOrder = 13

    OrbitGoal
    {
        body = Duna
        description = Bring the probe into a high polar orbit around Duna.
        reward = 15000
        optional = true

        minInclination = 87
        maxInclination = 93

        minPeA = 80000
        maxApA = 100000
    }

    SubMissionGoal
    {
        description = Bring the probe into any orbit around Duna.

        OrbitGoal
        {
            body = Duna
        }

        PartGoal
        {
            partName = commDish
            partCount = 1
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

    CrashGoal
    {
        optional = true
        body = Duna
    }
}
