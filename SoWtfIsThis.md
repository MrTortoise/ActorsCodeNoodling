****Space Trading AI Learning project


# So WTF is this?

Several projects are overlapping.

1. **Prcedural Generation**: Procedurally generated everything based on sampling of curves to produce tables from which to generate items and locations.
1.  **Inter?-Galactic SimCity**: Placing of various complexes to exploit resource distribution with the aim of production and growth.
1. **Trading**: Some kind of trading platform simulation. Goods production and trade for other goods to achive some end. 
1. **Games**: a set of games which represent ends to the above trading (and potentially replace).

I see this as also being a general order of implementation.
It is roughly analogous to a drilling down into the scale.

## Locations:

1. The universe will consist of clumps of systems seperated by distance.
	- using large data types allows distances to be measured fairly close to a univer scale.
	- This is way beyond any fun usefulness. But it does mean given good datatypes (oct tree or r*) it may be possible to using actor systems to dynamically partition this based on density. 
	- I want to build a system that scales sufficiently that the only limitation on size is bigint^3. 
	- It will have the intersting side effect of being able to decide where to start ... where there are others or on your own?

2. As all these things are procedurally generated it will be possible to define vague structures ... eg 2 galaxies or the clumpiness of systems within galaxies.
	- Because nothing is directly connected distance becomes the effective connection.

1. Within a solar system there will be a number of center of masses (ie suns) and orbiting bodies.
	- Orbiting bodies may have orbiting bodies.
	
1. Orbiting bodies and possibly center of masses have resources.

##Economy:

1. The main idea behind economy is not to have a currency backing it - or more to the point the base currency item beign highly consumable. 
	- I'd expect other consumable currencies to become the standard in trading. See POE for example.
1. Trading will therefor be batering for goods. 
1. Trading of promises with collateral. Opens up possibility of stealing a promise. Gambling etc.

##Production: 
The find resource, build factory, sell shit, get rich ... profit. thing has been done to death. Production itself needs to be interesting enough to be a game. 
Eg 
1. you combine thigns and get what you want but also waste. 
2. How do you deal with the waste? this will all have side effects.
	-    Maybe waste can be dumped -> lower living conditions -> shitty workers.
3. If workers then they need food, where do they come from?
4. If process waste what do you get out of that?
    - Nasty stuff and something more consumable?
    - When stuff gets sufficiently nasty -> weapons.

So now your goal is to create an ecosystem that produces lots of workers to goto work purifying nasty shit (and dying) that has a side effect of producing the items for the peopele who want to blow shit up. By makign some thign be waste that cannto be processed (maybe it has an innate decay rate) you build interesting long term feedback cycles that cause the environment to be challenging and changing.

Items:
A very very broad category. Probably terrible, needs breaking into categories. Eg are factories, worker complexes items?
Eg promises and contracts are definatley items.
