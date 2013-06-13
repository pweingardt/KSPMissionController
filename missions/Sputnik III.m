Mission
{
    name = Sputnik III
    description = Bring a small satellite into a stable orbit around Kerbin and return safely back to Kerbins surface.
    reward = 24000

    category = PROBE, LANDING

    OrbitGoal
    {
        reward = 2000

        body = Kerbin
        minPeA = 70000
        maxEccentricity = 1
    }

    LandingGoal
    {
        reward = 2000

        body = Kerbin
    }
}
