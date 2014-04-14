JCC=javac
TARGET=*.java types/*.java

bin/game.class:
	${JCC} ${TARGET} -d bin/

all: bin/game.class
run: bin/game.class
	java -cp bin/ game
clean:
	rm -rf bin/*.class 
