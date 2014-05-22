import java.util.ArrayList;
import java.util.Arrays;

public class SchematicCaptureApp {

	/**
	 * @param args
	 */
	Canvas canvas;
	
	public Component groupComponent(ArrayList<? extends BasicComponent> componentList){
		Component c= new Component();
		for (int i=0;i<componentList.size();i++){
			componentList.get(i).setParent(c);
			c.addChild(componentList.get(i));
		}
		
		return c;
	}	
	public void drawComponent(BasicComponent b){
		if(canvas.component==null){
			canvas.component=new ArrayList<BasicComponent>();
		}
		canvas.component.add(b);
	}
	
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		/*
		Text t=new Text();
		Line l=new Line();
		Rectangle r=new Rectangle();
		Text t2=new Text();
		Line l2=new Line();
		Rectangle r2=new Rectangle();
		ArrayList<BasicComponent> al1= new ArrayList<BasicComponent>();
		al1.add(t);al1.add(l);al1.add(r);
		ArrayList<BasicComponent> al2= new ArrayList<BasicComponent>();
		al2.add(r2);al2.add(l2);al2.add(t2);
		
		Component c = groupComponent(al1);
		Component c2 = groupComponent(al2);
		ArrayList<Component> al3=new ArrayList<Component>();
		al3.add(c);al3.add(c2);
		Component c3 = groupComponent(al3);
		*/
	}


	
}
