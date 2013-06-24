Mission
{
    name = Sputnik I
    description = Our scientists decided to explore space. For some reason they elected you as our head rocket engineer. We need to build a rocket in order to explore Kerbins atmosphere. Build a rocket that is able to leave the atmosphere and bring the probe with the data back to Kerbins surface.

    reward = 50000
    category = ORBIT, PROBE

    packageOrder = 1

    SubMissionGoal
    {
        description = Reach at least 70km above ground and record the atmospheric data. You will need 3 Communotron 16, 1 accelerometer, 1 gravimeter, 1 thermometer and 1 barometer.

        OrbitGoal
        {
            body = Kerbin
            minAltitude = 70000
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

    LandingGoal
    {
        body = Kerbin
    }
}
