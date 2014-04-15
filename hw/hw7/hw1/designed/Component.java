import java.lang.String;

public class Component{
	private String text;
	private Graph graph;
	private int naturalSize=new int[2];
	private float stretchability;	// how much the component can grow
	private float shrinkability;	// how much the component can shrink
	private float scale; // displaying size = size * scale
	
	public boolean grow(float newScale){
		if(newScale<1 || newScale>stretchability){
			return false;
		}else{
			scale = newScale;
			return true;
		}
	}
	public boolean shrink(float newScale){
		if(newScale>1 || newScale<shrinkability){
			return false;
		}else{
			scale = newScale;
			return true;
		}
	}
}
