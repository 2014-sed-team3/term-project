public class Composition{
	private static final int maxSize = 100;
	private Component[] components=new Component[maxSize];
	private Line[] lines = new Line[maxSize];
	
	public arrange(Strategy strategy){
		return strategy.arrang(components);
	}
}
