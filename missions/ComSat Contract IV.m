Mission
{
    name = ComSat Contract IV
    description = We signed a contract to bring a satellite into a very specific orbit around Kerbin. Their orbit is really odd...
    repeatable = true
    reward = 200000
    randomized = true

    clientController = true
    lifetime = TIME(1y)

    category = SATELLITE, ORBIT

    OrbitGoal
    {
        body = Kerbin

        minApA = RANDOM(1000000, 2000000)
        maxApA = ADD(minApA, 10000)

        maxEccentricity = 0.0001

        minInc = RANDOM(0, 178)
        maxInc = ADD(minInc, 0.25)

        minLan = RANDOM(0, 179.4)
        maxLan = ADD(minLan, 0.5)
    }
}
