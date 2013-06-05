Mission
{
    name = Vostok III
    description = Bring two manned crafts into a stable circular 80km orbit around Kerbin and dock them.
    reward = 44000

    SubMissionGoal
    {
        OrbitGoal
        {
            description = Reach this orbit and dock two vessels in this orbit
            reward = 4000
            crewCount = 1

            body = Kerbin
            minApA = 75000
            maxApA = 85000
            maxEccentricity = 0.01
        }

        DockingGoal
        {

        }
    }
}
