Mission
{
    name = Kerbin Exploration I
    description = Start a satellite into a circular polar orbit betweend 85° and 90° inclination, 100km above Kerbins surface. Some scientists here want to map Kerbin.
    reward = 30000

    category = ORBIT

    OrbitGoal
    {
        body = Kerbin
        minInclination = 85
        maxInclination = 90

        minApA = 95000
        maxApA = 105000
        maxEccentricity = 0.001
    }
}
