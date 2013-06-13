Mission
{
    name = ComSat Contract V
    description = We signed a contract to bring a satellite into a very specific orbit.
    repeatable = true
    reward = 10000
    randomized = true

    passiveMission = true
    passiveReward = 500

    lifetime = TIME(150d)
    clientControlled = true

    category = SATELLITE, ORBIT

    OrbitGoal
    {
        body = Kerbin

        minApA = RANDOM(100000, 200000)
        maxApA = ADD(minApA, 5000)

        maxEccentricity = 0.001

        minInc = RANDOM(0, 88)
        maxInc = ADD(minInc, 0.5)
    }
}
