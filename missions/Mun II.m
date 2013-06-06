Mission
{
    name = Mun II
    description = Mun orbit. Bring a satellite into a stable orbit around Mun, and if possible, bring the probe back to Kerbin.
    reward = 80000

    OrbitGoal
    {
        description = Minimal periapsis is 4000 meters.
        body = Mun

        maxEccentricity = 1
        minPeA = 4000
    }

    LandingGoal
    {
        reward = 10000

        optional = true
        body = Kerbin
    }
}
