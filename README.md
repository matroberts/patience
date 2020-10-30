# Patience
A text based version of the card game patience (klondike).

References:
* https://bicyclecards.com/how-to-play/klondike/
* http://web.engr.oregonstate.edu/~afern/papers/solitaire.pdf
* http://www.jupiterscientific.org/sciinfo/KlondikeSolitaireReport.html
* https://statweb.stanford.edu/~cgates/PERSI/papers/solitaire.pdf

Definitions:
* Stock (Pile, Reserve)  - Where the cards are drawn from
* Talon (Waste)          - Where the cards get put after their drawn
* Foundation             - Where the Aces etc. get stacked up in order
* Tableau                - The seven stacks of cards


## How describe the moves?
It's a text based interface.  Options for describing the moves are:

1. You could try to name where you are moving the card from and too  

         e.g. W(aste)    > T(ableau)5
              T(ableau)5 > F(oundation)

   but moving stacks around the Tableau is difficult to describe

2. Instead you could name the card you are moving, and where you are moving it too

         e.g. 3H        > F(oundation)
		      3H        > T(ableau)

   This way it doesn't matter where the card currently is (Waste, Foundation or Tableau) only that it is visible.
   It allows you to describe moving stacks in the tableau in a simple way.
   
   There are potentially two locations in the tableau where the a card can go, for example the 3 Hearts can go on the 4 Clubs or 4 Spades
   So you would (optionally) want to be able to specify the second location for the move 

        e.g.  3H        > T2 or TT

   There should have to be some smartness to make this work natually, so when you are moving cards between stacks it always chooses the other stack (rather than you have to explicitly say T1 or T2)
   This smartness would also mean you could always avoid using the T2 command by just applying the move twice if it went to the wrong place
	
3. You could go the whole hog and describe a move as the card you are moving and the card you are putting it on
   
        e.g.  3H        > 2H  (would move the 3H into the foundation)
		      3H        > 4C  (would move the 3H onto the 4C in the tableau)

   But then moving an Ace into the foundation, or a king onto the tableau becomes unnatural.

So the second option looks the best.


The operations on the text based interface are then described like this:
 
      <Operation> ::= <Action> | <Move>

	  <Action>    ::= U (Undo), R (Redo), D (Deal)

	  <Move>      ::= <Rank><Suit><Location>

	  <Suit>      ::= C,D,H,S
	  <Rank>      ::= A,2,3,4,5,6,7,8,9,10,J,Q,K
	  <Location>  ::= F(oundation) or T(ableau) or TT (Tableau location 2)



ctor Klondike (InternalLayout)
Layout Klondike(string move);

InternalLayout {
	Foundation: [[], [], [], []]
	Tableau: [[], [], [], [].....]
	Stock: []
	Talon: []
}
