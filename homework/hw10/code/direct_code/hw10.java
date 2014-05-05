package hw10;
import java.util.*;

class application{
	private ArrayList<Document> docs = new ArrayList<Document>();

	public void manage(){
		//managing documents
	}
	public void create(String type){
		switch(type){
			case "DrawingDocument": 
				docs.add(new DrawingDocument());
				break;
			case "TextDocument"	: 
				docs.add(new TextDocument());
				break;
			default : 
				break;
		}
	}
	public void present(){
		for(int i=0;i<docs.size();i++)
			docs.get(i).present();
	}
}

abstract class Document{
	public void present(){
	}
}

class DrawingDocument extends Document{
}

class TextDocument extends Document{
}