public class GUIApplication{
	private ArrayList<Widget> widgets = new ArrayList<Widget>();
	private String standard;
	private String style;
	
	public void createWidget(String type, String style){
		this.style = style if(needed);
		if(type.isEqual('Window') && standard.isEqual('Motif')){
			this.Widget.add(new MotifWindow(style));
		}else if(type.isEqual('Window') && standard.isEqual('PresentationManager')){
			this.Widget.add(new PresentationManagerWindow(style));
		}else if(type.isEqual('Button') && standard.isEqual('Motif')){
			this.Widget.add(new MotifButton(style));
		}else if(type.isEqual('Button') && standard.isEqual('PresentationManager')){
			this.Widget.add(new PresentationManagerButton(style));
		}else if(type.isEqual('ScrollBar') && standard.isEqual('Motif')){
			this.Widget.add(new MotifScrollBar(style));
		}else if(type.isEqual('ScrollBar') && standard.isEqual('PresentationManager')){
			this.Widget.add(new PresentationManagerScrollBar(style));
		}
	}
}
//Look-and-feel standards
public interface Motif{
}
public interface PresentationManager{
}

//Widget interface
public interface Widget{
}

//Window
public abstract class Window implements Widget{
	private String style;
}
public class MotifWindow extends Window implements Motif{
	public MotifWindow(String style){
		this.style = style;
	}
}
public class PresentationManagerWindow extends Window implements PresentationManager{
}

//ScrollBar
public class ScrollBar extends Widget{
}
public class MotifScrollBar extends ScrollBar implements Motif{
}
public class PresentationManagerScrollBar extends ScrollBar implements PresentationManager{
}

//Button
public class Button extends Widget{
}
public class MotifButton extends Button implements Motif{
}
public class PresentationManagerButton extends Button implements PresentationManager{
}