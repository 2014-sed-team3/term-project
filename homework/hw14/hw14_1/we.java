public class we{
	private l = new List();
	private sl = new SkipList();
	
	public void traverse(){
		int i;
		for(i=0;i<l.length;i++){
			System.out.println(l.get(i));
		}
		for(i=0;i<sl.size();i++){
			System.out.println(sl.get(i));
		}
	}
}