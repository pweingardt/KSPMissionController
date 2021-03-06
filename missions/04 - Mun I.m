Mission
{
    name = Mun I
    description = They say you should get comfortable with unmanned missions before we decide to bring a kerbal into space. We always wondere what is on the other side of the Mun, but because it is tidally locked, we need a probe. We want you to build a rocket that is able to reach a stable orbit around Mun. It does not have an atmosphere, no need to build a rocket with sensors on it. Bring the probe back by the way... you need to exercise orbital maneuvers before we trust you enough with manned missions.

    reward = 80000
    category = ORBIT

    requiresMission = Sputnik III
    packageOrder = 4

    OrbitGoal
    {
        body = Mun

        minPeA = 10000
        maxApA = 900000
    }

    LandingGoal
    {
        body = Kerbin
    }
}
