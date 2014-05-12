public class GUIapplication{
	public Wigdet[] widgets;
	public void switch_type(WidgetFactory style){
		this.widgets = new Widget[];
		this.widgets.append(style.createWindow());
		this.widgets.append(style.createScrollBar());
		this.widgets.append(style.createButton());
	}
}
