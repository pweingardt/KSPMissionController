Priority:
1. Geckgo filter code
2. Spaceplanes
3. Mission goal completion stored/tracked on other missions loaded





DONE - Apply costs to parts at load
Expose cost coefficients via debug menu
PARTIAL - Recycle on unload
Add support for ModuleReactionWheel
Add support for mods' modules? RT, Kethane. DRE handled already.


Earlier thoughts:
1. Make MC compatible with save and loading of games--store MC data in the SFS files so mission status and budget are stored and loaded. If that doesn’t work, hijack quicksave to write an additional SP file, and quickload to load it. WAITING ON KETHANE TO SEE HOW
2. New testing mode: under testing mode, when you launch, MC saves to a special pretest.sfs file. Disable exit to space center and the end flight/recover flight dialog. Allow only new option, End Test. That reverts to pretest.sfs, so NO universe changes are kept during testing mode.
Long term, I’d love the “simulator” to be another thing you could buy, and early on you’d have to do _real_ tests. Computers are newer than rockets…
(Those real tests, of course, would have real costs--but since payload accounts for much of the rocket’s cost now, the costs would already be lower, since you’d launch with boilerplate testing payloads. Further, you’d do ground tests of payload and those would be recoverable so you wouldn’t burn cash.)


Another goal: make hiring Kerbals cost MC money (training costs). SALARIES.

Add cost increase if launch too fast, cost decrease if launch same rocket more than once
Reliability that is low for early rockets, scales up for later ones

Stats screen for tracking completed missions, etc.

R&D