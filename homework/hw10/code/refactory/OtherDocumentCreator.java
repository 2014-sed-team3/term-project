package hw10_refactory;

public class OtherDocumentCreator implements DocumentCreator {
	public Document CreateDocument(){
		return new OtherDocument();
	}
}
