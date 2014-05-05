package hw10_refactory;

public class TextDocumentCreator implements DocumentCreator {
	public Document CreateDocument(){
		return new TextDocument();
	}
}
