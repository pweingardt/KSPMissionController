Mission
{
    name = Mun X
    description = Bring a manned space craft onto the surface of the Mun and bring him back.
    reward = 200000

    OrbitGoal
    {
        crewCount = 1
        reward = 10000
        body = Kerbin
        minPeA = 70000
        maxEccentricity = 1
    }

    OrbitGoal
    {
        crewCount = 1
        reward = 15000
        body = Mun
        minPeA = 3000
        maxEccentriciy = 1
    }

    LandingGoal
    {
        crewCount = 1
        body = Mun
    }

    OrbitGoal
    {
        reward = 15000
        crewCount = 1
        body = Mun
        minPeA = 3000
        maxEccentriciy = 1
    }

    LandingGoal
    {
        crewCount = 1
        body = Kerbin
    }
}
