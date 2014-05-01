class application{
	private ArrayList<Document> docs;
	
	public void manage(){
		//managing documents
	}
	public void create(String type){
		switch type{
			case 'DrawingDocument':	docs.append(new DrawingDocument());
									break;
			case 'TextDocument'	: docs.append(new TextDocument());
								  break;
			default : docs.append(new Document());
					  break;
		}
	}
	public present(){
		for(i=0;i<docs.length;i++)
			docs[i].present();
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