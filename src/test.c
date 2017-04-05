#include <stdio.h>

int main(){
	int whacked = 30.0;
	int missed = 8.0;
	int inaccurate = 15;

	printf("test division:%.2f%%", whacked/missed);

	return 0;
}