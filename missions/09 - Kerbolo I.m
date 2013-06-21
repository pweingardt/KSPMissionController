Mission
{
    name = Kerbolo I
    description = Our next step is to leave Kerbins sphere of influence. Our scientists do not know what will happen out there. They don't expect magic though. Take some instruments with you.

    reward = 95000
    category = ORBIT, PROBE

    requiresMission = Sputnik IV
    packageOrder = 9

    SubMissionGoal
    {
        description = Bring the probe into an escape trajectory out of Kerbins sphere of influence.

        OrbitGoal
        {
            body = Kerbin

            minEccentricity = 1
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
