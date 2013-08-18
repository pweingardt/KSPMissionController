Mission
{
    name = Kerbolo II
    description = A second, similar mission has been planned to explore beyond Kerbin. This time you will also go beyond the orbit of Kerbol, escaping the gravitational force of the sun and launching toward interstellar space.

    reward = 60000
    category = ORBIT, PROBE

    requiresMission = Kerbolo I
    packageOrder = 10

    SubMissionGoal
    {
        description = Bring the probe into an escape trajectory out of Kerbol's sphere of influence.

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
