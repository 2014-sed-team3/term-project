public class TextComposition implements Strategy{
	private static int MaxLine = 150;
	private Paragraph[] paragraphs = new Paragraph[maxLine];	// Where can I get it?
	
	public Line[] arrange(Component[] components){
		Line[] lines = new Line[MaxLine];
		int lineHead = 0;
		for(int i=0;i<paragraphs.length;i++){
			for(int j=0;j<paragraphs[i].getComponents().length;j++){
				lines[lineHead].add(components[j]);
			}
			lineHead++;
		}
		return lines;
	}
}

public class Paragraph{
	private static int MaxLine = 150;
	private Component[] components = new Component[MaxLine];
	
	public Component[] getComponents(){
		return components;
	}
}