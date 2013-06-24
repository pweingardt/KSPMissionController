Mission
{
    name = Sputnik II
    description = Our scientists say that some weird things happend in our last missions. They didn't like it. They think that the radiation outside the atmosphere is higher than they expected, and guess what you are doing now? You will build a rocket that is able to reach a stable low orbit around Kerbin in order to explore the radiation outside the atmosphere for at least 4 hours. Don't forget to record the data and bring the probe back!

    reward = 55000
    category = ORBIT, PROBE

    requiresMission = Sputnik I
    packageOrder = 2

    SubMissionGoal
    {
        description = Reach a stable orbit around Kerbin. You will need 3 Communotron 16, 1 accelerometer, 1 gravimeter, 1 thermometer and 1 barometer. Stay inside Kerbins magnetic field...

        minSeconds = TIME(4h)

        OrbitGoal
        {
            body = Kerbin
            minPeA = 70000
            maxApA = 400000
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
