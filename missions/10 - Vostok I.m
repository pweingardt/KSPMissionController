Mission
{
    name = Vostok I
    description = It is time. It is time to send our first Kerbonaut into space. We started a new policy: Before you launch a manned vessel you pay a certain amount of krones for each kerbonaut on the vessel. Don't panic, you will get those krones back once the kerbonauts land safely on Kerbin. And keep in mind, that you should bring them back.

    reward = 65000
    category = ORBIT, MANNED

    requiresMission = Kerbolo I
    packageOrder = 10

    SubMissionGoal
    {
        description = Bring the manned probe into a suborbital flight around Kerbin. Reach at least 70km altitude.
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
