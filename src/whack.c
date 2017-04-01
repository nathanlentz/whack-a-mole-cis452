#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <ncurses.h> 
#include <pthread.h> 
#include <sys/types.h>


/************************************************************
* WHACK A MOLE (CIS 452 - Project 3)
* @author Nathan Lentz
* @date 4/1/2017
* 
* Project demonstrates proficiency with concurrency
* through the use of multiple threads to whack moles
* See project at github.com/nathanlentz/whack-a-mole-cis452
* 
* NOTE: Compile with '-lpthread' & '-lncurses'
*************************************************************/

/* Properties / Global Variables */ 
int mole_up_duration;
int mole_down_duration;

struct mole_settings {
	// X Position on screen
	int column;
	// Y Position on screen
	int row;
	// Mole Whack Value
	char target;
};

struct game_settings {
	int grid_height;
	int grid_width;
	int num_moles;
	int max_active_moles;
};


// Used to modify mole up and down durations
typedef enum { EASY,MEDIUM, HARD } difficulty;
difficulty currentDifficulty;

/* Prototypes */
void* mrMole(void *settings);
void moleHitHandler(int signal);
void welcomeScreen(int row, int col);
char* getDifficulty();


/* Main entry point for program */
int main (int argc, char *argv[])
{
	/* Check proper arguments */
	// argv[0] - Program
	// argv[1] - Grid height
	// argv[2] - Grid width
	// argv[3] - Num of moles
	// argv[4] - Max moles 'up' at once
	if(argc != 5){
		fprintf(stderr, "There are not enough arguments\n\n");
		printf("Execute with: Board Height, Board Width, # of Moles, # of MAX Active Moles\n\n");
		exit(1);
	}
	struct game_settings settings;

	settings.grid_height = atoi(argv[1]);
	settings.grid_width = atoi(argv[2]);
	settings.num_moles = atoi(argv[3]);
	settings.max_active_moles = atoi(argv[4]);

	// TODO: Validation on game settings (i.e. max active moles !> num moles)

	/* NCURSES COORDINATES TAKE Y FIRST, FOLLOWED BY X */
	int yMax, xMax;

	initscr();
	noecho();
	// Get Window Size
	getmaxyx(stdscr, yMax, xMax);
	// Get difficulty from welcome screen
	welcomeScreen(yMax, xMax);
	printw("Your selected difficulty: %s", getDifficulty());
	move(1, 0);
	printw("Board Size: %d", settings.grid_width*settings.grid_height);
	move(1,0);
	printw("Press any key to begin playing");




	getch();
	clear();

	getch();
	endwin();



	return 0;
}

char* getDifficulty()
{
	switch(currentDifficulty){
		case EASY:
			return "EASY";
			break;
		case MEDIUM:
			return "MEDIUM";
			break;
		case HARD:
			return "HARD";
			break;
		default:
			break;
	}
	return "no difficulty";
}

void welcomeScreen(int yMax, int xMax)
{
	
	char* title = "WELCOME TO WHACK A MOLE";
	int titleSize = strlen(title);
	move(10, xMax/2 - (titleSize/1.5));
	printw(title);
	move(1,0);
	WINDOW * menuwin = newwin(6, titleSize, yMax - 15, xMax/2 - (titleSize/1.5));
	box(menuwin, 0,0);
	refresh();
	wrefresh(menuwin);
	keypad(menuwin, true);

	const char *choices[2];
	choices[0] = "EASY";
	choices[1] = "MEDIUM";
	choices[2] = "HARD";

	int choice;
	int highlight = 0;
	// Menu Stuff for choosing difficulty
	while(1){
		int i;

		for(i = -1; i < 3; i++){
			if(i == -1){
				mvwprintw(menuwin, i+1, 1, "Select a difficulty:");
				i++;
			}
			if(i == highlight){
				wattron(menuwin, A_REVERSE);
			}
			mvwprintw(menuwin, i+1, 1, choices[i]);
			wattroff(menuwin, A_REVERSE);

		}
		choice = wgetch(menuwin);
		switch(choice){
			case KEY_UP:
				highlight--;
				if(highlight == -1){
					highlight = 0;
				}
				break;
			case KEY_DOWN:
				highlight++;
				if(highlight == 3){
					highlight = 2;
				}
				break;
			default:
				break;
		}
		if(choice == 10){
			break;
		}
	}
	switch(highlight){
		case 0:
			currentDifficulty = EASY;
			break;
		case 1:
			currentDifficulty = MEDIUM;
			break;
		case 2:
			currentDifficulty = HARD;
			break;
		default:
			break;
	}

	// Free up memory from window
	delwin(menuwin);
	clear();

}
