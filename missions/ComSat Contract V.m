Mission
{
    name = ComSat Contract V
    description = We signed a contract to bring a satellite into a nearly perfect synchronous, equatorial orbit around Kerbin. They also specified the position of the satellite above the ground.
    repeatable = true
    repeatableSameVessel = false
    reward = 100000
    randomized = true

    requiresMission = Kerbolo I

    clientControlled = true
    lifetime = TIME(1y)

    category = SATELLITE

    packageOrder = 104

    OrbitGoal
    {
        nonPermanent = false
        body = Kerbin

        minApA = 2866750
        maxApA = 2870750

        maxEccentricity = 0.0001

        maxInclination = 0.02

        minLongitude = RANDOM(-180, 179.5)
        maxLongitude = ADD(minLongitude, 0.5)

        minOrbitalPeriod = TIME(5h 59m 55s)
        maxOrbitalPeriod = TIME(6h 5s)
    }
}
