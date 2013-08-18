Mission
{
    name = Sputnik II
    description = Our scientists have reviewed the last mission's data and have discovered some unexpected anomalies. Radiation levels outside the atmosphere are significantly higher than expected, and they've requested a second mission in order to further explore the radiation levels outside of Kerbal's atmosphere. Modify your previous probe in order to maintain a stable orbit around Kerbin for at least 4 hours before returning to the surface with all sensors and the new data.

    reward = 55000
    category = ORBIT, PROBE

    requiresMission = Sputnik I
    packageOrder = 2

    SubMissionGoal
    {
        description = Achieve a stable orbit around Kerbin. You will need 3 Communotron 16, 1 accelerometer, 1 gravimeter, 1 thermometer and 1 barometer. Stay inside Kerbins magnetic field to avoid corrupted readings.

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
