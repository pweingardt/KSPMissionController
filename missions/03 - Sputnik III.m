Mission
{
    name = Sputnik III
    description = The scientists are happy now, well done. They think that our first probe has been damaged and the radiation sensors were broken. Everything is fine now. Our next goal is to get outside Kerbins magnetic field. Reach a stable orbit somewhere there. No need to bring the probe back...

    reward = 40000
    category = ORBIT, PROBE

    requiresMission = Sputnik II
    packageOrder = 3

    SubMissionGoal
    {
        description = Reach a stable orbit around Kerbin outsides its magnetic field. You will need 3 Communotron 16.

        OrbitGoal
        {
            body = Kerbin
            minPeA = 400000
            maxApA = 1200000
        }

        PartGoal
        {
            partName = longAntenna
            partCount = 3
        }
    }
}
