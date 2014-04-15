public class ArrayComposition implements Strategy{
	private static int MaxLine = 150;
	private static int numElementsInRow=10;
	
	public setNumElementsInRow(int in){
		this.numElementsInRow = in;
	}
	public Line[] arrange(Component[] components){
		int count = 0;
		Line[] lines = new Line[MaxLine];
		int lineHead = 0;
		for(int i=0;i<components.length;i++){
			if(count++ <= numElementsInRow){
				lines[lineHead].add(component);
				count = 0;
			}else{
				lineHead ++;
			}
		}
		return lines;
	}
}