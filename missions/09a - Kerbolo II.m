Mission
{
    name = Kerbolo II
    description = We sent our first probe out of Kerbins atmosphere. Some scientists suggest a mission to discover other so called solar systems. They think the stars we see in the sky are just like Kerbol, but much further away. They are crazy, but we should try it. Build a space craft that is able to leave Kerbols sphere of influence.

    reward = 60000
    category = ORBIT, PROBE

    requiresMission = Kerbolo I
    packageOrder = 10

    SubMissionGoal
    {
        description = Bring the probe into an escape trajectory out of Kerbols sphere of influence.

        OrbitGoal
        {
            body = Sun

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
