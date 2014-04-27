public class TextView{
	String text;
	ScrollBar sb;
	ThickBlackBorder tb;
	public void display(Window w){
		w.display(text);
	}
	public void addScrollBar(ScrollBar s){
		sb = s;
	}
	public void addThickBlackBorder(ThickBlackBorder b){
		tb = b;
	}
	public void openFile(String path, String format){	
		switch(format){
				case format1:	text = readFormat1(path);
								break;
				case format2:	text = readFormat2(path);
								break;
		}
	}
}

public class ScrollBar{
	public ScrollBar(){
		//...
	}
}

public class ThickBlackBorder{
	public ThickBlackBorder(){
		//...
	}
}