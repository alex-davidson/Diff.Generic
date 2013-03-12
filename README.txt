Diff.Generic
------------

Based on the DiffPlex project at CodePlex: http://diffplex.codeplex.com/
DiffPlex is under the Microsoft Public License (Ms-PL).

I have a need for a more abstract diff implementation, capable of operating
on collections of any type for which equality is defined. DiffPlex provides
an implementation which works nicely for strings and includes additional
tooling around rendering the results. Diff.Generic is intended to distill
from DiffPlex a pure diffing library which is not string-oriented and has
nothing UI-related in it at all.

Initial work:
 * cloned the core engine
 * removed anything remotely UI-related
 * made the diffing code generic

To do:
 * general cleanup. I don't fully understand how the diffing algorithm works,
   so I intend to rearrange the code until I do.





