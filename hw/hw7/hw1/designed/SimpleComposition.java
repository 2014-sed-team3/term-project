public class SimpleComposition implements Strategy{
	private static int MaxLine = 150;
	public Line[] arrange(Component[] components){
		Line[] lines = new Line[MaxLine];
		int lineHead = 0;
		for(int i=0;i<components.length;i++){
			if(addLineBreak())
				lineHead ++;
			else
				lines[lineHead].add(components[i]);
		}
		return lines;
	}
	public boolean addLineBreak(){
		// Method to determine
	}
}