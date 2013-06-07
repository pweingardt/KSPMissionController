## Interface to access the Manager from your plugin

If you want to support the mission controller in your plugin, you have to reference the MissionLibrary.dll
(it is inside the lib folder) from your project. Use the method `ManagerAccessor.get`
(you may have to use the `MissionController` namespace) to access the interface.

Methods so far:

* `int getBudget()`: returns the current budget
* `int reward(int value)`: adds the passed value to the budget and returns the new budget
* `int costs(int value)`: subtracts the passed value from the budget and returns the new budget

# ATTENTION

Every time you call `reward` or `costs` the current space program will be written on the disk,
don't call it every second.
