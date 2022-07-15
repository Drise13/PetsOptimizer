# PetsOptimizer

`PetsOptimizer` is a genetic algorithm that produces high quality pet arrays for the Breeding skill in the game "Legends of Idleon".

<sup><sub>I'm also jealous of the name `Cogstruction` (an inspiration for this project) and am soliciting ideas for a better "clever" name.</sub></sup>

# Dependencies

We rely on having access to the JSON save data file used by Idleon. This data file contains all the information about your current game state including what pets you have, what territories you have unlocked, and what breeding upgrades you have. We rely on using https://github.com/desophos/idleon-saver to get this data file. For our purposes, we can use the data it saves directly without having to use an "export" method. Idleon-Saver puts your save file in `%appdata%/IdleonSaver/idleon_save.json` where we can read and get the information we need. Simply follow the instructions to run the application and we should be able to find the file. If you wish to provide an alternate file location, you can run the program with the `file` flag set. See `Running the application` for more details.

# Running the application

The application has a few options to make adjustments to how it runs.

A complete option example is `petsoptimizer.exe -f C:/myjsonfile.json -r 5 -i 1500 -p 5000`. These values are all optional and have reasonable defaults provided by the application.

## File Path: `f[ile]`

### Default Value: `%appdata%/IdleonSaver/idleon_save.json`

Specifies an alternate file path to use instead of the default location. Can be relative or absolute.

Expects the JSON nodes `Pets`, `PetsStored`, `Territory`, and `Breeding` to be available. Each are expected to be a parsable JSON array of values.

## Runs: `r[uns]`

### Default Value: `1`

Allows the application to perform several distinct runs and select the best of the best of each run. Increases time to produce an output, so 5 runs would take 5 times as long. In my testing, I found a run with the default values takes approximately 50 seconds. There are some multithreaded optimizations, so this will likely vary based on the cpu core count of your machine.

## Iterations: `i[terations]`

### Default Value: `1500`

How many iterations run the algorithm. In my testing, I generally found I had converged by iteration 500-600, with minor incremental improvements through iteration 1200. Increases run time, but generally provides higher quality output with diminishing returns. 5000 iterations is highly unlikely to be better than 2000.

## Population size: `p[opulation]`

### Default Value: `5000`

How many pet arrays to test each iteration. The more pet arrays, generally the better, but it comes with an increased time per iteration. 5000 is probably on the excessive side, but gives a decent variety for the algorithm to work with. I found that with 5000 pet arrays, I had an average iteration time of 25ms.

# How it works

A genetic algorithm is based off the principle of natural selection. The algorithm works as follows.

1. Randomly instatiate a large collection of pet arrays, say 5000.
2. Score each pet array based on how effective it is. For this algorithm, it prioritizes putting higher strength teams in later territories which require more foraging power per item gained.
3. Sort the population according to the score.
4. Remove the bottom 50% of pet arrays.
5. Create a new pet array from each of the existing pet arrays.
    1. When creating the new pet array, Mutate it.
    2. Mutate a pet array by randomly selecting 1-5 of all available pets. If the pet is not in a territory, pick a random territory and randomly assign it to a position. If the pet is in a territory, pick another random territory and random position and swap the two pets.
6. Add the new pet array to the existing collection.
7. Repeat this some number of times, say 1500 times, until the arrays converge to an optimized solution.
8. Print the best pet array.

There is also a mechanism to run the algorithm several times and choose the best of the best of each run. Genetic algorithms can get stuck in what's called `local maximums`, where the "best" is optimal given its current values and minor tweaks to those values, but is not the `global maximum` choice. Several runs gives the opportunity to explore other optimization routes and pick the best route. However, the cost to this more global maximum is more time spent.

# Results

Results are written to the execution directory, so if you run the application in `C:/My/Special/Folders/` it will output `best-result.txt` in that folder. It prints each territory with each pet in the specific position (top down equals left to right in-game). Pet positions within a territory are important since there are above / below interactions to keep in mind. It also prints the expected territory forage power, this should match what you see in game, at least until Lava add some gem-based boost modifiers like he has with Flaggy Rate in Construction. If the rate doesn't match (with adjustments for rounding), open an issue with you JSON save file and I can take a look when I have time.