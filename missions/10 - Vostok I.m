Mission
{
    name = Vostok I
    description = Since your successful landing on Mun, training has begun to prepare skilled Kerbal astronaunts to man spacecraft. It has been determined that we are now prepared to attempt such a manned mission. Your missions will have an increased cost in krones for astronauts to cover their insurance; a successful return to the surface will return this significant cost to your budget.

    reward = 95000
    category = ORBIT, MANNED

    requiresMission = Kerbolo I
    packageOrder = 10

    SubMissionGoal
    {
        description = Bring a manned probe into a suborbital flight around Kerbin. Reach an altitude of at least 70km.
        crewCount = 1

        OrbitGoal
        {
            body = Kerbin
            minAltitude = 70000
        }

        PartGoal
        {
            partName = longAntenna
            partCount = 3
        }
    }

    LandingGoal
    {
        body = Kerbin
    }
}
