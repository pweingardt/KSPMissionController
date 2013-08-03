Apply costs to parts at load
Expose cost coefficients via debug menu
Recycle on unload
Add support for ModuleReactionWheel
Add support for mods' modules? RT, Kethane. DRE handled already.


Earlier thoughts:
1. Make MC compatible with save and loading of games--store MC data in the SFS files so mission status and budget are stored and loaded. If that doesn’t work, hijack quicksave to write an additional SP file, and quickload to load it.
2. New testing mode: under testing mode, when you launch, MC saves to a special pretest.sfs file. Disable exit to space center and the end flight/recover flight dialog. Allow only new option, End Test. That reverts to pretest.sfs, so NO universe changes are kept during testing mode.
Long term, I’d love the “simulator” to be another thing you could buy, and early on you’d have to do _real_ tests. Computers are newer than rockets…
(Those real tests, of course, would have real costs--but since payload accounts for much of the rocket’s cost now, the costs would already be lower, since you’d launch with boilerplate testing payloads. Further, you’d do ground tests of payload and those would be recoverable so you wouldn’t burn cash.)

Next long term goal: check whenever a spacecraft is about to be auto-destroyed on falling too low on Kerbin, whether (a) it has enough delta-V for landing and a command pod, or (b) it has enough parachutes, and if so count it as recycled. No more having to ride your spent stages down! You’d get slightly less back than if you did it manually though.

Another goal: make hiring Kerbals cost MC money (training costs). SALARIES.

Add cost increase if launch too fast, cost decrease if launch same rocket more than once
Reliability that is low for early rockets, scales up for later ones

Stats screen for tracking completed missions, etc.

R&D