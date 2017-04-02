#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <ncurses.h> 
#include <pthread.h> 
#include <sys/types.h>
#include <semaphore.h>

#define MAX_HEIGHT 6
#define MAX_WIDTH 6
#define MAX_MOLES 36


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
int mole_down_duration;
int mole_up_duration;
int semId;
int yMax, xMax;
bool isPlaying = true;
char *targets;
char **board;

struct game_settings {
	int grid_height;
	int grid_width;
	int num_moles;
	int max_active_moles;
};

// Used to modify mole up and down durations
typedef enum { EASY, MEDIUM, HARD } difficulty;
difficulty currentDifficulty;

/* Prototypes */
void* moleQueue(void *settings);
void moleHitHandler(int signal);
void welcomeScreen(int row, int col);
char* getDifficulty();
void setMoleDifficulty();
void updateBoard(int width, int height);

/* Mutex to protect access to hit counter and moles in */
pthread_mutex_t hit_mutex;
pthread_mutex_t moles_active_mutex;

/* Semaphore for mole synching */
sem_t mole_active_enter;


/* Main entry point for program */
int main (int argc, char *argv[])
{
	/* Check proper arguments */
	// argv[0] - Program
	// argv[1] - Grid height - MAX 6
	// argv[2] - Grid width - MAX 6
	// argv[3] - Num of moles - MAX 36
	// argv[4] - Max moles 'up' at once 
	if(argc != 5){
		fprintf(stderr, "There are not enough arguments\n\n");
		printf("Execute with: Board Height, Board Width, # of Moles, # of MAX Active Moles\n\n");
		exit(1);
	}
	struct game_settings gameSettings;
	int status;

	/* Init Mutex */

	/* Semaphore */

	// Set game settings
	gameSettings.grid_height = atoi(argv[1]);
	gameSettings.grid_width = atoi(argv[2]);
	gameSettings.num_moles = atoi(argv[3]);
	gameSettings.max_active_moles = atoi(argv[4]);

	// TODO: Validation on game settings (i.e. max active moles !> num moles)

	/* NCURSES COORDINATES TAKE Y FIRST, FOLLOWED BY X */
	

	initscr();
	noecho();
	curs_set(0);
	// Get Window Size
	getmaxyx(stdscr, yMax, xMax);
	
	welcomeScreen(yMax, xMax);
	printw("Your selected difficulty: %s", getDifficulty());


	setMoleDifficulty();

	move(1, 0);
	
	printw("Board Size: %d", gameSettings.grid_width*gameSettings.grid_height);
	move(1,0);

	targets = "abcdefghijklmnopqrstuvwxyz0123456789";

	// Allocate space for game board
	board = malloc(sizeof *board * gameSettings.grid_height);
	int i, j;
	if(board){
		for(i = 0; i < gameSettings.grid_width; i++){
			board[i] = malloc(sizeof *board[i] * gameSettings.grid_width);
		}
	}

	
	printw("Press any key to continue");
	getch();
	clear();

	/* PRINT LARGE WHACK A MOLE TEXT HERE */
	int c;
	FILE *file;
	file = fopen("moleArt.txt", "r");
	if(file){
		while((c = getc(file)) != EOF){
			addch(c);
		}
	}

	int gridX = xMax/3;
	int gridY = yMax/2 - 5;
	move(gridY, gridX);
	for(i = 0; i < gameSettings.grid_width; i++){
		for(j = 0; j < gameSettings.grid_height; j++){
			board[i][j] = '_';
			move(gridY+j*2, gridX+(i*5));
			printw("%c", board[i][j]);;
		}
	}
	
	pthread_t threads[gameSettings.num_moles];

	//for(i = 0; i < gameSettings.num_moles; i++){
	for(i = 0; i < 1; i++){
		if((status = pthread_create(&threads[i], NULL, moleQueue, (void*)&targets[i])) != 0){
			fprintf(stderr, "mole create error %d: %s\n", status, strerror(status));
			exit(1);
		}
	}
	// printw("YOOOOOOO");
	// while(isPlaying){
	// 	char input;
	// 	//updateBoard(gameSettings.grid_width, gameSettings.grid_height);
	// 	if((input = getchar()) == 0x1B){
	// 		isPlaying = false;
	// 	}
	// }
	
	//Set board and create threads (moles)

	for(i = 0; i < 1; i++){
		pthread_join(threads[i], NULL);
	}

	// // Clean UP
	// pthread_mutex_destroy(&hit_mutex);
	// pthread_mutex_destroy(&moles_active_mutex);
	// sem_destroy(&mole_active_enter);
	updateBoard(gameSettings.grid_width, gameSettings.grid_height);

	getch();
	endwin();

	free(board);

	return 0;
}

/******************************************************
* Function for Moles to execute and loop in
*
*******************************************************/
void* moleQueue(void *target){
	char *moleHead = target;
	board[3][3] = *moleHead;
	// while(isPlaying){
	// 	board[3][3] = *moleHead;
	// 	// wait random amount of time
	// 	// try to enter semaphore
	// 	// access mutex
	// 	// do mole things
	// 	// leave semaphore
	// 	// repeat
	// }

	return 0;
}

/******************************************************
* Update board and draw changes from Moles
*******************************************************/
void updateBoard(int width, int height){
	int gridX = xMax/3;
	int gridY = yMax/2 - 5;
	move(gridY, gridX);

	// Draw game board
	int i, j;
	for(i = 0; i < width; i++){
		for(j = 0; j < height; j++){
			move(gridY+j*2, gridX+(i*5));
			printw("%c", board[i][j]);;
		}
	}
}

/******************************************************
* Return an string value corresponding to current 
* difficulty
*******************************************************/
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

/******************************************************
* Draws welcome screen and propts user to select 
* difficulty
*******************************************************/
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

/************************************************************
* Set mole up and down times based on difficulty set by users
*************************************************************/
void setMoleDifficulty(){
	switch(currentDifficulty){
		case EASY:
			mole_down_duration = 3;
			mole_up_duration = 3;
		case MEDIUM:
			mole_down_duration = 2;
			mole_up_duration = 2;
		case HARD:
			mole_down_duration = 1;
			mole_up_duration = 1;
		default:
			mole_down_duration = 3;
			mole_up_duration = 3;
	}
}