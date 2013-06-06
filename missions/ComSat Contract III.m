Mission
{
    name = ComSat Contract III
    description = We signed a contract to bring a satellite into a nearly perfect synchronous, equatorial orbit around Kerbin.
    repeatable = true
    reward = 100000

    OrbitGoal
    {
        body = Kerbin

        minApA = 2866750
        maxApA = 2870750

        maxEccentricity = 0.0001

        maxInc = 0.02

        minOrbitalPeriod = TIME(5s 59m 55s)
        maxOrbitalPeriod = TIME(6h 5s)
    }
}
