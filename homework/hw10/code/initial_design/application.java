package hw10;
import java.util.*;

public class application {
	private ArrayList<Document> docs = new ArrayList<Document>();
	
	public void PresentDocuments(int[] docID){
		for(int i=0;i<docID.length;i++){
			docs.get(i).present();
		}
	}
	public void CreateDocument(String type){
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
	public void ManageDocument(int docID){
	}
	
}