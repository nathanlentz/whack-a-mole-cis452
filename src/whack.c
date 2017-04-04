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
int isPlaying = 0; 	// 0: true, 1: false
char *targets;
char **board;
int molesHit;
int molesMissed;
int boardWidth;
int boardHeight;
int inaccurateHits;

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
void* gameTracker(void *params);
void moleHitHandler(int signal);
void welcomeScreen(int row, int col);
char* getDifficulty();
void setMoleDifficulty();
void updateBoard(int width, int height);

/* Mutex to protect access to hit counter and moles in */
static pthread_mutex_t hit_mutex = PTHREAD_MUTEX_INITIALIZER;
static pthread_mutex_t moles_missed_mutex = PTHREAD_MUTEX_INITIALIZER;

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

	// Set game settings
	gameSettings.grid_height = atoi(argv[1]);
	gameSettings.grid_width = atoi(argv[2]);
	gameSettings.num_moles = atoi(argv[3]);
	gameSettings.max_active_moles = atoi(argv[4]);
	molesHit = 0;
	molesMissed = 0;
	inaccurateHits = 0;

	boardWidth = gameSettings.grid_width;
	boardHeight = gameSettings.grid_height;

	/* Init Mutex */
	//pthread_mutex_init(&hit_mutex, NULL);
	//pthread_mutex_init(&moles_missed_mutex, NULL);

	/* Semaphore */
	int res = sem_init(&mole_active_enter, 0, gameSettings.max_active_moles);
	if(res < 0){
		perror("Semaphore init failure");
		exit(1);
	}

	// TODO: Validation on game settings (i.e. max active moles !> num moles)

	/* NCURSES COORDINATES TAKE Y FIRST, FOLLOWED BY X */
	

	initscr();
	noecho();
	curs_set(0);
	// Get Window Size
	getmaxyx(stdscr, yMax, xMax);
	
	welcomeScreen(yMax, xMax);


	setMoleDifficulty();

	targets = "abcdefghijklmnopqrstuvwxyz0123456789";

	// Allocate space for game board
	board = malloc(sizeof *board * gameSettings.grid_height);
	int i, j;
	if(board){
		for(i = 0; i < gameSettings.grid_width; i++){
			board[i] = malloc(sizeof *board[i] * gameSettings.grid_width);
		}
	}

	move(yMax-1,0);
	printw("Your selected difficulty: %s", getDifficulty());

	/* PRINT LARGE WHACK A MOLE TEXT HERE */
	move(0,0);
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
			refresh();
		}
	}

	getch();
	
	pthread_t threads[gameSettings.num_moles];
	pthread_t trackerThread;

	if((status = pthread_create(&trackerThread, NULL, gameTracker, NULL)) != 0){
			fprintf(stderr, "Game Tracker Error %d: %s\n", status, strerror(status));
			endwin();
	}
	for(i = 0; i < gameSettings.num_moles; i++){
		if((status = pthread_create(&threads[i], NULL, moleQueue, (void*)&targets[i])) != 0){
			fprintf(stderr, "mole create error %d: %s\n", status, strerror(status));
			endwin();
		}
	}

	while(isPlaying == 0){
		updateBoard(gameSettings.grid_width, gameSettings.grid_height);
	}


	pthread_join(trackerThread, NULL);
	for(i = 0; i < 1; i++){
		pthread_join(threads[i], NULL);
	}


	clear();

	move(0,0);
	printw("All moles gone for now, press key to exit");
	refresh();
	getch();


	// // Clean UP
	pthread_mutex_destroy(&hit_mutex);
	pthread_mutex_destroy(&moles_missed_mutex);
	sem_destroy(&mole_active_enter);


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
	int waitUp;
	int waitDown;
	int row;
	int column;
	int spotFound;
	//board[3][3] = *moleHead;
	while(isPlaying == 0){
		spotFound = 0;
		waitUp = rand() % (mole_down_duration + 1 - 0);
		waitDown = rand() % (mole_up_duration + 1 - 0);

		sleep(waitUp);
		// Wait to access board
		sem_wait(&mole_active_enter);
		while(spotFound == 0){
			row = rand() % (boardHeight + 1 - 1);
			column = rand() % (boardWidth + 1 - 0);
			if(board[row][column] == '_'){
				board[row][column] = *moleHead;
				spotFound = 1;
			}
		}
		
		sleep(waitDown);
		if(board[row][column] == *moleHead){
			pthread_mutex_lock(&moles_missed_mutex);
			molesMissed++;
			pthread_mutex_unlock(&moles_missed_mutex);
		}
		board[row][column] = '_';

		sem_post(&mole_active_enter);
	}

	return 0;
}

/******************************************************
* Track player input to change state. If esc is pressed
* change isPlaying state
*******************************************************/
void* gameTracker(void *params){
	int i,j;
	int isMiss;
 	while(1){
 		isMiss = 0;
		char input;
		if((input = getchar()) == 0x1B){ 	// Escape key
			isPlaying = 1;
			break;
		}
		for(i = 0; i < boardWidth; i++){
			for(j = 0; j < boardHeight; j++){
				if(board[i][j] == input){
					molesHit++;
					board[i][j] = '_';
					isMiss = 1;
				} 
			}
		}
		if(isMiss == 0){
			inaccurateHits++;
		}

	}
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
			printw("%c", board[i][j]);
			refresh();
		}
	}
	// Draw updated stat
	move(10,1);
	printw("MOLES SMACKED: %d", molesHit);
	refresh();
	move(11,1);
	printw("MOLES MISSED: %d", molesMissed);
	refresh();
	move(12,1);
	printw("CRAPPY SWINGS: %d", inaccurateHits);
	refresh();

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
	refresh();
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
		// TODO: Add a 'max' and 'min' time duration
		case EASY:
			mole_down_duration = 7;
			mole_up_duration = 7;
		case MEDIUM:
			mole_down_duration = 5;
			mole_up_duration = 5;
		case HARD:
			mole_down_duration = 3;
			mole_up_duration = 3;
		default:
			mole_down_duration = 7;
			mole_up_duration = 7;
	}
}