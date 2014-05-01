
public class Table {
	private spreadsheetData mData;
	public void show(spreadsheetData newData){
		mData=newData;
		//show newData on screen
		System.out.format("table\n%f\n",mData.mData[0][0]);
	}
	public void onUpdate(){
		//modified mData
		mData.mData[0][0]=5;
		mData.onUpdate();
	}
	
}
