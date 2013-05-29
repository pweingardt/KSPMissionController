Mission
{
    name = Mun I
    description = Mun flyby. Bring a satellite into a close Mun escape trajectory 4-6m above Muns surface. Optional: Bring the probe back to Kerbin afterwards!
    reward = 20000

    OrbitGoal
    {
        body = Mun

        minEccentricity = 1
        minPeA = 4000
        maxPeA = 6000
    }

    LandingGoal
    {
        reward = 5000

        optional = true
        body = Kerbin
    }

}
