package hw10_refactory;

public class DrawingDocumentCreator implements DocumentCreator {
	public Document CreateDocument(){
		return new DrawingDocument();
	}
}
