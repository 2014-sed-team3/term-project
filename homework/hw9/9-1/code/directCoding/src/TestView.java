
public class TestView {

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
	
		public void readFormat1(String path){
			
		}
		public void readFormat2(String path){
			
		}
}
