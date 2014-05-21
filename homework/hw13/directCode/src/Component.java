import java.util.ArrayList;


public class Component extends BasicComponent {
	private ArrayList<BasicComponent> child;
	public Component(){
		child=new ArrayList<BasicComponent>();
	}
	public void addChild(BasicComponent b){
		child.add(b);
	}
}
