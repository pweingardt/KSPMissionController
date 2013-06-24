Mission
{
    name = Duna I
    description = We have discovered a new planet. We call it Duna. It is red and we don't know why. Our scientists think it is red because of the rust on the surface. But I think it is because of chili sauce. Just like in our last missions, you are now officialy ordered to bring a probe near Duna. We don't care what you do there, just get there! Watch out for moons. If possible, bring the probe into a polar orbit. Don't forget the instruments! We need long range communication dishes on the probe! If you can manage it, crash into Duna...

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
        description = Bring the probe near Duna. We do not care where or how... But take some instruments with you, we don't know if Duna has an atmosphere. And just fly through the atmosphere if there is one. And take some pictures!

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
