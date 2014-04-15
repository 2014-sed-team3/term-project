import java.lang.String;

public class Component{
	private String text;
	private Graph graph;
	private int size=new int[2];
	private float stretchability;	// how much the component can grow
	private float shrinkability;	// how much the component can shrink
	private float scale; // displaying size = size * scale
}
