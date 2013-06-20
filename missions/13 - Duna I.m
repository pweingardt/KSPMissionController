Mission
{
    name = Duna I
    description = We have discovered a new planet. We call it Duna. It is red and we don't know why. Our scientists think it is red because of the rust on the surface. But I think it is because of chili sauce. Just like in our Minmus mission, you are now officialy ordered to crash into Duna. Watch out for moons. If possible, bring the probe into a polar orbit first. Don't forget the instruments! We need long range communication dishes on the probe!

    reward = 100000
    category = PROBE, IMPACT

    requiresMission = Kerbolo II
    packageOrder = 13

    OrbitGoal
    {
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
        description = Crash on Duna. We do not care where on how... But take some instruments with you, we don't know if Duna has an atmosphere.

        CrashGoal
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
}
