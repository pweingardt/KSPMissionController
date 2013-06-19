Mission
{
    name = ComSat Contract V
    description = We signed a contract to bring a satellite into a very specific orbit.
    repeatable = true
    reward = 10000
    randomized = true

    requiresMission = Kerbolo I

    passiveMission = true
    passiveReward = 500

    lifetime = TIME(150d)
    clientControlled = true

    category = SATELLITE

    packageOrder = 100

    OrbitGoal
    {
        nonPermanent = false
        body = Kerbin

        minApA = RANDOM(100000, 200000)
        maxApA = ADD(minApA, 5000)

        maxEccentricity = 0.001

        minInclination = RANDOM(0, 88)
        maxInclination = ADD(minInclination, 0.5)
    }
}
