Mission
{
    name = ComSat Contract II
    description = We signed a contract to bring a satellite into a semi synchronous orbit around Kerbin.
    repeatable = true
    reward = 85000
    randomized = true

    clientControlled = true
    lifetime = TIME(1y)

    category = SATELLITE, ORBIT

    OrbitGoal
    {
        body = Kerbin

        minApA = 1582180
        maxApA = 1588180

        maxEccentricity = 0.0001

        minInclination = RANDOM(0, 88)
        maxInclination = ADD(minInclination, 0.5)
    }
}
