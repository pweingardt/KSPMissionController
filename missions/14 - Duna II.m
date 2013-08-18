Mission
{
    name = Duna II
    description = Our remote observations were spectacular. Send a followup probe to Duna, this time landing it on the surface with its sensor package and communication dish intact. 

    reward = 200000
    category = LANDING

    requiresMission = Duna I
    packageOrder = 14

    SubMissionGoal
    {
        description = Bring the probe into any orbit around Duna.

        LandingGoal
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

