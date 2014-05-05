package hw10_refactory;

import java.util.ArrayList;

public class application {
	private ArrayList<Document> docs = new ArrayList<Document>();
	
	public void PresentDocuments(int[] docID){
		for(int i=0;i<docID.length;i++){
			docs.get(i).present();
		}
	}
	public void CreateDocument(DocumentCreator creator){
		docs.add(creator.CreateDocument());
	}
	public void ManageDocument(int docID){
	}
	
}