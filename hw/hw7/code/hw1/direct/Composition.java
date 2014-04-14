public class Composition{
	private static final int cSize = 100;
	private int rowMax=10;	 // for strat. 3
	private int[] paragraphBreak = new int[cSize];	// for strat.2 
	private Component[] components=new Component[cSize];
	private int componentsHead;
	private int strategy;
	private boolean[] linebreaks;

	public Line[] arrange(){
		int i;
		// Simple 
		if(strategy==1){
			for(i=0;i<componentsHead;i++){
				linebreaks[i]=true;
			}
		}
		// Text 
		else if(strategy==2){
			for(i=0;i<paragraphBreak.length;i++){
				linebreaks[paragraphBreak[i]] = true;
			}
		}
		// Array
		else if(strategy==3){
			for(i=0;i<componentsHead;i+=rowMax){
				linebreaks[i]=true;
			}
		}
	}
}
