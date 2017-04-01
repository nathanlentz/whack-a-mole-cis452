// short demo of screen control using ncurses
// gw
// note: compile with -lncurses

#include <ncurses.h> 
 
int main()
{
	int row, col;
 
	initscr();
	// determine window size
	getmaxyx (stdscr, row, col);
	// move to middle of window
	move (row/2, col/2);
	printw("hello, world.");

	getch();
	endwin();
	return 0;
}