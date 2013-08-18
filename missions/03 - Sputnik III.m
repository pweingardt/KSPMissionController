Mission
{
    name = Sputnik III
    description = The data from your second mission has been analyzed and the readings were within the expected range. Additional research on the original craft suggests that its radiation sensors were faulty. We now have sufficient data to authorize a third mission, this time to deliver a permanent satellite to a higher orbert beyond the range of Kerbin's magnetic field.

    reward = 60000
    category = ORBIT, PROBE

    requiresMission = Sputnik II
    packageOrder = 3

    SubMissionGoal
    {
        description = Achieve a stable orbit around Kerbin outside of its magnetic field. You will need 3 Communotron 16.

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
