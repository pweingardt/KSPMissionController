Mission
{
    name = Kerbolo I
    description = Our scientists are now confident in their understanding of Kerbin and its moons. Our next step is to leave Kerbin's sphere of influence. Our scientists are uncertain what conditions this probe will encounter; take a full sensor package with you. Data will be transmitted back to Kerbal ground stations directly so a return to the plan will not be required.

    reward = 85000
    category = ORBIT, PROBE

    requiresMission = Sputnik IV
    packageOrder = 9

    SubMissionGoal
    {
        description = Bring the probe into an escape trajectory out of Kerbin's sphere of influence.

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
