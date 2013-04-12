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
 * factored out some components of the algorithm
 

DiffPlex's Operation
--------------------

The stages of operation are:
 1. Reduce each input to a stream of chunks.
 2. Translate the chunks to integers, retaining equality.
 3. Find a sufficiently-optimal edit path.
 4. For each chunk in each stream, determine if it is involved in an edit.
 5. Aggregate adjacent edits.

DiffPlex implements a diffing algorithm which operates on two lists of
integers. Chunk streams are translated to integer lists via a hash function
which retains the equality relation. This means that changing the equality
relation only affects the initial translation and has no bearing at all on
the implementation or efficiency of the diffing algorithm.

The hash function's implementation is a dictionary which simply returns the
next unused integer when it sees a new chunk value. An interesting side
effect of this is that no integer in stream A (left side, or 'old' side) will
ever be larger than its position in the array. This may prove useful for
optimisations.

The diffing algorithm devises an edit path which is presumably close to
minimal, ultimately producing a boolean value for each chunk of each stream
indicating whether or not that chunk was 'modified' in that edit path. This
information is then used to combine adjacent edited chunks into edit blocks.
Each block has a deletion list and an insertion list.





