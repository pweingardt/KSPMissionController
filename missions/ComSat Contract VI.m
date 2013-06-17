Mission
{
    name = ComSat Contract VI
    description = We signed a contract to bring a satellite into a nearly perfect synchronous, equatorial orbit around Kerbin. They also specified the position of the satellite...
    repeatable = true
    reward = 100000

    clientControlled = true
    lifetime = TIME(1y)

    category = SATELLITE, ORBIT

    OrbitGoal
    {
        body = Kerbin

        minApA = 2866750
        maxApA = 2870750

        maxEccentricity = 0.0001

        maxInclination = 0.02

        minLongitude = -70.0
        maxLongitude = -69.5

        minOrbitalPeriod = TIME(5h 59m 55s)
        maxOrbitalPeriod = TIME(6h 5s)
    }
}
